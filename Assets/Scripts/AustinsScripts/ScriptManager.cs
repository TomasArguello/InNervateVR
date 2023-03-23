using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    private static ScriptManager _instance;
    public static ScriptManager Instance { get { return _instance; } }

    public InteractionLabelManager LabelManager { private set; get; }
    // Add other managers here

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        OnInitialized();
    }

    private void OnInitialized()
    {
        LabelManager = FindObjectOfType<InteractionLabelManager>();
    }
}
