using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QuizController : MonoBehaviour
{
    public bool LockQuiz
    {
        set
        {
            this.lockQuiz = value;
        }
        get
        {
            return this.lockQuiz;
        }
    }

    [SerializeField]
    private legAnimController diagnosingLegController;
    [SerializeField]
    private GameObject[] indicatorSpheres;
    [SerializeField]
    private GameObject animationFeedback;

    private bool lockQuiz;
    public List<cutNerve> nerveQuizList;
    private cutNerve currentNerve;
    private QuizUIController quizUIController;
    private DiagnosingManager diagnosingManager;
    private IEnumerator QuestionAnim;
    public GameObject tLimbColliders;

    PhotonView PV;
    int randomNerve;

    private void Awake()
    {
        nerveQuizList = new List<cutNerve>();

        animationFeedback.SetActive(false);

        quizUIController = GetComponent<QuizUIController>();
        diagnosingManager = GetComponent<DiagnosingManager>();

        PV = GetComponent<PhotonView>();
        randomNerve = 0;
        
    }

    public void InitiateQuiz(cutNerve[] nerves,int randomNerveInt)
    {
        lockQuiz = false;

        if (nerveQuizList.Count > 0)
        {
            nerveQuizList.Clear();
        }

        nerveQuizList.AddRange(nerves);
        for(int i = 0; i < nerveQuizList.Count; i++)
        {
            Debug.Log("The nerve " + nerveQuizList[i].name + " is in nerveQuizList");
        }
        PrepQuestion(randomNerveInt);
        //SetQuestion();
    }

    public void TerminateQuiz()
    {
        nerveQuizList.Clear();
        quizUIController.ResetUI();
    }

    IEnumerator delaySetQuestion(float timeInSecs)
    {
        yield return new WaitForSeconds(timeInSecs);
        SetQuestion();
    }

    public void PrepQuestion(int chosenNerve)
    {
        //randomNerve = Random.Range(0, nerveQuizList.Count);
        PV.RPC("SyncNerve", RpcTarget.Others, chosenNerve, PhotonNetwork.LocalPlayer.NickName);
        StartCoroutine("delaySetQuestion", 1f);
    }

    public void SetQuestion()
    {
        //int randomElement;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            //randomElement = Random.Range(0, nerveQuizList.Count);
            //PV.RPC("SyncNerve", RpcTarget.Others, randomElement, PhotonNetwork.LocalPlayer.NickName);

        }
        else
        {
            randomNerve = Random.Range(0, nerveQuizList.Count);
        }

        //currentNerve = nerveQuizList[randomElement];
        currentNerve = nerveQuizList[randomNerve];
        Debug.Log("The randomNerve chosen is " + randomNerve + " from " + PhotonNetwork.LocalPlayer.NickName);
        currentNerve.CutNerve();

        animationFeedback.SetActive(true);

        PlayLegAnimQuestion();

        nerveQuizList.Remove(currentNerve);
    }

    [PunRPC]
    public void SyncNerve(int chosenNerve, string playerName)
    {
        randomNerve = chosenNerve;
        Debug.Log("The randomNerve chosen is " + randomNerve + " from " + playerName);
    }

    public void CheckForCorrectness(cutNerve nerve, int chosenNerve)
    {
        if ((int)diagnosingManager.CurrentQuizState != 1 || lockQuiz) return;

        LockQuiz = true;
        ChangeSpheresVisibility(false);

        if (Equals(nerve, currentNerve) && nerveQuizList.Count > 0)
        {
            quizUIController.OnCorrect(this, diagnosingManager.CurrentUISection.RightPanel, chosenNerve);
        }
        else if (Equals(nerve, currentNerve) && nerveQuizList.Count == 0)
        {
            quizUIController.OnComplete(this, diagnosingManager, diagnosingManager.CurrentUISection.RightPanel);
        }
        else
        {
            quizUIController.OnIncorrect(this);
        }
    }

    public void PlayLegAnimQuestion()
    {
        if (QuestionAnim != null)
        {
            StopCoroutine(QuestionAnim);
        }

        QuestionAnim = LegAnimation();
        StartCoroutine(QuestionAnim);
    }

    public void ChangeSpheresVisibility(bool isVisible)
    {
        foreach(GameObject sphere in indicatorSpheres)
        {
            sphere.SetActive(isVisible);
        }
    }

    private IEnumerator LegAnimation()
    {
        ChangeSpheresVisibility(false);
        tLimbColliders.SetActive(false);

        diagnosingLegController.playAnimation();

        yield return new WaitForEndOfFrame();

        diagnosingManager.CurrentUISection.LeftPanel.SetActive(false);
        diagnosingManager.CurrentUISection.RightPanel.SetActive(false);

        yield return new WaitUntil(() => diagnosingLegController.legAnim.GetCurrentAnimatorStateInfo(0).IsName("DefaultState"));

        quizUIController.SetQuestionText();

        LockQuiz = false;

        ChangeSpheresVisibility(true);
        tLimbColliders.SetActive(true);

        diagnosingManager.CurrentUISection.LeftPanel.SetActive(true);
        diagnosingManager.CurrentUISection.RightPanel.SetActive(true);

        if (animationFeedback.activeSelf)
        {
            animationFeedback.SetActive(false);
        }
    }
}
