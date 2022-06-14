using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizController : MonoBehaviour
{
    static readonly string dataFilePath = Application.streamingAssetsPath + "/data.csv";

    List<Question> questionList = new List<Question>();

    [SerializeField]
    GameObject[] options;

    int currentQuestion;

    [SerializeField]
    Text questionWithImageText;

    [SerializeField]
    TextMeshProUGUI questionWithoutImageText;

    [SerializeField]
    Text questionOutOfQuestionsText;

    [SerializeField]
    Text questionOutOfQuestionsTextAnwsered;

    [SerializeField]
    Text answeredQuestionsOutOfText;

    int questionIndex = 1;

    int correctAnswers = 0;

    int incorrectAnswers = 0;

    int numberOfQuestions = 0;

    string timespan;

    Texture2D tex;

    [SerializeField]
    RawImage image;

    [SerializeField]
    GameObject quizPrefab;

    [SerializeField]
    GameObject resultsPrefab;

    [SerializeField]
    GameObject anwseredQuizPrefab;

    [SerializeField]
    Sprite originalEllipse;

    [SerializeField]
    Text correctAnswersNumber;

    [SerializeField]
    Text incorrectAnswersNumber;

    [SerializeField]
    Text elapsedTimeText;

    [SerializeField]
    GameObject enlargedImagePanel;

    [SerializeField]
    public GameObject BonusTextPanel;

    [SerializeField]
    Text BonusPanelCorrectText;

    [SerializeField]
    public Text HideCorrectInfo1;

    [SerializeField]
    public Text HideCorrectInfo2;

    [SerializeField]
    public Text BonusInfoTime;

    [SerializeField]
    public Text BonusInfoCorrect;

    [SerializeField]
    Text BonusPanelWrongText;

    [SerializeField]
    Image Button;

    [SerializeField]
    Text ButtonText;

    [SerializeField]
    GameObject BonusImage;

    [SerializeField]
    private Color correctColor;

    [SerializeField]
    private Color wrongColor;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    Text EndQuiz;

    [SerializeField]
    Text SeeResults;

    [SerializeField]
    GameObject confirmAnswerButton;

    void Start()
    {
        questionList = ReadQuestionData(dataFilePath);
        GetTotalQuestionsNumber();
        SetQuestionData();
    }

    public void Correct()
    {
        ColorOnClick();
        correctAnswers++;
        answeredQuestionsOutOfText.text = "<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";
        GetComponent<ProgressBar>().Increase(1f / (numberOfQuestions));
        UnclickableButtons();
        if (questionList[currentQuestion].bonusPic.Length != 0)
        {
            string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].bonusPic;
            BonusImage.SetActive(true);
            var rawData = File.ReadAllBytes(filename);
            tex = new Texture2D(0, 0);
            tex.LoadImage(rawData);
            BonusImage.GetComponent<RawImage>().texture = tex;
        }
        else
        {
            //BonusImage.GetComponent<RawImage>().texture = null;
            BonusImage.SetActive(false);
        }

        Button.color=correctColor;
        ButtonText.text="PUIKIOS ŽINIOS!";
        HideCorrectInfo1.text = "";
        HideCorrectInfo2.text = "";
        BonusPanelCorrectText.enabled = true;
        BonusPanelWrongText.enabled = false;
        BonusPanelCorrectText.text = questionList[currentQuestion].bonusInfo;
        infoText.text = questionList[currentQuestion].bonusInfo;
        BonusInfoTime.text=GetComponent<Stopwatch>().CurrentTime();
        //StartCoroutine(Wait());
        questionIndex++;
        GetComponent<Stopwatch>().enabled = false;
        BonusInfoCorrect.text = "<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";
        BonusTextPanel.SetActive(true);
    }

    public void Wrong()
    {
        ColorOnClick();
        GetComponent<ProgressBar>().Increase(1f / (numberOfQuestions));
        UnclickableButtons();
        if (questionList[currentQuestion].bonusPic.Length != 0)
        {
            string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].bonusPic;
            BonusImage.SetActive(true);
            var rawData = File.ReadAllBytes(filename);
            tex = new Texture2D(0, 0);
            tex.LoadImage(rawData);
            BonusImage.GetComponent<RawImage>().texture = tex;
        }
        else
        {
            //BonusImage.GetComponent<RawImage>().texture = null;
            BonusImage.SetActive(false);
        }

        Button.color=wrongColor;
        ButtonText.text="DEJA...";
        HideCorrectInfo1.text = "Teisingas atsakymas - ";
        HideCorrectInfo2.text = questionList[currentQuestion].ReturnCorrectOptText();
        BonusPanelWrongText.enabled = true;
        BonusPanelCorrectText.enabled = false;
        BonusPanelWrongText.text = questionList[currentQuestion].bonusInfo;
        infoText.text = "<b>Teisingas atsakymas - <color=#3EBAFF>" + questionList[currentQuestion].ReturnCorrectOptText() + "</b></color>\n\n" + questionList[currentQuestion].bonusInfo;
        BonusInfoTime.text=GetComponent<Stopwatch>().CurrentTime();
        //StartCoroutine(Wait());
        questionIndex++;
        GetComponent<Stopwatch>().enabled = false;
        BonusInfoCorrect.text = "<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";
        BonusTextPanel.SetActive(true);
    }

    IEnumerator Wait()
    {
        questionIndex++;
        GetComponent<Stopwatch>().enabled = false;
        BonusInfoCorrect.text="<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";
        yield return new WaitForSeconds(2);
        BonusTextPanel.SetActive(true);
        yield return new WaitUntil(() => BonusTextPanel.activeSelf == false);
        yield return new WaitForSeconds(1);
        questionList = questionList.Where(x => x.themeName != questionList[currentQuestion].themeName).ToList();
        SetQuestionData();
    }

    void UnclickableButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Button>().interactable = false;
        }
    }

    void GetTotalQuestionsNumber()
    {
        numberOfQuestions = questionList
            .GroupBy(p => p.themeName)
            .Select(g => g.First())
            .Count();
    }

    void SetQuestionData()
    {
        GetComponent<Stopwatch>().enabled = true;
        currentQuestion = Random.Range(0, questionList.Count);


        if (questionList.Count == 1)
        {
            EndQuiz.text = "UŽBAIGTI KLAUSIMYNĄ";
            SeeResults.text = "PERŽIŪRĖTI REZULTATUS";
        }

        if (questionList.Count > 0)
        {
            questionOutOfQuestionsText.text = "<b>" + questionIndex + " / " + numberOfQuestions + "</b>";
            questionOutOfQuestionsTextAnwsered.text = "<b>" + questionIndex + " / " + numberOfQuestions + "</b>";
            answeredQuestionsOutOfText.text = "<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";

            if (questionList[currentQuestion].picName.Length != 0)
            {
                image.enabled = true;
                questionWithImageText.enabled = true;
                questionWithoutImageText.enabled = false;
                questionWithImageText.text = questionList[currentQuestion].question + "<b>" + questionList[currentQuestion].highlightedText + "</b>";
                string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].picName;
                var rawData = File.ReadAllBytes(filename);
                tex = new Texture2D(0, 0);
                tex.LoadImage(rawData);
                image.texture = tex;
            }

            image.enabled = false;
            questionWithImageText.enabled = false;
            questionWithoutImageText.enabled = true;
            questionWithoutImageText.text = questionList[currentQuestion].question + "<b>" + questionList[currentQuestion].highlightedText + "</b>";
            SetAnswers();
        }
        else
        {
            GetComponent<Stopwatch>().enabled = false;
            incorrectAnswers = numberOfQuestions - correctAnswers;
            SetResults(correctAnswersNumber, incorrectAnswersNumber, elapsedTimeText);
            ChangePrefabs(quizPrefab, resultsPrefab);
        }
    }

    void SetAnswers()
    {
        options[0].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt1;
        options[1].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt2;
        options[2].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt3;
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;
            options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = originalEllipse;
            options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;

            if (questionList[currentQuestion].correctOpt == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void ColorOnClick()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().wrongColor;
            options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().wrongColor;

            // pridetas
            options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = options[i].GetComponent<AnswerScript>().incorrectEllipse;

            if (questionList[currentQuestion].correctOpt == i + 1)
            {
                options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().correctColor;
                options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().correctColor;

                // pridetas
                options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = options[i].GetComponent<AnswerScript>().correctEllipse;
            }
        }
    }

    public List<Question> ReadQuestionData(string fileName)
    {
        List<Question> questionList = new List<Question>();
        string[] lines = File.ReadAllLines(fileName, Encoding.BigEndianUnicode).Skip(1).ToArray();
        foreach (var line in lines)
        {
            string[] parts = line.Trim().Split(';');
            string themeName = parts[0];
            string questionText = parts[1];
            string highlitedText = parts[2];
            string opt1 = parts[3];
            string opt2 = parts[4];
            string opt3 = parts[5];
            int correctOpt = int.Parse(parts[6]);
            string bonusInfo = parts[7];
            string picName = parts[8];
            string bonusPic = parts[9];

            Question newQuestion = new Question(themeName, questionText, highlitedText, opt1, opt2, opt3, correctOpt, bonusInfo, picName, bonusPic);
            questionList.Add(newQuestion);
        }
        return questionList;
    }

    void ChangePrefabs(GameObject prefab1, GameObject prefab2)
    {
        prefab1.SetActive(false);
        prefab2.SetActive(true);
    }

    public void SetResults(Text correct, Text incorrect, Text timeElapsed)
    {
        timespan = GetComponent<Stopwatch>().CurrentTime();
        correct.text = correctAnswers.ToString();
        incorrect.text = incorrectAnswers.ToString();
        timeElapsed.text = timespan;
    }

    public void EnlargeImage()
    {
        GetComponent<Stopwatch>().enabled = false;
        enlargedImagePanel.SetActive(true);
        enlargedImagePanel.transform.GetChild(0).GetComponent<RawImage>().texture = tex;
    }

    public void MinimizeImage()
    {
        enlargedImagePanel.SetActive(false);
        GetComponent<Stopwatch>().enabled = true;
    }

    public void NextQuestion()
    {
        ChangePrefabs(anwseredQuizPrefab, quizPrefab);
        questionList = questionList.Where(x => x.themeName != questionList[currentQuestion].themeName).ToList();
        SetQuestionData();
        confirmAnswerButton.SetActive(true);
    }

    public void BackToQuestion()
    {
        ChangePrefabs(anwseredQuizPrefab, quizPrefab);
        confirmAnswerButton.SetActive(false);
    }

    public void BackToQuestionInfo()
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].GetComponent<AnswerScript>().isCorrect == true)
            {
                options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().interactable = true;
                ChangePrefabs(quizPrefab, anwseredQuizPrefab);
            }
            else
            {
                options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().interactable = false;
            }
        }
    }
}