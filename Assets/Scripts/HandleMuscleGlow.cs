using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HandleMuscleGlow : MonoBehaviour
{

    public PhotonView PV; //Added by Tomas for syncing purposes
    private Renderer MeshRenderer;
    private Material mat;
    Color baseColor;
    Color glowColor;

    public Button Resetbutton;

    // Start is called before the first frame update
    void Awake()
    {

        MeshRenderer = GetComponent<Renderer>();
        mat = MeshRenderer.material;

        baseColor = Color.red;



        Resetbutton.onClick.AddListener(delegate {
            SetHealthy();
        });

        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage()
    {
        Debug.Log("Damaging");
        glowColor = baseColor * Mathf.LinearToGammaSpace(1);
        mat.SetColor("_EmissionColor", glowColor);

        if (PV.IsMine)
        {
            PV.RPC("DamageSync", RpcTarget.Others);
        }

        /*
        foreach (GameObject item in affectedMuscles)
            item.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alphalevel);
        rend = item.GetComponent<Renderer>();
        mat = rend.material;*/
    }

    public void SetHealthy()
    {

        glowColor = baseColor * Mathf.LinearToGammaSpace(0);
        mat.SetColor("_EmissionColor", glowColor);
        if (PV.IsMine)
        {
            PV.RPC("SetHeatlhtySync", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void DamageSync()
    {
        Debug.Log("Damaging");
        glowColor = baseColor * Mathf.LinearToGammaSpace(1);
        mat.SetColor("_EmissionColor", glowColor);
        Debug.Log("Damage has been dealt to " + this.name);
    }

    [PunRPC]
    public void SetHeatlhtySync()
    {
        glowColor = baseColor * Mathf.LinearToGammaSpace(0);
        mat.SetColor("_EmissionColor", glowColor);
        Debug.Log("The " + this.name + " has been recovered and is back to normal!");
    }
}
