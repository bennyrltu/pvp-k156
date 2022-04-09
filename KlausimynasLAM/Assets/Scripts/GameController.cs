using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    string timespan = "";

    void Start()
    {
        readDataFromCSV(dataFilePath, ref themeName, ref question, ref answerA, ref answerB, ref answerC, ref answerD, ref correctAnswerIndex);
        setQuestionData();
    }

    void setQuestionData()
    {
        if (question.Count > 0)
        {
            currentQuestion = Random.Range(0, question.Count);

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
            writeResultsToCSV(resultsFilePath, correctAnswers, numberOfQuestions, timespan);
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

            if (correctAnswerIndex[currentQuestion] == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    public void correct()
    {
        Debug.Log("Correct Answer");
        correctAnswers++;
        GetComponent<ProgressBar>().Increase(fillAmount());
        setQuestionData();
    }

    public void wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(fillAmount());
        setQuestionData();
    }

    public float fillAmount()
    {
        return 1f / numberOfQuestions;
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

    void writeResultsToCSV(string path, int correctAnswersNumber, int totalNumberOfQuestionsAnswered, string timespan)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Theme Name" + "," + correctAnswersNumber + "," + totalNumberOfQuestionsAnswered + "," + timespan);
        writer.Close();
    }
}