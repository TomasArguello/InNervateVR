using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DiagnosingManager : Manager
{
    public enum QuizState { Selection, Testing, Complete}
    public QuizState CurrentQuizState { get { return currentUISection.QuizStates; } }
    public QuizUISection CurrentUISection { get { return currentUISection; } }

    [System.Serializable]
    public struct QuizUISection
    {
        public QuizState QuizStates;
        public GameObject LeftPanel;
        public GameObject RightPanel;
    }

    [System.Serializable]
    public struct DiagnosingNerve
    {
        public NerveCategories NerveCategory;
        public SkinnedMeshRenderer NerveMesh;
        public cutNerve[] NervePoints;
    }

    [Header("Canine Leg")]
    [SerializeField]
    private GameObject DiagnosingLeg;
    [Header("Nerve Settings")]
    [SerializeField]
    private DiagnosingNerve[] DiagnosingNerveArray;
    [SerializeField]
    private legAnimController DiagnosingLegAnimController;
    [Header("Quiz Settings")]
    [SerializeField]
    private QuizController quizController;
    [SerializeField]
    private QuizUISection[] quizSections;

    public PhotonView PV;

    private Dictionary<NerveCategories, DiagnosingNerve> DiagnosingNerveDictionary;
    private Dictionary<QuizState, QuizUISection> QuizNerveDictionary;
    private DiagnosingNerve currentNerve;
    private QuizUISection currentUISection;

    private void Awake()
    {
        DiagnosingNerveDictionary = new Dictionary<NerveCategories, DiagnosingNerve>();
        QuizNerveDictionary = new Dictionary<QuizState, QuizUISection>();

        foreach (DiagnosingNerve nerve in DiagnosingNerveArray)
        {
            DiagnosingNerveDictionary.Add(nerve.NerveCategory, nerve);
        }

        foreach (QuizUISection section in quizSections)
        {
            QuizNerveDictionary.Add(section.QuizStates, section);
        }

        PV = GetComponent<PhotonView>();
    }

    public override void InitiateSection()
    {
        ChangeQuizSection(0);

        DiagnosingLeg.SetActive(true);

        DiagnosingLegAnimController.setHealthy();
    }

    public override void TerminateSection()
    {
        Debug.Log("TerminateSection is being call from DiagnosingManager??");
        Debug.Log(currentNerve + " is the currentNerve");
        if (!Equals(currentNerve, default(DiagnosingNerve)))
        { 
            ChangeNerveVisibility(currentNerve, true);
        }

        if (!Equals(currentUISection, default(QuizUISection)))
        {
            currentUISection.LeftPanel.SetActive(false);
            currentUISection.RightPanel.SetActive(false);
        }

        quizController.TerminateQuiz();

        DiagnosingLeg.SetActive(false);
    }

    public void ResetSection()
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, true);
        }

        ChangeQuizSection(0);
        PV.RPC("ResetSectSync", RpcTarget.Others);
    }

    public void InitiateQuizSection()
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, false);
        }

        int randomNerveInt = Random.Range(0, currentNerve.NervePoints.Length);
        quizController.InitiateQuiz(currentNerve.NervePoints,randomNerveInt);
        ChangeNerveVisibility(currentNerve, true);
        ChangeQuizSection(1);
        PV.RPC("InitQuizSect", RpcTarget.Others);
    }

    public void InitiateQuizSection(int newNerveElement)
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, false);
        }

        ChangeNerveCategory(newNerveElement);
        int randomNerveInt = Random.Range(0, currentNerve.NervePoints.Length);
        quizController.InitiateQuiz(currentNerve.NervePoints,randomNerveInt);
        Debug.Log("The currentNerve from " + PhotonNetwork.LocalPlayer.NickName + " is " + currentNerve.NerveCategory);
        ChangeQuizSection(1);
        PV.RPC("InitQuizSect", RpcTarget.Others, newNerveElement,randomNerveInt);
    }

    [PunRPC]
    public void InitQuizSect()
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, false);
        }

        quizController.InitiateQuiz(currentNerve.NervePoints,0);
        ChangeNerveVisibility(currentNerve, true);
        ChangeQuizSection(1);
    }

    [PunRPC]
    public void InitQuizSect(int newNerveElem,int randomNerveInt)
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, false);
        }

        ChangeNerveCategory(newNerveElem);
        quizController.InitiateQuiz(currentNerve.NervePoints,randomNerveInt);
        Debug.Log("The currentNerve from " + PhotonNetwork.LocalPlayer.NickName + " is " + currentNerve.NerveCategory);
        ChangeQuizSection(1);
    }

    [PunRPC]
    public void ResetSectSync()
    {
        foreach (KeyValuePair<NerveCategories, DiagnosingNerve> valuePair in DiagnosingNerveDictionary)
        {
            ChangeNerveVisibility(valuePair.Value, true);
        }

        ChangeQuizSection(0);
    }

    public void CompleteQuizSection()
    {
        DiagnosingLegAnimController.setHealthy();
        ChangeQuizSection(2);
    }

    private void ChangeNerveCategory(int newNerveElement)
    {
        DiagnosingNerve newNerve;

        if (DiagnosingNerveDictionary.TryGetValue((NerveCategories)newNerveElement, out newNerve))
        {
            if (!Equals(currentNerve, default(DiagnosingNerve)))
            {
                ChangeNerveVisibility(currentNerve, false);
            }

            ChangeNerveVisibility(newNerve, true);
            currentNerve = newNerve;
            Debug.Log("The currentNerve in ChangeNerveCategory is " + currentNerve.NerveCategory);
        }
    }

    private void ChangeQuizSection(int newQuizState)
    {
        QuizUISection newQuizUISection;

        if (QuizNerveDictionary.TryGetValue((QuizState)newQuizState, out newQuizUISection))
        {
            if (!Equals(currentUISection, default(QuizUISection)))
            {
                currentUISection.LeftPanel.SetActive(false);
                currentUISection.RightPanel.SetActive(false);
            }

            newQuizUISection.LeftPanel.SetActive(true);
            newQuizUISection.RightPanel.SetActive(true);

            currentUISection = newQuizUISection;
        }
    }

    private void ChangeNerveVisibility(DiagnosingNerve newNerve, bool isVisible)
    {
        newNerve.NerveMesh.enabled = isVisible;

        foreach (cutNerve cutnerve in newNerve.NervePoints)
        {
            cutnerve.gameObject.SetActive(isVisible);
        }
    }
}
