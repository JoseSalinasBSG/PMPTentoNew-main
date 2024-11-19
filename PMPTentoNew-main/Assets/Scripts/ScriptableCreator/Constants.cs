using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constant", menuName = "Constant")]
public class Constants : ScriptableObject
{
    public string errorFormatField = "El campo tiene que ser un correo (example@example.com)";
    public string fieldEmpty = "Ingresa tu correo electrónico";
}
