using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLeg : MonoBehaviour
{

    private IEnumerator TranslateCoroutine;
    public Toggle ShowHealthy;
    bool moveAway;

    // Use this for initialization
    void Start()
    {

        ShowHealthy.onValueChanged.AddListener(delegate {
            CallMove(-1.2f, 0);
        });

        moveAway = true;

    }




    public void CallMove(float x,float z)
    {
        moveAway = !moveAway;

        if (TranslateCoroutine != null)
        {
            return;
        }


        //pass parameters here
        TranslateCoroutine = TranslateLeg(1.0f, x, z);

        StartCoroutine(TranslateCoroutine);

    }

    IEnumerator TranslateLeg(float timer, float xDir, float zDir)
    {

        float t = 0.0f;
        Vector3 StartPos = transform.position;
       // Vector3 EndPos = new Vector3(StartPos.x + xDir, StartPos.y, StartPos.x + zDir);
        Vector3 EndPos = (moveAway) ? new Vector3(StartPos.x - xDir, StartPos.y, StartPos.z + zDir) : new Vector3(StartPos.x + xDir, StartPos.y, StartPos.z + zDir);

        while (t <= timer)
        {

            Vector3 PositionLerp = Vector3.Lerp(StartPos, EndPos, t / timer);
            transform.position = PositionLerp;

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = EndPos;

        TranslateCoroutine = null;

    }
}
