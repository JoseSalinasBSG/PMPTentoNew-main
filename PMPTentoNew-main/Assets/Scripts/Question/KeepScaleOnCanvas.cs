using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeepScaleOnCanvas : MonoBehaviour
{
    [SerializeField] private ScriptableObjectInstructor _instructors;
    [SerializeField] private ScriptableObjectUser _user;
    //[SerializeField] private GameObject _instructorPrefab;
    [SerializeField] private Camera _camera;
    //[SerializeField] private Transform _guideTransform;//extrae la transformada del personaje en escena
    private Transform _guideTransform;
    [SerializeField] private Transform _referenceInstructor;//variable para usar su posicion como referencia al momento de instanciar el prefab del instructor

    //[SerializeField] private Vector3 _initScale;
    [SerializeField] private float  _factor = 2;
    [SerializeField] private RectTransform _topLimit;

    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minResolution;
    [SerializeField] private float _maxResolution;
    [SerializeField] private Transform _pointOfInstantiate;//transform del punto donde se instanciara el prefab del instructor

    
    //public float x;
    private bool _lost;
    private static readonly int s_Sad = Animator.StringToHash("sad");

    public bool Lost
    {
        get => _lost;
        set => _lost = value;
    }
    private void Start()
    {
        //_initScale = _guideTransform.localScale;


        _guideTransform =
            Instantiate(_instructors.instructors[_user.userInfo.idInstructor].prefab.transform.GetChild(0), _referenceInstructor.position, _referenceInstructor.rotation, _pointOfInstantiate).transform;//instancio el hijo del prefab que esta en el scriptable object Instructors e igualara su transformada, instanciara como hijo de points of instantiate
        
        //Instantiate(_instructors.instructors[1].prefab, transform.position, Quaternion.Euler(new Vector3(8.3f, 180f, 0f)), transform.parent).transform;
        //_guideTransform.localRotation= Quaternion.Euler(new Vector3(0f, 180f, 0f));
        
        RectTransform r_pointOfInstantiate=_pointOfInstantiate.GetChild(0).GetComponent<RectTransform>();//obtenemos la componente RectTransform del hijo del gameObject PointOfInstantiate y lo guardamos en una variable del tipo RectTransform
        RectTransform r_referenceInstructor=_referenceInstructor.GetComponent<RectTransform>();//guardamos la componente RectTransform de _referenceInstructor

        r_pointOfInstantiate.sizeDelta = new Vector2(r_referenceInstructor.sizeDelta.x,r_referenceInstructor.sizeDelta.y);//asignamos el Width y Height del RectTransform de referenceInstructor al RectTransform de PointOfInstantiate
      

        

        /*

        var anim = _guideTransform.GetComponent<Animator>();
        if (_lost)
        {
            anim.SetTrigger(s_Sad);
        }
        else
        {
            anim.SetTrigger($"happy{Random.Range(1,3)}");
        }*/
    }

   


    private void Update()
    {
        if (_camera != null && _guideTransform != null)//si ninguno es nulo
        {
         
            //_guideTransform.position = transform.position;//igualar guideTransform a la transformada del GameObject de este script
            //var y = transform.position.z - _topLimit.position.z;//calcular y
            //Debug.Log(y);

            //x = (y * (_minResolution - _maxResolution) + _maxResolution * _minHeight - _minResolution *  _maxHeight) / (_minHeight - _maxHeight);
            //_guideTransform.localScale = Vector3.one * x;//setear con el valor de la variable x en el x, y y z de la escala de Guide Transform
        }
    }
}
