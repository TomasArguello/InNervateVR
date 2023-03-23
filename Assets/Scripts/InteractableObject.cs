using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class InteractableObject: MonoBehaviour
{
    //public abstract void Awake();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerExit(Collider other);
    public abstract void CallOnClickInteractable();
}
