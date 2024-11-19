using UnityEngine;

public class RouletteWithDrag : MonoBehaviour
{
    public RectTransform rouletteTransform;
    public float initialRotationSpeed = 5f;
    public float continuousRotationSpeed = 30f; // Nueva velocidad de rotación continua
    public float minDragDistance = 10f;
    public float minDragTime = 0.1f;
    public float deceleration = 5f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float dragStartTime;
    private float dragEndTime;
    private float currentRotationSpeed;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    dragStartTime = Time.time;
                    currentRotationSpeed = 0f;
                    break;

                case TouchPhase.Moved:
                    touchEndPos = touch.position;

                    float dragDistance = Vector2.Distance(touchStartPos, touchEndPos);
                    float dragTime = Time.time - dragStartTime;

                    if (dragDistance >= minDragDistance && dragTime >= minDragTime)
                    {
                        Vector2 dragDirection = (touchEndPos - touchStartPos).normalized;
                        currentRotationSpeed = dragDistance / dragTime * initialRotationSpeed;
                    }
                    else
                    {
                        // Aplica rotación continua mientras se mantiene presionado
                        currentRotationSpeed = continuousRotationSpeed;
                    }
                    break;

                case TouchPhase.Ended:
                    dragEndTime = Time.time;
                    break;
            }
        }
        
        // Aplica rotación a la ruleta
        RotateRoulette();
    }

    void RotateRoulette()
    {
        // Gradualmente reduce la velocidad de rotación (deceleración)
        currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, deceleration * Time.deltaTime);

        // Rota la ruleta basada en la velocidad de rotación actual
        rouletteTransform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);
    }
}