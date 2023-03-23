using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cutNerve : MonoBehaviour{

    public legAnimController legAnimRef;
    public SwapTextures nerveRef;
    public HandleMuscleGlow[] musclesRef;
    public HandleMuscleGlow[] affectedMuscles;

    public int NerveState;
    public int NerveIndex;
    //private bool isHealthy;
    private int currentMode;

    private int currentNerveIndex;

    void Start ()
    {
        //isHealthy = true;

        currentMode = 0;
        currentNerveIndex = 0;
    }

    public void CutNerve()
    {
        //isHealthy = false;
        legAnimRef.AnimState = NerveState;
        legAnimRef.healthyBool = false;
        legAnimRef.currentCutNerve = NerveIndex;

        nerveRef.ChangeToCutTexture(NerveState);

        foreach (HandleMuscleGlow item in musclesRef)
        {
            item.SetHealthy();
        }

        foreach (HandleMuscleGlow item in affectedMuscles)
        {
            item.Damage();
            //Debug.Log("Called Damage");
        }
    }

    public void CheckCanCut()
    {
        if (NerveIndex == currentMode)
        {
            //Debug.Log("Cut Nerve called");
            CutNerve();
        }
    }

    public void SetNerveMode(int value)
    {
        if (value != currentMode)
        {
            foreach (HandleMuscleGlow item in musclesRef)
            {
                item.SetHealthy();
            }
        }

        //Debug.Log("selected: " + value);
        legAnimRef.setHealthy();
        currentMode = value;
    }
}
