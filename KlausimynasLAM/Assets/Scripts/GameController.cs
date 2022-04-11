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
    string dataFilePath = "Assets/Data/data.csv";

    /* rezultatø saugojimo failas */
    // gal geriau storint kiekvieno temos rezultatus atskirai, nes vis tiek leaderboardai bus atskiri
    string resultsFilePath = "Assets/Data/results.csv";
    List<string> themeName = new List<string>();
    List<string> question = new List<string>();
    List<string> answerA = new List<string>();
    List<string> answerB = new List<string>();
    List<string> answerC = new List<string>();
    List<string> answerD = new List<string>();
    List<int> correctAnswerIndex = new List<int>();
    List<string> asdf = new List<string>();


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

    //GameObject resultsPrefab = (GameObject)Resources.Load("QuizResults", typeof(GameObject));
    //GameObject quizPrefab = (GameObject)Resources.Load("Quiz.prefab", typeof(GameObject));.
    public GameObject quizPrefab;
    public GameObject usernamePrefab;

    void Start()
    {
        readDataFromCSV(dataFilePath, ref themeName, ref question, ref answerA, ref answerB, ref answerC, ref answerD, ref correctAnswerIndex);
        setQuizName();
        setQuestionData();
        //var resultsPrefab = (GameObject)Resources.Load("QuizResults", typeof(GameObject));
        //var quizPrefab = (GameObject)Resources.Load("Quiz.prefab", typeof(GameObject));
        //GameObject resultsPrefab = (GameObject)Resources.Load("QuizResults", typeof(GameObject));
        //GameObject quizPrefab = (GameObject)Resources.Load("Quiz.prefab", typeof(GameObject));
    }

    void setQuizName()
    {
        asdf = themeName.Distinct().ToList();
        for (int i = 0; i < themes.Length; i++)
        {
            themes[i].transform.GetChild(0).GetComponent<Text>().text = asdf[i];
        }
    }

    void setQuestionData()
    {
        GetComponent<Stopwatch>().enabled = true;
        currentQuestion = Random.Range(0, question.Count);

        if (question.Count > 0)
        {

            questionText.text = question[currentQuestion];
            questionNumber.text = "Klausimas " + questionIndex;
            questionCount.text = "/" + numberOfQuestions;

            setAnswers();
            themeName.RemoveAt(currentQuestion);
            question.RemoveAt(currentQuestion);
            answerA.RemoveAt(currentQuestion);
            answerB.RemoveAt(currentQuestion);
            answerC.RemoveAt(currentQuestion);
            answerD.RemoveAt(currentQuestion);
            correctAnswerIndex.RemoveAt(currentQuestion);
            questionIndex++;
        }
        else
        {
            Debug.Log("Out of questions");
            GetComponent<Stopwatch>().PrintTime();
            timespan = GetComponent<Stopwatch>().CurrentTime();
            GetComponent<Stopwatch>().enabled = false;
            //quizPrefab.SetActive(false);
            //resultsPrefab.SetActive(true);
            //Person personToAdd = new Person("Test", correctAnswers, numberOfQuestions, timespan);

            //WritePerson(resultsFilePath);
            ChangePrefabs(quizPrefab, usernamePrefab);
        }
    }

    void setAnswers()
    {
        options[0].transform.GetChild(0).GetComponent<Text>().text = answerA[currentQuestion];
        options[1].transform.GetChild(0).GetComponent<Text>().text = answerB[currentQuestion];
        options[2].transform.GetChild(0).GetComponent<Text>().text = answerC[currentQuestion];
        options[3].transform.GetChild(0).GetComponent<Text>().text = answerD[currentQuestion];

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].GetComponent<Button>().interactable = true;
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;
            //options[i].transform.GetChild(0).GetComponent<Text>().color = options[i].GetComponent<AnswerScript>().startTextColor;
            //options[i].GetComponent<Image>().color = new Color32(92, 176, 95, 255);

            if (correctAnswerIndex[currentQuestion] == i + 1)
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
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
        unclickableButtons();
        StartCoroutine(wait());
    }

    public void wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
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

    void readDataFromCSV(string path, ref List<string> themes, ref List<string> questions, ref List<string> answersA, ref List<string> answersB, ref List<string> answersC, ref List<string> answersD, ref List<int> correctAnswer)
    {
        string line;
        StreamReader reader = new StreamReader(path, Encoding.GetEncoding(1257));
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            try
            {
                string[] parts = line.Trim().Split(';');

                themes.Add(parts[0]);
                questions.Add(parts[1]);
                answersA.Add(parts[2]);
                answersB.Add(parts[3]);
                answersC.Add(parts[4]);
                answersD.Add(parts[5]);
                correctAnswer.Add(int.Parse(parts[6]));
                numberOfQuestions++;
            }
            catch { }
        }
        reader.Close();
    }

    public void WritePerson(string resultsFilePath, string enteredName)
    {
        Person personToAdd = new Person(enteredName, correctAnswers, numberOfQuestions, timespan);
        StreamWriter writer = new StreamWriter(resultsFilePath, true);
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
}