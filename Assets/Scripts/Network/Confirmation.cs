using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour
{
    public GameObject UI;
    public Button ConfirmButton;
    public Button CancelButton;
    
    private void Start()
    {
        UI.SetActive(false);
        CancelButton.onClick.AddListener(Hide);
    }

    public void Display()
    {
        UI.SetActive(true);
    }

    public bool IsDisplaying()
    {
        return UI.activeSelf;
    }

    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Confirm()
    {
        ConfirmButton.onClick.Invoke();
    }

    public void Cancel()
    {
        CancelButton.onClick.Invoke();
    }
}
