using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Estados de la ruleta + animación de giro iniciada por botón.
/// No modifica textos: solo activa/desactiva readyGO/spinningGO/cooldownGO.
/// El cooldown (24h por defecto) se persiste en PlayerPrefs (UTC) y muestra HH:mm:ss en vivo.
/// </summary>
public sealed class RouletteStateAndSpin : MonoBehaviour
{
    private enum State { Ready, Spinning, Cooldown }

    [Header("Referencias")]
    [Tooltip("RectTransform del wheel (el círculo que rota).")]
    [SerializeField] private RectTransform wheel;

    [Header("UI States (solo activar/desactivar)")]
    [Tooltip("Objeto del estado LISTO con el botón de '¡Girar!'")]
    [SerializeField] private GameObject readyGO;
    [Tooltip("Botón dentro de readyGO que inicia el giro.")]
    [SerializeField] private UnityEngine.UI.Button readyButton;

    [Tooltip("Objeto del estado GIRANDO (visual '¡Girando!').")]
    [SerializeField] private GameObject spinningGO;

    [Tooltip("Objeto del estado COOLDOWN (contiene el label del temporizador).")]
    [SerializeField] private GameObject cooldownGO;
    [Tooltip("TMP_Text que muestra HH:mm:ss restante dentro de cooldownGO.")]
    [SerializeField] private TMP_Text cooldownLabel;

