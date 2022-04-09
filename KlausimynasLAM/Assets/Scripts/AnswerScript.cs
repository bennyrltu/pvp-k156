using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public ReadCSV readCSV;

    public void Answer()
    {
        if (isCorrect)
        {
            readCSV.correct();
        }
        else
        {
            readCSV.wrong();
        }
    }
}