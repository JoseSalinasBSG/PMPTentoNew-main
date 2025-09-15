using UnityEngine;

public class KeepScaleOnCanvas : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _instructors;
    [SerializeField] private ScriptableObjectUser _user;
    [SerializeField] private Transform _referenceInstructor;    //variable para usar su posicion como referencia
    [SerializeField] private Transform _pointOfInstantiate; //transform del punto donde se instanciara el prefab del instructor


    private void Start()
    {
        //instancio el prefab que esta en el scriptable object Instructors
        Instantiate(_instructors.instructors[_user.userInfo.idInstructor].prefab.transform.GetChild(1), _referenceInstructor.position, _referenceInstructor.rotation, _pointOfInstantiate);
    }
}
