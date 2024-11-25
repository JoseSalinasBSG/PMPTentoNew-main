using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassScrollEvents : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    public ScrollRect scrollRect;
    private bool isDragging=false;
    [SerializeField] UnityEvent onPointerClick;
    [SerializeField] bool hasScrollHorizontal=false;

    void Start()
    {
        if (hasScrollHorizontal)
        {
            scrollRect = transform.parent.GetComponentInParent<ScrollRect>();//para no obtener el scrollrect del mismo gameobject empieza a buscar a partir del primer padre
        }
        else
        {
            scrollRect=GetComponentInParent<ScrollRect>();//scroll rect de padre mas cercano
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnBeginDrag(eventData);
            isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnDrag(eventData);
            isDragging = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnEndDrag(eventData);
            isDragging = false;//indicador de que se esta arrastrando en falso
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging)//si es true
        {
            return;//cancela OnPointerClick
        }
        Debug.Log("OnPointerClick");
        onPointerClick?.Invoke();//llama al evento onPointerClick y a todos sus metodos suscritos en el inspector
    }

}
