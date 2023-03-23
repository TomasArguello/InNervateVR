using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MuscleOpacityController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] MuscleMeshes;
    public PhotonView PV;

    void Awake()
    {
        if (MuscleMeshes.Length == 0)
        {
            Debug.LogError("Muscle meshes data is null");
            Destroy(this);
        }
        PV = GetComponent<PhotonView>();
    }

    public void ChangeMuscleOpacity(float newValue)
    {
        if (newValue < 0.0f || newValue > 1.0f)
        {
            Debug.LogError("Invalid opacity value for muscles");
            return;
        }

        ExecuteOpacityChange(newValue);
    }

    public void ChangeMuscleOpacity(UnityEngine.UI.Slider SliderBar)
    {
        ExecuteOpacityChange(SliderBar.value);
    }

    private void ExecuteOpacityChange(float newValue)
    {
        PV.RPC("ExecOpacityChangeSync", RpcTarget.Others, newValue);

        foreach (SkinnedMeshRenderer MuscleMesh in MuscleMeshes)
        {
            Color TempColor = MuscleMesh.material.GetColor("_Color");
            MuscleMesh.material.SetColor("_Color", new Color(TempColor.r, TempColor.g, TempColor.b, newValue));
        }
    }

    [PunRPC]
    public void ExecOpacityChangeSync(float newValue)
    {
        foreach (SkinnedMeshRenderer MuscleMesh in MuscleMeshes)
        {
            Color TempColor = MuscleMesh.material.GetColor("_Color");
            MuscleMesh.material.SetColor("_Color", new Color(TempColor.r, TempColor.g, TempColor.b, newValue));
        }
        Debug.Log("ExecOpacityChangeSync worked!!");
    }
}