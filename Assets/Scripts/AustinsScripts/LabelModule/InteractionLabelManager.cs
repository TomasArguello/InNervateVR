using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LabelCategory { Bones, Muscles, Nerves };

[System.Serializable]
public struct LabelButton
{
    public Button UIButton;
    public LabelCategory ButtonCategory;
}

public class InteractionLabelManager : Manager
{
    [Header("UI References")]
    [SerializeField] Text TitleUI;
    [SerializeField] Text DescriptionUI;
    [SerializeField] LabelButton[] ButtonUI;
    [Header("Canine Labeling Leg")]
    [SerializeField] GameObject LabelLeg;
    [SerializeField] InteractionLabel[] InteractionLabelArray;

    public InteractionLabel[] GetInteractionLabelArray { get { return InteractionLabelArray; } }
    LabelCategory CurrentCategory;
    InteractionLabel CurrentLabel;

    public override void InitiateSection()
    {
        LabelLeg.SetActive(true);

        SetTitleAndDescriptionAlpha(0.0f);
        ResetUIElements(CurrentCategory);
    }

    public override void TerminateSection()
    {
        if (CurrentLabel != null)
            CurrentLabel.UnSelect();

        foreach (InteractionLabel label in InteractionLabelArray)
            label.gameObject.SetActive(false);

        LabelLeg.SetActive(false);
    }

    public void ChangeLabel(InteractionLabel newLabel)
    {
        if (CurrentLabel == newLabel)
            return;
        if (CurrentLabel != null)
            CurrentLabel.UnSelect();

        SetTitleAndDescriptionAlpha(0.0f);
        TitleUI.text = newLabel.GetLabelTitle;
        DescriptionUI.text = newLabel.GetLabelDescription;
        FadeTitleAndDescription(1.0f, 0.5f);

        CurrentLabel = newLabel;
    }

    public void ChangeLabelCategory(int newElement)
    {
        LabelCategory newCategory = (LabelCategory)newElement;

        if (CurrentCategory == newCategory)
            return;

        ResetUIElements(newCategory);
    }

    private void ResetUIElements(LabelCategory newCategory)
    {
        foreach (InteractionLabel label in InteractionLabelArray)
            label.gameObject.SetActive(label.GetLabelCategory == newCategory);

        FadeTitleAndDescription(0.0f, 0.25f);

        if (CurrentLabel != null)
        {
            CurrentLabel.UnSelect();
            CurrentLabel = null;
        }

        foreach(LabelButton button in ButtonUI)
            button.UIButton.interactable = (button.ButtonCategory != newCategory);

        CurrentCategory = newCategory;
    }

    private void FadeTitleAndDescription(float alpha, float time)
    {
        TitleUI.CrossFadeAlpha(alpha, time, false);
        DescriptionUI.CrossFadeAlpha(alpha, time, false);
    }

    private void SetTitleAndDescriptionAlpha(float alpha)
    {
        TitleUI.canvasRenderer.SetAlpha(alpha);
        DescriptionUI.canvasRenderer.SetAlpha(alpha);
    }
}