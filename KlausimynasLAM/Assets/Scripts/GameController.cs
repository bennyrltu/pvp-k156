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
    string sortedResultsFilePath = "Assets/Data/resultsSorted.csv";
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

    void Start()
    {
        readDataFromCSV(dataFilePath, ref themeName, ref question, ref answerA, ref answerB, ref answerC, ref answerD, ref correctAnswerIndex);
        setQuizName();
        setQuestionData();

        //
        if (File.Exists(sortedResultsFilePath))
            File.Delete(sortedResultsFilePath);
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

            //Rokylo
            //writeResultsToCSV(resultsFilePath, correctAnswers, numberOfQuestions, timespan);

            Person personToAdd = new Person("Test", correctAnswers, numberOfQuestions, timespan);

            WritePerson(resultsFilePath, personToAdd);

            //List<Person> person = readPersonData(resultsFilePath);


            //var peopleSorted = person
            //    .OrderByDescending(person => person.correcqs)
            //    .ThenBy(person => person.time)
            //    .ThenBy(person => person.name)
            //    //.Take(5)
            //    .ToList();


            //foreach (Person personToWrite in peopleSorted)
            //{
            //    WritePerson(sortedResultsFilePath, personToWrite);
            //}
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
            options[i].transform.GetChild(0).GetComponent<Text>().color = options[i].GetComponent<AnswerScript>().startTextColor;

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
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
        unclickableButtons();
        //changeColorsOnClick();
        StartCoroutine(wait());
    }

    public void wrong()
    {
        Debug.Log("Wrong Answer");
        GetComponent<ProgressBar>().Increase(1f/numberOfQuestions);
        unclickableButtons();
        //changeColorsOnClick();
        StartCoroutine(wait());
    }

    void changeColorsOnClick()
    {

    }

    IEnumerator wait()
    {
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

    //Rokylo
    //void writeResultsToCSV(string path, int correctAnswersNumber, int totalNumberOfQuestionsAnswered, string timespan)
    //{
    //    StreamWriter writer = new StreamWriter(path, true);
    //    writer.WriteLine("Theme Name" + "," + correctAnswersNumber + "," + totalNumberOfQuestionsAnswered + "," + timespan);
    //    writer.Close();
    //}

    public List<Person> readPersonData(string fileName)
    {
        List<Person> personList = new List<Person>();
        string[] lines = File.ReadAllLines(fileName, Encoding.GetEncoding(1257));
        foreach (var line in lines)
        {
            string[] parts = line.Trim().Split(',');
            string name = parts[0];
            int correctqs = int.Parse(parts[1]);
            int allqs = int.Parse(parts[2]);
            string time = parts[3];

            Person person = new Person(name, correctqs, allqs, time);
            personList.Add(person);
        }
        return personList;
    }

    void WritePerson(string resultsFilePath, Person person)
    {
        StreamWriter writer = new StreamWriter(resultsFilePath, true);
        writer.WriteLine(person.ToString());
        writer.Close();
    }
}