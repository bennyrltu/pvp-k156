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

        int index = peopleSorted.FindIndex(a => a.name.Equals(PlayerPrefs.GetString("username")));

        for (int i = 0; i < peopleSorted.Count; i++)
        {
            if (i < options.Length)
            {
                if (peopleSorted[i].name.Contains("Anonimas"))
                {
                    options[i].transform.GetChild(1).GetComponent<Text>().text = peopleSorted[i].name.Substring(0, 8);
                    options[i].transform.GetChild(2).GetComponent<Text>().text = peopleSorted[i].GetCorrectAndAll();
                    options[i].transform.GetChild(4).GetComponent<Text>().text = peopleSorted[i].time;
                }
                else
                {
                    options[i].transform.GetChild(1).GetComponent<Text>().text = peopleSorted[i].name;
                    options[i].transform.GetChild(2).GetComponent<Text>().text = peopleSorted[i].GetCorrectAndAll();
                    options[i].transform.GetChild(4).GetComponent<Text>().text = peopleSorted[i].time;
                }
                if (peopleSorted[i].name == PlayerPrefs.GetString("username"))
                {
                    options[i].GetComponent<Image>().color = new Color32(0, 250, 0, 150);
                }
                if (index > 5)
                {
                    if (peopleSorted[index].name.Contains("Anonimas"))
                    {
                        options[options.Length - 1].transform.GetChild(1).GetComponent<Text>().text = peopleSorted[index].name.Substring(0, 8);
                    }
                    else
                    {
                        options[options.Length - 1].transform.GetChild(1).GetComponent<Text>().text = peopleSorted[index].name;
                    }
                    options[options.Length - 1].transform.GetChild(2).GetComponent<Text>().text = peopleSorted[index].GetCorrectAndAll();
                    options[options.Length - 1].transform.GetChild(4).GetComponent<Text>().text = peopleSorted[index].time;
                    options[options.Length - 1].transform.GetChild(0).GetComponent<Text>().text = "#" + index.ToString();
                    options[options.Length - 1].GetComponent<Image>().color = new Color32(0, 250, 0, 150);
                }
            }
        }
        PlayerPrefs.DeleteAll();
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
