using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Genera la ruleta asignando color, ícono y cantidad a los 10 slices existentes.
/// - Usa RouletteSO como pool de datos.
/// - Probabilidad uniforme.
/// - Regla: no adyacentes iguales y el último distinto al primero.
/// </summary>
public sealed class RouletteVisualGenerator : MonoBehaviour
{
    [Header("Datos")]
    [Tooltip("SO con el pool de ítems de ruleta.")]
    [SerializeField] private RouletteSO _rouletteSO;

    [Header("Estructura")]
    [Tooltip("Contenedor con los 10 hijos (Item1..Item10). Si se deja vacío, se usa este mismo transform.")]
    [SerializeField] private Transform _itemsContainer;

    private int _sliceCount = 10;
    private List<RouletteItem> _items = new List<RouletteItem>();

    private void Start()
    {
        if (_rouletteSO == null || _rouletteSO.RouletteItems == null || _rouletteSO.RouletteItems.Length == 0)
        {
            Debug.LogError($"{nameof(RouletteVisualGenerator)}: RouletteSO sin datos.");
            return;
        }

        if (_itemsContainer == null) _itemsContainer = transform;

        _items.Clear();
        for (int i = 0; i < _itemsContainer.childCount; i++)
        {
            var ri = _itemsContainer.GetChild(i).GetComponent<RouletteItem>();
            if (ri != null) _items.Add(ri);
        }

        if (_items.Count != _sliceCount)
        {
            Debug.LogError($"{nameof(RouletteVisualGenerator)}: Se esperaban {_sliceCount} items y se encontraron {_items.Count}.");
            return;
        }

        Generate();
    }

    [ContextMenu("Regenerate")]
    public void Generate()
    {
        var pool = _rouletteSO.RouletteItems;
        var seq = BuildSequenceNoAdjacent(pool, _sliceCount);

        for (int i = 0; i < _sliceCount; i++)
        {
            var data = pool[seq[i]];
            _items[i].ApplyVisualFromSO(data); // asigna color + icono + cantidad
        }
    }

    /// <summary>
    /// Construye una secuencia de 'count' índices a 'pool' tal que:
    /// seq[i] != seq[i-1] y seq[count-1] != seq[0]. Probabilidad uniforme.
    /// </summary>
    private static List<int> BuildSequenceNoAdjacent(RouletteItemData[] pool, int count)
    {
        var rnd = new System.Random(Environment.TickCount);
        var result = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            int chosen = -1;
            int guard = 0;

            while (guard++ < 1000)
            {
                int cand = rnd.Next(0, pool.Length);

                if (i > 0 && pool[cand]._ItemRouletteSo == pool[result[i - 1]]._ItemRouletteSo)
                    continue;

                if (i == count - 1 && result.Count > 0 &&
                    pool[cand]._ItemRouletteSo == pool[result[0]]._ItemRouletteSo)
                    continue;

                chosen = cand;
                break;
            }

            if (chosen == -1)
            {
                // Fallback en caso extremo (pool muy pequeño)
                for (int j = 0; j < pool.Length; j++)
                {
                    bool okPrev = (i == 0) || pool[j]._ItemRouletteSo != pool[result[i - 1]]._ItemRouletteSo;
                    bool okFirst = (i != count - 1) || (result.Count == 0) || pool[j]._ItemRouletteSo != pool[result[0]]._ItemRouletteSo;
                    if (okPrev && okFirst) { chosen = j; break; }
                }
            }
            result.Add(chosen);
        }
        return result;
    }
}
