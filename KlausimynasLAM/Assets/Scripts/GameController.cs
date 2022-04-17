using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;

public class GameController : MonoBehaviour
{
    /* klausimynø duomenø failas */
    //string dataFilePath = "Assets/Data/data.csv";
    string dataFilePath = Application.streamingAssetsPath + "/data.csv";

    List<Question> questionList = new List<Question>();

    int totalQuestions = 0;


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
    Texture2D myTexture;
    public GameObject rawImage;

    void Start()
    {
        questionList = readQuestionData(dataFilePath);
        setQuizName();
        setQuestionData();   
    }

    void setQuizName()
    {
        List<Question> distinctPeople = questionList
            .GroupBy(p => p.themeName)
            .Select(g => g.First())
            .ToList();

        totalQuestions = distinctPeople.Count;
        for (int i = 0; i < distinctPeople.Count; i++)
        {
            themes[i].transform.GetChild(0).GetComponent<Text>().text = distinctPeople[i].getThemeName();
        }
    }

    void setQuestionData()
    {
        GetComponent<Stopwatch>().enabled = true;
        rawImage.SetActive(false);
        currentQuestion = Random.Range(0, questionList.Count);
        if (questionList.Count > 0)
        {

            questionText.text = questionList[currentQuestion].getQuestion();
            questionNumber.text = "Klausimas " + questionIndex;
            questionCount.text = "/" + totalQuestions;
            questionCount.text = "3";
            if (questionList[currentQuestion].getPicName().Length != 0)
            {
                string filename = Application.streamingAssetsPath + "/Images/" + questionList[currentQuestion].getPicName();
                rawImage.SetActive(true);
                Debug.Log(filename);
                var rawData = File.ReadAllBytes(filename);
                Texture2D tex = new Texture2D(0, 0);
                tex.LoadImage(rawData);
               rawImage.GetComponent<RawImage>().texture = tex;
            }

            setAnswers();
            questionList = questionList.Where(x => x.themeName != questionList[currentQuestion].getThemeName()).ToList();
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

    void setAnswers()
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
            //Debug.Log(correctAnswerIndex[currentQuestion]);

            if (questionList[currentQuestion].getCorrectOpt() == (i + 1).ToString())
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
                //options[i].GetComponent<Image>().color = new Color32(92, 176, 95, 255);
            }
        }
    }

    public void correct()
    {
        Debug.Log("Correct Answer");
        correctAnswers++;
        GetComponent<ProgressBar>().Increase(1f/totalQuestions);
        unclickableButtons();
        StartCoroutine(wait());
    }

    public void wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(1f/totalQuestions);
        unclickableButtons();
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        GetComponent<Stopwatch>().enabled = false;
        yield return new WaitForSeconds(2);
        setQuestionData();
    }

    void unclickableButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Button>().interactable = false;
        }
    }

    //void readDataFromCSV(string path, ref List<string> themes, ref List<string> questions, ref List<string> answersA, ref List<string> answersB, ref List<string> answersC, ref List<string> answersD, ref List<int> correctAnswer, ref List<string> imagepaths)
    //{
    //    string line;
    //    StreamReader reader = new StreamReader(path, Encoding.BigEndianUnicode);
    //    reader.ReadLine();

    //    while ((line = reader.ReadLine()) != null)
    //    {
    //        try
    //        {
    //            string[] parts = line.Trim().Split(';');

    //            themes.Add(parts[0]);
    //            questions.Add(parts[1]);
    //            answersA.Add(parts[2]);
    //            answersB.Add(parts[3]);
    //            answersC.Add(parts[4]);
    //            answersD.Add(parts[5]);
    //            correctAnswer.Add(int.Parse(parts[6]));
    //            imagepaths.Add(parts[7]);
    //            numberOfQuestions++;
    //        }
    //        catch { }
    //    }
    //    reader.Close();
    //}

    public void WritePerson(string resultsPath, string enteredName)
    {
        Person personToAdd = new Person(enteredName, correctAnswers, numberOfQuestions, timespan);
        StreamWriter writer = new StreamWriter(resultsPath, true, Encoding.BigEndianUnicode);
        writer.WriteLine(personToAdd.ToString());
        writer.Close();
    }

    public void setResults(Text a, Text b)
    {
        a.text = correctAnswers + "/" + numberOfQuestions;
        b.text = timespan + " min.";
    }

    public void ChangePrefabs(GameObject prefab1, GameObject prefab2)
    {
        prefab1.SetActive(false);
        prefab2.SetActive(true);
    }

    public List<Question> readQuestionData(string fileName)
    {
        List<Question> questionList = new List<Question>();
        string[] lines = File.ReadAllLines(fileName, Encoding.BigEndianUnicode).Skip(1).ToArray();
        foreach (var line in lines)
        {
            string[] parts = line.Trim().Split(';');
            string themeName = parts[0];
            string question = parts[1];
            string opt1 = parts[2];
            string opt2 = parts[3];
            string opt3 = parts[4];
            string opt4 = parts[5];
            string correct = parts[6];
            string picName = parts[7];

            Question questionas = new Question(themeName, question, opt1, opt2, opt3, opt4, correct, picName);
            questionList.Add(questionas);
        }
        return questionList;
    }
}