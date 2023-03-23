using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIScreens { Welcome, MainMenu, LabelModule, NerveCuttingModule, DiagnosingModule };

[System.Serializable]
public struct UIPanels
{
    public GameObject LeftPanel;
    public GameObject RightPanel;
    public UIScreens UIScreenType;
    public Manager manager;
}

public class MenuManager : MonoBehaviour
{
    public GameObject RoomUI;
    RoomNetworkManager RoomNetworkManager;
    [Header("UI Screens")]
    [SerializeField]
    [Tooltip("Specify which UI panel to appear when starting scene.")]
    UIScreens StartScreen = UIScreens.Welcome;
    [SerializeField]
    [Tooltip("Select left and right UI panel references, and specify which type of UI Screen it is. (You don't have to have all panels filled)")]
    UIPanels[] UIScreen;
    [Header("Rotation Arrows")]
    [SerializeField]
    [Tooltip("Rotation arrows used for modules. Currently doesn't appear in Main Menu and Welcome Screen.")]
    GameObject[] RotateArrows;
    [Header("Pedastal")]
    [SerializeField]
    [Tooltip("Pedastal to show under canine leg model when a model is being shown.")]
    GameObject Pedastal;

    UIScreens CurrentScreen;
    Dictionary<UIScreens, UIPanels> UIDictionary;

    void Awake()
    {
        RoomNetworkManager = FindObjectOfType<RoomNetworkManager>();

        CurrentScreen = StartScreen;
        UIDictionary = new Dictionary<UIScreens, UIPanels>();

        Debug.Log("The UIScreen array has " + UIScreen.Length + " amount of UIScreens");

        foreach(UIPanels panel in UIScreen)
        {
            UIDictionary.Add(panel.UIScreenType, panel);
            Debug.Log("The UIPanel added is " + panel.UIScreenType);
            if (panel.UIScreenType == StartScreen)
                ChangeUIVisuals(true, panel);
            else
                ChangeUIVisuals(false, panel);
        }
        ChangeArrowVisibility();
        ChangePedastalVisibility();
    }

    //Solution for now. Implement visibility animation later if there's time
    void ChangeUIVisuals(bool isVisible, UIPanels panel)
    {
        if (panel.LeftPanel != null)
            panel.LeftPanel.SetActive(isVisible);
        if (panel.RightPanel != null)
            panel.RightPanel.SetActive(isVisible);

        if (isVisible && panel.manager != null)
            panel.manager.InitiateSection();
        else if (!isVisible && panel.manager != null)
            panel.manager.TerminateSection();
    }

    public void ChangeUIScreen(int newUIScreen)
    {
        try
        {
            ChangeUIScreen((UIScreens)newUIScreen);
        }
        catch
        {
            Debug.LogError("UI screen doesn't exist. Check integer value.");
        }
    }

    public void ChangeUIScreen(UIScreens newUIScreen)
    {
        UIPanels newPanel;
        UIPanels oldPanel;

        Debug.Log("The ChangeUIScreen function has been called!");
        Debug.Log("The CurrentScreen is " + CurrentScreen);
        Debug.Log("The newUIScreen is  " + newUIScreen);

        if (CurrentScreen == newUIScreen)
        {
            Debug.Log("The newUIScreen is the same as the CurrentScreen, so nothing will happen.");
            return;
        }

        if (UIDictionary != null && UIDictionary.TryGetValue(CurrentScreen, out oldPanel) && UIDictionary.TryGetValue(newUIScreen, out newPanel))
        {
            Debug.Log("ChangeUIVisuals is about to start!");
            ChangeUIVisuals(false, oldPanel);
            Debug.Log("ChangeUIVisuals should have worked...");
            ChangeUIVisuals(true, newPanel);
            CurrentScreen = newUIScreen;
            ChangeArrowVisibility();
            ChangePedastalVisibility();
        }
    }

    private void ChangeArrowVisibility()
    {
        bool IsVisible = !(CurrentScreen == UIScreens.Welcome || CurrentScreen == UIScreens.MainMenu);

        foreach (GameObject arrow in RotateArrows)
        {
            arrow.SetActive(IsVisible);
        }
    }

    private void ChangePedastalVisibility()
    {
        bool IsVisible = !(CurrentScreen == UIScreens.Welcome || CurrentScreen == UIScreens.MainMenu);
        Pedastal.SetActive(IsVisible);
    }

    public UIScreens GetStartScreen()
    {
        return StartScreen;
    }

    public UIScreens GetCurrentScreen()
    {
        return CurrentScreen;
    }

    public void Reset()
    {
        ChangeUIScreen(StartScreen);
    }
}