using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableButton : InteractableObject
{
    Button ButtonRef;

    // Start is called before the first frame update
    void Awake()
    {
        ButtonRef = this.gameObject.GetComponent<Button>();
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        ButtonRef.targetGraphic.color = ButtonRef.colors.highlightedColor;
    }

    public override void OnTriggerExit(Collider other)
    {
        ButtonRef.targetGraphic.color = ButtonRef.colors.normalColor;
    }

    public override void CallOnClickInteractable()
    {
        ButtonRef.onClick.Invoke();
    }
}
