using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /* klausimynø duomenø failas */
    string dataFilePath = Application.streamingAssetsPath + "/data.csv";

    // klausimų sąrašas
    List<Question> questionList = new List<Question>();

    /* klausimynu temu pavadinimu uzpildymui */
    public GameObject[] themes;

    /* atsakymø variantø uþpildymui */
    public GameObject[] options;

    /* dabartinio klausimo indeksas */
    int currentQuestion;

    /* klausimo, klausimo numerio ir klausimø kiekio uþpildymui */
    public Text questionText;
    public Text questionNumber;
    public Text questionCount;

    /* klausimo numerio indeksas atvaizdavimui */
    int questionIndex = 1;

    /* teisingø atsakymø kiekis */
    int correctAnswers = 0;

    /* kiek ið viso klausimø yra atrinkta á klausimynà */
    int numberOfQuestions = 0;

    /* sugaiðtas laikas sprendþiant klausimynà */
    // reikës keist, pataisius Stopwatch.cs, nes saugosim tai kaip int, sekundëmis
    string timespan;

    public GameObject quizPrefab;
    public GameObject usernamePrefab;
    Texture2D tex;
    public GameObject rawImage;
    public GameObject enlargedImagePanel;

    void Start()
    {
        questionList = ReadQuestionData(dataFilePath);
        SetQuizName();
        SetQuestionData();
        GetComponent<Stopwatch>().enabled = false;
    }

     void Update()
    { 
        if(quizPrefab.activeSelf)
        {
            GetComponent<Stopwatch>().enabled = true;
            enabled = false;
        }
    }

    void SetQuizName()
    {
        List<Question> distinctPeople = questionList
            .GroupBy(p => p.themeName)
            .Select(g => g.First())
            .ToList();

        numberOfQuestions = distinctPeople.Count;

        for (int i = 0; i < distinctPeople.Count; i++)
        {
            themes[i].transform.GetChild(0).GetComponent<Text>().text = distinctPeople[i].getThemeName();
        }
    }

    void SetQuestionData()
    {
        GetComponent<Stopwatch>().enabled = true;
        rawImage.SetActive(false);
        currentQuestion = Random.Range(0, questionList.Count);

        if (questionList.Count > 0)
        {

            questionText.text = questionList[currentQuestion].getQuestion();
            questionNumber.text = "Klausimas " + questionIndex;
            questionCount.text = "/" + numberOfQuestions;

            if (questionList[currentQuestion].getPicName().Length != 0)
            {
                questionText.transform.position = new Vector2(960, 525);
                string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].getPicName();
                rawImage.SetActive(true);
                Debug.Log(filename);
                var rawData = File.ReadAllBytes(filename);
                tex = new Texture2D(0, 0);
                tex.LoadImage(rawData);
                rawImage.GetComponent<RawImage>().texture = tex;
            }

            SetAnswers();
            questionList = questionList.Where(x => x.themeName != questionList[currentQuestion].getThemeName()).ToList();
            questionIndex++;
        }
        else
        {
            Debug.Log("Out of questions");
            GetComponent<Stopwatch>().PrintTime();
            timespan = GetComponent<Stopwatch>().CurrentTime();
            GetComponent<Stopwatch>().enabled = false;
            ChangePrefabs(quizPrefab, usernamePrefab);
        }
    }

    void SetAnswers()
    {
        options[0].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].getOpt1();
        options[1].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].getOpt2();
        options[2].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].getOpt3();
        options[3].transform.GetChild(0).GetComponent<Text>().text = questionList[currentQuestion].getOpt4();

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;
            //options[i].transform.GetChild(0).GetComponent<Text>().color = options[i].GetComponent<AnswerScript>().startTextColor;
            //options[i].GetComponent<Image>().color = new Color32(92, 176, 95, 255);

            if (questionList[currentQuestion].getCorrectOpt() == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
                //options[i].GetComponent<Image>().color = new Color32(92, 176, 95, 255);
            }
        }
    }

    public void Correct()
    {
        Debug.Log("Correct Answer");
        correctAnswers++;
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
        UnclickableButtons();
        StartCoroutine(Wait());
    }

    public void Wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
        UnclickableButtons();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        GetComponent<Stopwatch>().enabled = false;
        yield return new WaitForSeconds(2);
        SetQuestionData();
    }

    void UnclickableButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Button>().interactable = false;
        }
    }

    public void WritePerson(string resultsPath, string enteredName)
    {
        Person personToAdd = new Person(enteredName, correctAnswers, numberOfQuestions, timespan);
        StreamWriter writer = new StreamWriter(resultsPath, true, Encoding.BigEndianUnicode);
        writer.WriteLine(personToAdd.ToString());
        writer.Close();
    }

    public void SetResults(Text a, Text b)
    {
        a.text = correctAnswers + "/" + numberOfQuestions;
        b.text = timespan + " min.";
    }

    public void ChangePrefabs(GameObject prefab1, GameObject prefab2)
    {
        prefab1.SetActive(false);
        prefab2.SetActive(true);
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
            string opt4 = parts[5];
            int correctOpt = int.Parse(parts[6]);
            string picName = parts[7];

            Question newQuestion = new Question(themeName, questionText, opt1, opt2, opt3, opt4, correctOpt, picName);
            questionList.Add(newQuestion);
        }
        return questionList;
    }
    public void enlargeImage()
    {
        GetComponent<Stopwatch>().enabled = false;
        enlargedImagePanel.SetActive(true);
        enlargedImagePanel.transform.GetChild(0).GetComponent<RawImage>().texture = tex;
    }

    public void minimizeImage()
    {
        enlargedImagePanel.SetActive(false);
        GetComponent<Stopwatch>().enabled = true;
    }
}