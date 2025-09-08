using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Resuelve el ítem ganador al terminar el giro (por distancia puntero ↔ ícono),
/// muestra el popup y aplica la recompensa al usuario.
/// Requiere: RouletteStateAndSpin (evento OnSpinFinished) y RouletteItem con IconRect expuesto.
/// </summary>
public sealed class RoulettePrizeResolver : MonoBehaviour
{
    [Header("Dependencias")]
    [Tooltip("Controlador de animación/estados de la ruleta. Me suscribo a OnSpinFinished.")]
    [SerializeField] private RouletteStateAndSpin stateAndSpin;

    [Tooltip("SO del usuario para otorgar las recompensas.")]
    [SerializeField] private ScriptableObjectUser userSO;

    [Header("Estructura de la ruleta")]
    [Tooltip("Transform del contenedor de los ítems (Item1..Item10).")]
    [SerializeField] private Transform itemsContainer;
   
    [Tooltip("Transform del PUNTERO (flecha a la izquierda). Se mide distancia icono ↔ puntero.")]
    [SerializeField] private Transform pointer;

    private int sliceCount = 10;

    [Header("Popup de recompensa")]
    [Tooltip("Contenedor del popup (puede ser FadeUI o un simple GameObject).")]
    [SerializeField] private GameObject rewardPopup;

    [Tooltip("Imagen del ícono del premio dentro del popup.")]
    [SerializeField] private Image rewardIcon;

    [Tooltip("Label de cantidad dentro del popup (formato xN).")]
    [SerializeField] private TMP_Text rewardAmountLabel;

    // --- Internos ---
    private readonly List<RouletteItem> _items = new List<RouletteItem>();
    private RouletteItem _winner;

    private void OnEnable()
    {
        if (stateAndSpin != null)
            stateAndSpin.OnSpinFinished.AddListener(HandleSpinFinished);

        CollectItems();
    }

    private void OnDisable()
    {
        if (stateAndSpin != null)
            stateAndSpin.OnSpinFinished.RemoveListener(HandleSpinFinished);
    }

    [ContextMenu("Collect Items")]
    private void CollectItems()
    {
        _items.Clear();
        if (itemsContainer == null) { itemsContainer = transform; }

        for (int i = 0; i < itemsContainer.childCount; i++)
        {
            var ri = itemsContainer.GetChild(i).GetComponent<RouletteItem>();
            if (ri != null) _items.Add(ri);
        }
    }

    private void HandleSpinFinished()
    {
        if (_items.Count != sliceCount)
        {
            Debug.LogError($"{nameof(RoulettePrizeResolver)}: Se esperaban {sliceCount} items y hay {_items.Count}. Revisa el contenedor.");
            return;
        }
        if (pointer == null)
        {
            Debug.LogError($"{nameof(RoulettePrizeResolver)}: Falta asignar el 'pointer' (flecha).");
            return;
        }

        _winner = ResolveWinnerByIconDistance(pointer, _items);
        if (_winner == null)
        {
            Debug.LogError("No se pudo resolver un ganador (lista vacía o sin IconRect).");
            return;
        }

        // Otorgar premio + configurar popup
        GrantPrizeToUser(_winner);
        ShowRewardPopup(_winner);
    }

    /// <summary>
    /// Devuelve el RouletteItem cuya posición de ícono esté más cerca del puntero.
    /// Usa posiciones en mundo (rectTransform.position).
    /// </summary>
    private static RouletteItem ResolveWinnerByIconDistance(Transform pointerTf, List<RouletteItem> items)
    {
        RouletteItem winner = null;
        float bestSqr = float.MaxValue;
        Vector3 p = pointerTf.position;

        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            if (it == null || !it.HaveInformation) continue;
            var iconRect = it.IconRect;
            if (iconRect == null) continue;

            float sqr = (iconRect.position - p).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                winner = it;
            }
        }
        return winner;
    }

    /// Otorga la recompensa (power-up o monedas) al usuario.
    private void GrantPrizeToUser(RouletteItem item)
    {
        if (userSO == null || item == null || item.RouletteItemData == null || item.RouletteItemData._ItemRouletteSo == null)
        {
            Debug.LogWarning("No se pudo otorgar premio: referencias nulas.");
            return;
        }

        var so = item.RouletteItemData._ItemRouletteSo;

        // Coins
        if (so.GetType() == typeof(CoinsItemRoulette))
        {
            userSO.AddCoins(item.Amount);
        }
        // PowerUp
        else if (so.GetType() == typeof(PowerUpItemRoulette))
        {
            var p = (PowerUpItemRoulette)so;
            userSO.AddPowerUp(p.powerUpSO, item.Amount);
        }
        else
        {
            Debug.LogWarning($"Tipo de ItemRouletteSO no soportado: {so.GetType().Name}");
        }

        // Si tienes eventos que refrescan UI de usuario:
        GameEvents.RequestUpdateDetail?.Invoke();
    }

    /// Configura y muestra el popup con sprite y cantidad.
    private void ShowRewardPopup(RouletteItem item)
    {
        if (rewardPopup == null) return;

        bool hasInfo = item != null && item.HaveInformation && item.RouletteItemData != null && item.RouletteItemData._ItemRouletteSo != null;

        if (rewardIcon != null)
        {
            rewardIcon.gameObject.SetActive(hasInfo);
            rewardIcon.sprite = hasInfo ? item.RouletteItemData._ItemRouletteSo.spriteIconPowerUp : null;
        }

        if (rewardAmountLabel != null)
        {
            rewardAmountLabel.gameObject.SetActive(hasInfo);
            rewardAmountLabel.text = hasInfo ? $"+{item.Amount}" : string.Empty;
        }

        rewardPopup.SetActive(true);
        rewardPopup.GetComponent<FadeUI>()?.FadeInTransition();
    }

    /// Llama desde el botón "Continuar" del popup para cerrarlo.
    public void CloseRewardPopup()
    {
        if (rewardPopup != null) rewardPopup.SetActive(false);

        if (stateAndSpin != null)
        {
            stateAndSpin.StartCooldownNow();
        }
    }
}
