using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMesh : InteractableObject
{
   
        GameObject MeshRef;
        public int direction;

        public RotateObject[] legRef;

    // Start is called before the first frame update


    void Awake()
        {
            MeshRef = this.gameObject.GetComponent<GameObject>();
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
        //MeshRef.onClick.Invoke();
        foreach ( RotateObject item in legRef )
            item.CallRotate(direction);
    }

    
}
