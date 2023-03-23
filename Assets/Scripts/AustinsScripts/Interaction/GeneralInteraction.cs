using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public abstract class GeneralInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] protected Color NormalEmissionColor;
    [SerializeField] protected Color HighlightedEmissionColor;
    [SerializeField] protected Color SelectedEmissionColor;

    protected MeshRenderer InteractableMesh;
    protected bool IsSelected;

    protected void Awake()
    {
        InteractableMesh = GetComponent<MeshRenderer>();

        EventTrigger Trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry EntrySelect = new EventTrigger.Entry();
        EventTrigger.Entry EntryOnHover = new EventTrigger.Entry();
        EventTrigger.Entry EntryOnUnhover = new EventTrigger.Entry();

        EntrySelect.eventID = EventTriggerType.PointerClick;
        EntrySelect.callback.AddListener((data) => { Select((PointerEventData)data); });
        Trigger.triggers.Add(EntrySelect);

        EntryOnHover.eventID = EventTriggerType.PointerEnter;
        EntryOnHover.callback.AddListener((data) => { Hover((PointerEventData)data); });
        Trigger.triggers.Add(EntryOnHover);

        EntryOnUnhover.eventID = EventTriggerType.PointerExit;
        EntryOnUnhover.callback.AddListener((data) => { UnHover((PointerEventData)data); });
        Trigger.triggers.Add(EntryOnUnhover);
    }

    public virtual void UnSelect()
    {
        IsSelected = false;
        InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
        Debug.Log("UNSELECT SHOULD BE WOKRING");
    }

    //NOTE: I changed the security level of the next three functions from protected to public, so every 
    //reference to these functions will need to match their security as well!!

    public virtual void Select(PointerEventData data)
    {
        IsSelected = true;
        Debug.Log("Select should be working!");
        InteractableMesh.material.SetColor("_EmissionColor", SelectedEmissionColor);
        Debug.Log("Checking out the InteractableMesh really quick " + InteractableMesh.name);
        Debug.Log("SELECT SHOULD BE WORKING!");
    }

    public virtual void Select()
    {
        IsSelected = true;
        Debug.Log("Select should be working!");
        InteractableMesh.material.SetColor("_EmissionColor", SelectedEmissionColor);
        Debug.Log("Checking out the InteractableMesh really quick " + InteractableMesh.name);
        Debug.Log("SELECT SHOULD BE WORKING!");
    }

    public virtual void Hover(PointerEventData data)
    {
        if (!IsSelected)
            InteractableMesh.material.SetColor("_EmissionColor", HighlightedEmissionColor);
        Debug.Log("HOVER SHOULD BE WORKING");
    }

    public virtual void Hover()
    {
        if (!IsSelected)
            InteractableMesh.material.SetColor("_EmissionColor", HighlightedEmissionColor);
        Debug.Log("IsSelected was " + IsSelected + " and the InteractableMesh material's color is " + InteractableMesh.material.color);
        Debug.Log("HOVER SHOULD BE WORKING");
    }

    public virtual void UnHover(PointerEventData data)
    {
        if (!IsSelected)
            InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
        Debug.Log("UNHOVER SHOULD BE WORKING");
    }

    public virtual void UnHover()
    {
        if (!IsSelected)
            InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
        Debug.Log("UNHOVER SHOULD BE WORKING");
    }
}
