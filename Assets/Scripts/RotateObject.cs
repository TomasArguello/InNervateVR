using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RotateObject : MonoBehaviour {

    private IEnumerator RotateCoroutine;

    public GameObject interactableLeg;
    public GameObject healthyLeg;
    public GameObject labelLeg;
    public GameObject diagnosingLeg;
    public PhotonView PV; //Added by Tomas

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV != null)
        {
            Debug.Log( this.name + " has a PV!");
        }
    }

    public void CallRotate(float direction)
    {
        if (RotateCoroutine != null)
        {
            return;
        }

        //pass parameters here
        RotateCoroutine = RotateNinety(1.0f, direction);

        StartCoroutine(RotateCoroutine);
        //Added by Tomas
        Debug.Log("CallRotate worked. Hopefully it syncs");
        PV.RPC("CallRotateSync", RpcTarget.Others, direction);
    }

    IEnumerator RotateNinety(float timer, float direction)
    {
        float t = 0.0f;
        Vector3 StartRot = interactableLeg.transform.rotation.eulerAngles;
        Vector3 EndRot = new Vector3(0, StartRot.y + (90.0f * direction), 0);
     

        while (t <= timer)
        {
            Vector3 EulerLerp = Vector3.Lerp(StartRot, EndRot, t / timer);

            interactableLeg.transform.rotation = Quaternion.Euler(EulerLerp);
            healthyLeg.transform.rotation = Quaternion.Euler(EulerLerp);
            labelLeg.transform.rotation = Quaternion.Euler(EulerLerp);
            diagnosingLeg.transform.rotation = Quaternion.Euler(EulerLerp);

            //foreach (InteractionLabel label in ScriptManager.Instance.LabelManager.GetInteractionLabelArray)
            //{
                //label.UpdateLineRendererPositions();
            //}

            t += Time.deltaTime;
            yield return null;
        }

        interactableLeg.transform.rotation = Quaternion.Euler(EndRot);
        healthyLeg.transform.rotation = Quaternion.Euler(EndRot);
        labelLeg.transform.rotation = Quaternion.Euler(EndRot);
        diagnosingLeg.transform.rotation = Quaternion.Euler(EndRot);

        //foreach (InteractionLabel label in ScriptManager.Instance.LabelManager.GetInteractionLabelArray)
        //{
            //label.UpdateLineRendererPositions();
        //}

        RotateCoroutine = null;
    }

    //Tomas added this for syncing across the internet
    [PunRPC]
    public void CallRotateSync(float direction)
    {
        Debug.Log("Received the CallRotateSync RPC");
        //CallRotate(direction);
        if (RotateCoroutine != null)
        {
            return;
        }

        //pass parameters here
        RotateCoroutine = RotateNinety(1.0f, direction);

        StartCoroutine(RotateCoroutine);
        Debug.Log("CallRotate should have worked by now");
    }
}
