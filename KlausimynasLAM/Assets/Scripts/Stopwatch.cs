using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Stopwatch : MonoBehaviour
{
    float timer;
    float seconds;
    float minutes;

    [SerializeField] string OverTimeDuration;

    [SerializeField] Text StopWatchText;

    [SerializeField]
    GameObject quizPrefab;

    [SerializeField]
    GameObject resultPrefab;

    [SerializeField]
    Text correctAnswersNumber;

    [SerializeField]
    Text incorrectAnswersNumber;

    [SerializeField]
    Text elapsedTimeText;

    // Start is called before the first frame update
    void Start()
    {
        timer=0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (quizPrefab.activeSelf)
        {
            Calc();
        }
    }

    void Calc()
    {
            timer += Time.deltaTime;
            seconds = (int)(timer % 60);
            minutes = (int)((timer / 60) % 60);
            StopWatchText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (StopWatchText.text == OverTimeDuration)
        {
            enabled=false;
            GetComponent<QuizController>().SetResults(correctAnswersNumber, incorrectAnswersNumber, elapsedTimeText);
            ChangePrefabs(quizPrefab, resultPrefab);

        }
    }

    public string CurrentTime()
    {
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void ChangePrefabs(GameObject prefab1, GameObject prefab2)
    {
        prefab1.SetActive(false);
        prefab2.SetActive(true);
    }


}
