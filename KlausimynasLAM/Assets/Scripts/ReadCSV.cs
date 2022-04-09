using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ReadCSV : MonoBehaviour
{
    List<string> klausimynas = new List<string>();
    List<string> klausimas = new List<string>();
    List<string> atsA = new List<string>();
    List<string> atsB = new List<string>();
    List<string> atsC = new List<string>();
    List<string> atsD = new List<string>();
    List<int> teisingasAtsakymas = new List<int>();

    string filePath = "Assets/Data/data.csv";
    string savePath = "Assets/Data/rez.csv";

    public GameObject[] options;
    //public int CorrectAnswer;

    int currentQuestion;
    //public int startIndex = 0;
    public Text qText;
    public Text qNumber;
    public Text qCount;

    public Stopwatch stopwatch;

    int index = 1;
    string rez = "";
    int corrects = 0;
    //test
    int QuestionsCount = 0;

    void Start()
    {
        readCSV(filePath, ref klausimynas, ref klausimas, ref atsA, ref atsB, ref atsC, ref atsD, ref teisingasAtsakymas);
        qCount.text = "/ " +QuestionsCount.ToString();
        setData();
    }

    void setData()
    {
        if (klausimas.Count > 0)
        {
            currentQuestion = Random.Range(0, klausimas.Count);

            qText.text = index + ". " + klausimas[currentQuestion];
            qNumber.text = index.ToString()+ ".";
            setAnswers();

            klausimynas.RemoveAt(currentQuestion);
            klausimas.RemoveAt(currentQuestion);
            atsA.RemoveAt(currentQuestion);
            atsB.RemoveAt(currentQuestion);
            atsC.RemoveAt(currentQuestion);
            atsD.RemoveAt(currentQuestion);
            teisingasAtsakymas.RemoveAt(currentQuestion);
            index++;
        }
        else
        {
            Debug.Log("Out of questions");
            this.GetComponent<Stopwatch>().PrintTime();
            rez = this.GetComponent<Stopwatch>().CurrentTime();
            WriteString(savePath, corrects, rez);
            this.GetComponent<Stopwatch>().enabled = false;
        }

    }

    void setAnswers()
    {
        options[0].transform.GetChild(0).GetComponent<Text>().text = atsA[currentQuestion];
        options[1].transform.GetChild(0).GetComponent<Text>().text = atsB[currentQuestion];
        options[2].transform.GetChild(0).GetComponent<Text>().text = atsC[currentQuestion];
        options[3].transform.GetChild(0).GetComponent<Text>().text = atsD[currentQuestion];

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            if (teisingasAtsakymas[currentQuestion] == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    public void correct()
    {
        Debug.Log("Correct Answer");
        setData();
        corrects++;
        this.GetComponent<ProgressBar>().Increase(FillAmount());
    }

    public void wrong()
    {
        Debug.Log("Wrong Answer");
        setData();
        this.GetComponent<ProgressBar>().Increase(FillAmount());
    }

    void readCSV(string filePath, ref List<string> klausimynoPavadinimas, ref List<string> klausimai, ref List<string> atsakymaiA, ref List<string> atsakymaiB, ref List<string> atsakymaiC, ref List<string> atsakymaiD, ref List<int> teisingasAtsakymas)
    {
        string line;

        StreamReader file = new StreamReader(filePath, Encoding.UTF8);
        file.ReadLine();
        while ((line = file.ReadLine()) != null)
        {
            try
            {
                string[] parts = line.Trim().Split(';');

                klausimynoPavadinimas.Add(parts[0]);
                klausimai.Add(parts[1]);
                atsakymaiA.Add(parts[2]);
                atsakymaiB.Add(parts[3]);
                atsakymaiC.Add(parts[4]);
                atsakymaiD.Add(parts[5]);
                teisingasAtsakymas.Add(int.Parse(parts[6]));
                QuestionsCount++;
            }
            catch { }
        }
    }

    void WriteString(string savepath, int corrects, string rez)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(savePath, true);
        writer.WriteLine("Test Name" + "," + corrects + "/" + index + "," + rez);
        writer.Close();
    }

    public float FillAmount()
    {
        return 1f / QuestionsCount;
    }

}