    [Header("Animación de giro")]
    [Tooltip("Duración del giro (s).")]
    [SerializeField, Min(0.3f)] private float spinDuration = 3f;
    [Tooltip("Revoluciones mínimas completas durante el giro.")]
    [SerializeField, Min(1)] private int minRevolutions = 3;
    [Tooltip("Revoluciones máximas completas durante el giro.")]
    [SerializeField, Min(1)] private int maxRevolutions = 5;
    [Tooltip("Curva de desaceleración (0→1).")]
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Tooltip("Sentido horario (true) o antihorario (false).")]
    [SerializeField] private bool clockwise = true;

    [Header("Cooldown")]
    [Tooltip("Horas de cooldown tras un giro.")]
    [SerializeField, Min(1)] private int cooldownHours = 24;
    [Tooltip("Clave PlayerPrefs para guardar el fin de cooldown en UTC (ISO8601).")]
    [SerializeField] private string ppNextAvailableUtcKey = "RouletteNextAvailableUtc";

    [Header("Control de cuándo iniciar el cooldown")]
    [Tooltip("Si true, entra a cooldown justo al terminar el giro. Si false, espera a que llames StartCooldownNow().")]
    [SerializeField] private bool startCooldownOnSpinFinish = true;

    [Header("Eventos")]
    public UnityEvent OnSpinStarted;
    public UnityEvent OnSpinFinished;
    public UnityEvent OnCooldownStarted;
    public UnityEvent OnCooldownEnded;

    // --- Estado interno ---
    private State state = State.Ready;
    private Coroutine spinCo;
    private Coroutine cdCo;
    private DateTime nextAvailableUtc;

    // === Lifecycle ===
    private void Awake()
    {
        if (readyButton != null) readyButton.onClick.AddListener(OnReadyClicked);
    }

    private void OnEnable()
    {
        if (LoadNextAvailableUtc(out nextAvailableUtc) && DateTime.UtcNow < nextAvailableUtc)
            EnterCooldown();
        else
            EnterReady();
    }

    private void OnDisable()
    {
        if (readyButton != null) readyButton.onClick.RemoveListener(OnReadyClicked);
        if (spinCo != null) { StopCoroutine(spinCo); spinCo = null; }
        if (cdCo != null) { StopCoroutine(cdCo); cdCo = null; }
    }

    // === Public API ===

    /// <summary>Inicia el cooldown inmediatamente (útil si desactivaste startCooldownOnSpinFinish).</summary>
    public void StartCooldownNow()
    {
        nextAvailableUtc = DateTime.UtcNow.AddHours(cooldownHours);
        SaveNextAvailableUtc(nextAvailableUtc);
        EnterCooldown();
    }

    /// <summary>Devuelve true si la ruleta está lista para girar.</summary>
    public bool IsReady() => state == State.Ready;

    // === Handlers ===
    private void OnReadyClicked()
    {
        if (state != State.Ready) return;
        if (wheel == null) { Debug.LogWarning("RouletteStateAndSpin: 'wheel' no asignado."); return; }
        EnterSpinning();
    }

    // === State transitions ===
    private void EnterReady()
    {
        state = State.Ready;
        SetActiveSafe(readyGO, true);
        SetActiveSafe(spinningGO, false);
        SetActiveSafe(cooldownGO, false);
        if (cooldownLabel) cooldownLabel.text = string.Empty;

        if (cdCo != null) { StopCoroutine(cdCo); cdCo = null; }
        OnCooldownEnded?.Invoke();
    }

    private void EnterSpinning()
    {
        state = State.Spinning;
        SetActiveSafe(readyGO, false);
        SetActiveSafe(spinningGO, true);
        SetActiveSafe(cooldownGO, false);

        OnSpinStarted?.Invoke();

        if (spinCo != null) StopCoroutine(spinCo);
        spinCo = StartCoroutine(SpinRoutine());
    }

    private void EnterCooldown()
    {
        state = State.Cooldown;
        SetActiveSafe(readyGO, false);
        SetActiveSafe(spinningGO, false);
        SetActiveSafe(cooldownGO, true);

        if (cdCo != null) StopCoroutine(cdCo);
        cdCo = StartCoroutine(CooldownRoutine());

        OnCooldownStarted?.Invoke();
    }

    // === Coroutines ===
    private IEnumerator SpinRoutine()
    {
        float startZ = wheel.eulerAngles.z;

        int revs = UnityEngine.Random.Range(minRevolutions, maxRevolutions + 1);
        float endAngle = UnityEngine.Random.Range(0f, 360f);
        float sign = clockwise ? -1f : 1f;               // en UI, horario suele ser negativo
        float delta = sign * (revs * 360f + endAngle);

        float t = 0f;
        float dur = Mathf.Max(spinDuration, 0.0001f);
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / dur;
            float k = ease.Evaluate(Mathf.Clamp01(t));
            wheel.rotation = Quaternion.Euler(0f, 0f, startZ + delta * k);
            yield return null;
        }

        wheel.rotation = Quaternion.Euler(0f, 0f, startZ + delta);
        spinCo = null;

        OnSpinFinished?.Invoke();

        if (startCooldownOnSpinFinish)
        {
            StartCooldownNow();
        }
        else
        {
            // Si no arranca cooldown aquí, volvemos a Ready y esperas a llamarlo cuando cierres el popup del premio.
            EnterReady();
        }
    }

    private IEnumerator CooldownRoutine()
    {
        while (true)
        {
            var remaining = nextAvailableUtc - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero) break;

            if (cooldownLabel != null)
                cooldownLabel.text = Format(remaining);

            yield return new WaitForSecondsRealtime(0.25f);
        }

        SaveNextAvailableUtc(DateTime.UtcNow); // opcional: pisa valor
        cdCo = null;
        EnterReady();
    }

    // === Persistencia ===
    private bool LoadNextAvailableUtc(out DateTime utcOut)
    {
        utcOut = DateTime.MinValue;
        if (!PlayerPrefs.HasKey(ppNextAvailableUtcKey)) return false;
        var raw = PlayerPrefs.GetString(ppNextAvailableUtcKey, string.Empty);
        if (string.IsNullOrEmpty(raw)) return false;

        if (DateTime.TryParse(raw, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsed))
        {
            utcOut = parsed.ToUniversalTime();
            return true;
        }
        return false;
    }

    private void SaveNextAvailableUtc(DateTime utc)
    {
        PlayerPrefs.SetString(ppNextAvailableUtcKey, utc.ToUniversalTime().ToString("o"));
        PlayerPrefs.Save();
    }

    // === Utils ===
    private static void SetActiveSafe(GameObject go, bool value)
    {
        if (go != null && go.activeSelf != value) go.SetActive(value);
    }

    private static string Format(TimeSpan ts)
    {
        if (ts < TimeSpan.Zero) ts = TimeSpan.Zero;
        int hours = (int)ts.TotalHours;
        return $"{hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
    }
}
