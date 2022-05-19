using System;
using UnityEngine;
using UnityEngine.UI;

public class SubmitUsername : MonoBehaviour
{
    public InputField TextBox;
    public Text correctString;
    public Text timespan;

    public GameObject current;
    public GameObject next;

    public Person Person
    {
        get => default;
        set
        {
        }
    }

    public Leaderboard Leaderboard
    {
        get => default;
        set
        {
        }
    }

    public void ClickSaveButton()
    {

        string resultsFilePath = Application.streamingAssetsPath + "/results.csv";

        var now = DateTime.Now;
        string date = now.ToString("yy/MM/dd/H/m/s");
        if (TextBox.text.Length == 0)
        {
            PlayerPrefs.SetString("username", "Anonimas"+date);
        }
        else
        {
            PlayerPrefs.SetString("username", TextBox.text);
        }      
        GetComponent<GameController>().WritePerson(resultsFilePath, PlayerPrefs.GetString("username"));
        GetComponent<GameController>().SetResults(correctString, timespan);
        GetComponent<Leaderboard>().SetResults();
        SwitchPrefabs(current, next);
    }

    public void SwitchPrefabs(GameObject current, GameObject next)
    {
        current.SetActive(false);
        next.SetActive(true);
    }
}
