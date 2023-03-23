using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class InteractiveQuiz : GeneralInteraction
{
    [SerializeField]
    private Renderer sphereIndicator;

    private cutNerve CutNerve;
    private QuizController quizController;
    public DiagnosingManager diagManager;
    public PhotonView PV;

    protected new void Awake()
    {
        base.Awake();

        CutNerve = GetComponent<cutNerve>();

        quizController = FindObjectOfType<QuizController>();
        diagManager = FindObjectOfType<DiagnosingManager>();
        PV = GetComponent<PhotonView>();
        //Debug.Log("So what is IsSelected? It is " + IsSelected);
    }

    public override void Select(PointerEventData data)
    {
        int randomNerveInt = Random.Range(0, quizController.nerveQuizList.Count);
        quizController.CheckForCorrectness(CutNerve,randomNerveInt);
        PV.RPC("SelectSync", RpcTarget.Others,randomNerveInt);
    }

    public override void Select()
    {
        Debug.Log("The selected nerve from " + PhotonNetwork.LocalPlayer.NickName + " is " + CutNerve + " :From Select()");
        int randomNerveInt = Random.Range(0, quizController.nerveQuizList.Count);
        quizController.CheckForCorrectness(CutNerve,randomNerveInt);
        PV.RPC("SelectSync", RpcTarget.Others,randomNerveInt);
    }

    public override void UnSelect() { }

    public override void Hover(PointerEventData data)
    {
        Debug.Log("HOVER SHOULD MOST DEFINITELY WORK: From Interactive Quiz");
        if (!IsSelected)
        {
            Debug.Log("HOVER SHOULD MOST DEFINITELY WORK: From Interactive Quiz");
            sphereIndicator.material.SetColor("_EmissionColor", HighlightedEmissionColor);
            PV.RPC("HoverSync", RpcTarget.Others);
        }
    }

    public override void Hover()
    {
        if (!IsSelected)
        {
            Debug.Log("HOVER SHOULD MOST DEFINITELY WORK: From Interactive Quiz Hover()");
            sphereIndicator.material.SetColor("_EmissionColor", HighlightedEmissionColor);
            PV.RPC("HoverSync", RpcTarget.Others);
        }
    }

    public override void UnHover(PointerEventData data)
    {
        if (!IsSelected)
        {
            sphereIndicator.material.SetColor("_EmissionColor", NormalEmissionColor);
            PV.RPC("UnHoverSync", RpcTarget.Others);
        }
    }

    public override void UnHover()
    {
        if (!IsSelected)
        {
            sphereIndicator.material.SetColor("_EmissionColor", NormalEmissionColor);
            Debug.Log("UNHOVER SHOULD MOST DEFINITELY WORK: From Interactive Quiz UnHover()");
            PV.RPC("UnHoverSync", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void SelectSync(int chosenNerveInt)
    {
        Debug.Log("The nerve selected from " + PhotonNetwork.LocalPlayer.NickName + " is " + CutNerve);
        quizController.CheckForCorrectness(CutNerve,chosenNerveInt);
    }

    [PunRPC]
    public void HoverSync()
    {
        if (!IsSelected)
        {
            sphereIndicator.material.SetColor("_EmissionColor", HighlightedEmissionColor);
        }
    }

    [PunRPC]
    public void UnHoverSync()
    {
        if (!IsSelected)
        {
            sphereIndicator.material.SetColor("_EmissionColor", NormalEmissionColor);
        }
    }
}
