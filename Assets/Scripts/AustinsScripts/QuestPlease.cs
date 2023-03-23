using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPlease : MonoBehaviour
{
    [SerializeField]
    private Text canvasText;

    private void Start()
    {
        canvasText.text = OVRPlugin.GetSystemHeadsetType().ToString() + "\n" + OVRPlugin.GetConnectedControllers().ToString();
    }
}
