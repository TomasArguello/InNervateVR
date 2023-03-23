using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSync : MonoBehaviour
{
    [Tooltip("If true, synced objects will be enabled when this game object is enabled. If false, synced objects will be disabled when this game object is enabled.")]
    [SerializeField]
    private bool EnableOnSync = false;
    [Tooltip("Game objects that will enabled/disabled when this game object is enabled.")]
    public GameObject[] ObjectsToSync = new GameObject[0];

    private void OnEnable()
    {
        foreach (GameObject go in ObjectsToSync)
        {
            go.SetActive(EnableOnSync);
        }
    }
}
