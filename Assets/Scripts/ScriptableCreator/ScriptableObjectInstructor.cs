using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Instructors", fileName = "Instructors")]
public class ScriptableObjectInstructor : ScriptableObject
{
    public Instructor[] instructors;
    public Instructor currentInstructor;
}
