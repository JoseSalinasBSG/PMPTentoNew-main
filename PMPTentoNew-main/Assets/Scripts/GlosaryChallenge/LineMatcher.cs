using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LineMatcher : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private RectTransform[] lineImageRectTransforms;
    [SerializeField] private RectTransform startObject = null;
    [SerializeField] private RectTransform endObject = null;
    [SerializeField] private UnityEvent OnEndObjectSelected;

    private int currentIndex = 0;

    public void Draw()
    {
        if (startObject != null && endObject != null)
        {
            DrawLine();
            CleanLinesReferences();
        }
    }

    public void CleanLinesReferences()
    {
        startObject = null;
        endObject = null;
    }

    public void RestartDefaultValues()
    {
        currentIndex = 0;
        DisableLines();
    }

    private void DisableLines()
    {
        foreach (var line in lineImageRectTransforms)
        {
            line.gameObject.SetActive(false);
        }
    }
    public void SetStartObject(RectTransform startPosition)
    {
        if (startObject == null)
        {
            startObject = startPosition;
        }
        else
        {
            SetEndObject(startPosition);
        }
    }

    public void SetEndObject(RectTransform endPosition)
    {
        endObject = endPosition;
        OnEndObjectSelected?.Invoke();
    }

    private void DrawLine()
    {
        lineImageRectTransforms[currentIndex].gameObject.SetActive(true);
        // Get the positions of the start and end objects in the canvas space
        Vector2 start = GetCanvasPosition(startObject);
        Vector2 end = GetCanvasPosition(endObject);

        // Set the position to the midpoint of the start and end points
        lineImageRectTransforms[currentIndex].anchoredPosition = (start + end) / 2f;

        // Calculate the length of the line
        float length = Vector2.Distance(start, end);

        // Set the size of the line
        lineImageRectTransforms[currentIndex].sizeDelta = new Vector2(length, lineImageRectTransforms[currentIndex].sizeDelta.y);

        // Calculate the angle of rotation
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;

        // Set the rotation of the line
        lineImageRectTransforms[currentIndex].rotation = Quaternion.Euler(0, 0, angle);

        currentIndex++;
    }

    private Vector2 GetCanvasPosition(RectTransform uiElement)
    {
        // Convert the UI element's position to canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            uiElement.position,
            null,
            out localPoint);
        return localPoint;
    }
}
