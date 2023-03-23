using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class legAnimController : MonoBehaviour {

    //public cutNerve[] RadialNerveList;
    public Animator legAnim;
    public SwapTextures radialNerveRef;
    public SwapTextures medianUlnarNerveRef;
    public SwapTextures suprascapularNerveRef;
    public SwapTextures musculocutaneousNerveRef;

    public int AnimState;
    public bool healthyBool;
    public int currentCutNerve;


    private IEnumerator FadeCoroutine;
    private float tiltAroundY = 0;
    private float tiltAngleTarget = 90.0f;
    private float smooth = 5.0f;

    public string[] allNerves;

    public HandleMuscleGlow[] allMuscles;

    public PhotonView PV;

    public GameObject centerPanel;

    // Use this for initialization
    void Awake ()
    {
        legAnim = GetComponent<Animator>();

        allNerves = new string[] { "none", "radialNerve", "median_ulnar", "suprascapularNerve", "musculocutaneousNerve" };

        healthyBool = true;
        AnimState = 0;
        currentCutNerve = 0;
        PV = GetComponent<PhotonView>();
        centerPanel = GameObject.Find("CenterPanel");
    }
	
    public void setNerveStates()
    {
        legAnim.SetInteger(allNerves[currentCutNerve], AnimState);
        legAnim.SetBool("healthy", healthyBool);
        //Debug.Log(currentCutNerve);
        //Debug.Log(allNerves[currentCutNerve]);

    }

    //Play Animation state based on Nerve states
    public void playAnimation()
    {
        setNerveStates();
        legAnim.Play("NoMovement");
        PV.RPC("playAnimSync", RpcTarget.Others);
    }

    public void setHealthy()
    {
        AnimState = 0;
        currentCutNerve = 0;
        legAnim.SetInteger(allNerves[1], AnimState);
        legAnim.SetInteger(allNerves[2], AnimState);
        legAnim.SetInteger(allNerves[3], AnimState);
        legAnim.SetInteger(allNerves[4], AnimState);
        healthyBool = true;

        radialNerveRef.setHealthyTexture();
        medianUlnarNerveRef.setHealthyTexture();
        musculocutaneousNerveRef.setHealthyTexture();
        suprascapularNerveRef.setHealthyTexture();

        foreach (HandleMuscleGlow muscle in allMuscles)
        {
            muscle.SetHealthy();
        }

    }

    public void toggleHealthyLeg()
    {
        centerPanel.SetActive(!centerPanel.activeSelf);
        PV.RPC("toggleHealthyLegSync", RpcTarget.Others);
    }

    //Set leg to healthy animation
    public void resetAnimation()
    {
        setHealthy();
        PV.RPC("resetAnimSync", RpcTarget.Others);
    }

    [PunRPC]
    public void playAnimSync()
    {
        //playAnimation();
        setNerveStates();
        legAnim.Play("NoMovement");
        Debug.Log("Animation has been played!");
    }

    [PunRPC]
    public void resetAnimSync()
    {
        setHealthy();
        Debug.Log("Animation has been reset!");
    }

    [PunRPC]
    public void toggleHealthyLegSync()
    {
        centerPanel.SetActive(!centerPanel.activeSelf);
    }

}
