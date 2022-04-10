using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stopwatch : MonoBehaviour
{
    float timer;
    float seconds;
    float minutes;

    [SerializeField] Text StopWatchText;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Calc();
    }

    void Calc()
    {
        timer += Time.deltaTime;
        seconds = (int)(timer % 60);
        minutes = (int)((timer / 60) % 60);
        StopWatchText.text = minutes.ToString("00") + ": " + seconds.ToString("00");
    }

    public string CurrentTime()
    {
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void PrintTime()
    {
        Debug.Log(StopWatchText.text);
    }

    public void ResetTime()
    {
        timer = 0.0f;
    }
}
