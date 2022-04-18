using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public GameObject[] options;

    string resultsFilePath = Application.streamingAssetsPath + "/results.csv";

    void Start()
    {
        SetResults();
    }

    public void SetResults()
    {
        List<Person> personList = ReadPersonData(resultsFilePath);

        var peopleSorted = personList
                .OrderByDescending(person => person.correcqs)
                .ThenBy(person => person.time)
                .ThenBy(person => person.name)
                .ToList();

        for (int i = 0; i < options.Length; i++)
        {
            options[i].transform.GetChild(1).GetComponent<Text>().text = peopleSorted[i].getName();
            options[i].transform.GetChild(2).GetComponent<Text>().text = peopleSorted[i].getCorrectAndAll();
            options[i].transform.GetChild(4).GetComponent<Text>().text = peopleSorted[i].getTime();
        }
    }

    public List<Person> ReadPersonData(string fileName)
    {
        List<Person> personList = new List<Person>();
        string[] lines = File.ReadAllLines(fileName, Encoding.BigEndianUnicode);
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
}
