using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsGroup : MonoBehaviour
{
    [SerializeField] private GlosaryChallengeController _glosaryChallengeController;
    [SerializeField] private List<OptionGC> _options;

    private OptionGC _oldSelectedButton;

    public List<OptionGC> Options
    {
        get => _options;
        set => _options = value;
    }

    public OptionGC OldSelectedButton
    {
        get => _oldSelectedButton;
        set
        {
            _oldSelectedButton = value; 
            _glosaryChallengeController.Evaluate();
        }
    }
    public void SelectButton(OptionGC buttonSelected)
    {
        if (buttonSelected.Equals(OldSelectedButton))
        {
            buttonSelected.DeselectButton();
            OldSelectedButton = null;
            return;
        }
        if (OldSelectedButton)
        {
            OldSelectedButton.DeselectButton();
        }
        buttonSelected.SelectButton();
        OldSelectedButton = buttonSelected;
    }

    public void CleanOldSelected()
    {
        // OldSelectedButton.DeselectButton();
        OldSelectedButton = null;
    }

    public void StartCleanAnimationGroup()
    {
        CleanOldSelected();
        for (int i = 0; i < _options.Count; i++)
        {
            _options[i].StartAnimation();
        }
    }

}
