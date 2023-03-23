using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMeshNerve: InteractableObject
{
    GameObject MeshRef;
    cutNerve cutNerveRef;

    public void Awake()
    {
        //MeshRef = this.gameObject.GetComponent<GameObject>();
        cutNerveRef = this.gameObject.GetComponent<cutNerve>();
        //hit.collider.gameObject.GetComponent<InteractableButton>();
    }


    public override void OnTriggerEnter(Collider other)
    {
        //cutNerveRef.CutNerve();
        //throw new System.NotImplementedException();

    }

    public override void OnTriggerExit(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    public override void CallOnClickInteractable()
    {
        cutNerveRef.CheckCanCut();
        //throw new System.NotImplementedException();
    }
}
