using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttempts : MonoBehaviour
{
   [SerializeField] private ScriptableObjectUser _user;

   private void Update()
   {
      for (int i = 0; i < _user.userInfo.LearningModeState.ItemStates.Count; i++)
      {
         var s = _user.userInfo.LearningModeState.ItemStates[i];
         for (int j = s.timesToRetrive.Count -1; j >=0 ; j--)
         {
            Debug.Log(s.timesToRetrive[j]);
            if (DateTime.Now > s.timesToRetrive[j])
            {
               Debug.Log($"intento para id {s.id} recuperado desde {s.timesToRetrive[i]}");
               s.timesToRetrive.RemoveAt(j);
               GameEvents.RecoveryAttempt?.Invoke(s.id);
            }
         }
      }
   }
}
