using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizUIController : MonoBehaviour
{
    [Header("Question UI")]
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Slider muscleOpacitySlider;
    [SerializeField]
    private string correctFeedback, incorrectFeedback;

    private string nerveQuestion;
    private IEnumerator textFeedback;
    public DiagnosingManager diagManager;
    public QuizController quizController;

    private void Awake()
    {
        nerveQuestion = questionText.text;
        diagManager = FindObjectOfType<DiagnosingManager>();
        quizController = FindObjectOfType<QuizController>();
    }

    public void OnCorrect(QuizController quizController, GameObject rightPanel, int chosenNerve)
    {
        rightPanel.SetActive(false);
        DisplayFeedback(correctFeedback, quizController, chosenNerve);
    }

    public void OnIncorrect(QuizController quizController)
    {
        DisplayFeedback(incorrectFeedback, nerveQuestion, quizController);
    }

    public void OnComplete(QuizController quizController, DiagnosingManager diagnosingManager, GameObject rightPanel)
    {
        rightPanel.SetActive(false);
        DisplayFeedback(correctFeedback, quizController, diagnosingManager);
    }

    public void ResetUI()
    {
        muscleOpacitySlider.value = 1.0f;
    }

    public void SetQuestionText()
    {
        questionText.text = nerveQuestion;
    }

    private void DisplayFeedback(string firstString, string secondString, QuizController quizController)
    { 
        if (textFeedback != null)
        {
            StopCoroutine(textFeedback);
        }

        textFeedback = TextFeedback(firstString, secondString, quizController);
        StartCoroutine(textFeedback);
    }

    private void DisplayFeedback(string firstString, QuizController quizController, int chosenNerve)
    {
        if (textFeedback != null)
        {
            StopCoroutine(textFeedback);
        }

        textFeedback = TextFeedback(firstString, quizController, chosenNerve);
        StartCoroutine(textFeedback);
    }

    private void DisplayFeedback(string firstString, QuizController quizController, DiagnosingManager diagnosingManager)
    {
        if (textFeedback != null)
        {
            StopCoroutine(textFeedback);
        }

        textFeedback = TextFeedback(firstString, quizController, diagnosingManager);
        StartCoroutine(textFeedback);
    }

    private IEnumerator TextFeedback(string firstString, string secondString, QuizController quizController)
    {
        questionText.text = firstString;

        yield return new WaitForSeconds(3.0f);

        questionText.text = secondString;
        quizController.LockQuiz = false;
        quizController.ChangeSpheresVisibility(true);
    }

    private IEnumerator TextFeedback(string firstString, QuizController quizController, int chosenNerve)
    {
        questionText.text = firstString;

        yield return new WaitForSeconds(3.0f);

        //int randomNerveInt = Random.Range(0, quizController.nerveQuizList.Count);
        quizController.PrepQuestion(chosenNerve);
    }

    private IEnumerator TextFeedback(string firstString, QuizController quizController, DiagnosingManager diagnosingManager)
    {
        questionText.text = firstString;

        yield return new WaitForSeconds(3.0f);

        ResetUI();
        diagnosingManager.CompleteQuizSection();
    }
}
