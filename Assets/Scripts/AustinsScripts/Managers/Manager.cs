using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Manager : MonoBehaviour
{
    public abstract void InitiateSection();
    public abstract void TerminateSection();
}