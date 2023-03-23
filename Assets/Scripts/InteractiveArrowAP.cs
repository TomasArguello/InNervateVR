using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveArrowAP : GeneralInteraction
{
    RotateObject rotateobject;
    public float direction;

    protected new void Awake()
    {
        base.Awake();
        rotateobject = GetComponent<RotateObject>();
    }

    public override void Select(PointerEventData data)
    {
        rotateobject.CallRotate(direction);
    }

    public override void Select()
    {
        //base.Select();
        rotateobject.CallRotate(direction);
    }

    public override void UnSelect() { }
    //protected override void Hover(PointerEventData data) { }
    //protected override void UnHover(PointerEventData data) { }
}
