using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    static readonly string dataFilePath = Application.streamingAssetsPath + "/data.csv";

    List<Question> questionList = new List<Question>();

    [SerializeField]
    GameObject[] options;

    int currentQuestion;

    [SerializeField]
    Text questionText;

    [SerializeField]
    Text questionOutOfQuestionsText;

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
    Sprite originalEllipse;

    [SerializeField]
    Text correctAnswersNumber;

    [SerializeField]
    Text incorrectAnswersNumber;

    [SerializeField]
    Text elapsedTimeText;

    [SerializeField]
    GameObject enlargedImagePanel;

    void Start()
    {
        questionList = ReadQuestionData(dataFilePath);
        GetTotalQuestionsNumber();
        SetQuestionData();
    }

    void Update()
    {
        if (quizPrefab.activeSelf)
        {
            GetComponent<Stopwatch>().enabled = true;
            enabled = false;
        }
    }

    public void Correct()
    {
        Debug.Log("Correct Answer");
        correctAnswers++;
        GetComponent<ProgressBar>().Increase(1f / numberOfQuestions);
        UnclickableButtons();
        Debug.Log(questionList.Count.ToString());
        StartCoroutine(Wait());
    }

    public void Wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(1f / numberOfQuestions);
        UnclickableButtons();
        Debug.Log(questionList.Count.ToString());
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GetComponent<Stopwatch>().enabled = false;
        yield return new WaitForSeconds(2);
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

        if (questionList.Count > 0)
        {
            questionText.text = questionList[currentQuestion].correctAnwserBonus + "<b>" + questionList[currentQuestion].question + "</b>";
            questionOutOfQuestionsText.text = "<b>" + questionIndex + " / " + numberOfQuestions + "</b>";
            answeredQuestionsOutOfText.text = "<b>" + correctAnswers + "/" + numberOfQuestions + "</b>";

            if (questionList[currentQuestion].picName.Length != 0)
            {
                string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].picName;
                var rawData = File.ReadAllBytes(filename);
                tex = new Texture2D(0, 0);
                tex.LoadImage(rawData);
                image.texture = tex;
            }

            SetAnswers();
            questionIndex++;
        } 
        else
        {
            timespan = GetComponent<Stopwatch>().CurrentTime();
            GetComponent<Stopwatch>().enabled = false;
            incorrectAnswers = numberOfQuestions - correctAnswers;
            SetResults(correctAnswersNumber, incorrectAnswersNumber, elapsedTimeText);
            ChangePrefabs(quizPrefab, resultsPrefab);
        }
    }

    void SetAnswers()
    {
        //options[0].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt1;
        //options[1].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt2;
        //options[2].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt3;
        options[0].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt1;
        options[1].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt2;
        options[2].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].opt3;

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;
            options[i].transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = originalEllipse;

            if (questionList[currentQuestion].correctOpt == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
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
            string opt1 = parts[2];
            string opt2 = parts[3];
            string opt3 = parts[4];
            int correctOpt = int.Parse(parts[5]);
            string correctBonus = parts[6];
            string wrongBonus = parts[7];
            string picName = parts[8];

            Question newQuestion = new Question(themeName, questionText, opt1, opt2, opt3, correctOpt, correctBonus, wrongBonus, picName);
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
}
