//old code:
// using System;
// using UnityEngine;

// public class CheckAttempts : MonoBehaviour
// {
//    [SerializeField] private ScriptableObjectUser _user;

//    private void Update()
//    {
//       for (int i = 0; i < _user.userInfo.LearningModeState.ItemStates.Count; i++)
//       {
//          var s = _user.userInfo.LearningModeState.ItemStates[i];
//          for (int j = s.timesToRetrive.Count - 1; j >= 0; j--)
//          {
//             if (DateTime.Now > s.timesToRetrive[j])
//             {
//                s.timesToRetrive.RemoveAt(j);
//                GameEvents.RecoveryAttempt?.Invoke(s.id);
//             }
//          }
//       }
//    }
// }

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Esta clase gestiona la verificación de intentos en el modo de aprendizaje del usuario.
/// 
/// Actualizaciones recientes (por Jose Salinas Vega, 19/12/2024):
/// - Se reemplazó el uso de `Update` por una corrutina para optimizar el rendimiento.
/// - Se agregó un sistema basado en intervalos configurables para verificar intentos.
/// - Se implementó la lógica para restablecer los intentos después de 3 minutos (para pruebas).
/// 
/// Notas para desarrolladores futuros:
/// - Ajustar `ResetThreshold` y `CheckIntervalSeconds` según las necesidades del entorno
///   (por ejemplo, usar `TimeSpan.FromDays(1)` para producción).
/// - Asegurarse de que las listas `ItemStates` y `timesToRetrive` estén correctamente inicializadas.
/// - Verificar que los eventos disparados (`GameEvents.RecoveryAttempt`) estén configurados
///   para manejar múltiples IDs si es necesario.
/// </summary>

public class CheckAttempts : MonoBehaviour
{
    [SerializeField] private ScriptableObjectUser _user;
    private const float CheckIntervalSeconds = 10f; // Verificar cada # segundos para pruebas
    private readonly TimeSpan ResetThreshold = TimeSpan.FromMinutes(30); 

    private void Start()
    {
        // Iniciar la corrutina para revisar intentos periódicamente
        StartCoroutine(CheckAttemptsRoutine());
    }

    private IEnumerator CheckAttemptsRoutine()
    {
        while (true)
        {
            foreach (var item in _user.userInfo.LearningModeState.ItemStates)
            {
                if (item.timesToRetrive.Count > 0)
                {
                    var oldestTime = item.timesToRetrive[0];

                    // Si ha pasado el umbral
                    if (DateTime.Now - oldestTime >= ResetThreshold)
                    {
                        // Restablecer el contador
                        item.timesToRetrive.Clear();
                        
                        // Disparar el evento
                        GameEvents.RecoveryAttempt?.Invoke(item.id);
                    }
                }
            }
            // Esperar antes de la próxima verificación
            yield return new WaitForSeconds(CheckIntervalSeconds);
        }
    }
}
