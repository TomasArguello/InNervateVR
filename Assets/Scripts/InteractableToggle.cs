using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableToggle : InteractableObject
{
    Toggle toggleRef;

    // Start is called before the first frame update
    void Awake()
    {
        toggleRef = this.gameObject.GetComponent<Toggle>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        //ButtonRef.targetGraphic.color = ButtonRef.colors.highlightedColor;
    }

    public override void OnTriggerExit(Collider other)
    {
        //ButtonRef.targetGraphic.color = ButtonRef.colors.normalColor;
    }

    public override void CallOnClickInteractable()
    {
        //toggleRef.isOn= True;
        toggleRef.isOn = !toggleRef.isOn;
    }
}
