using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyReviewNameListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameQuestion;

    public void SetData(string nameQuestion)
    {
        _nameQuestion.text = nameQuestion;
    }
}
