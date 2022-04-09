using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public GameController gameController;

    public void checkAnswer()
    {
        if (isCorrect)
        {
            gameController.correct();
        }
        else
        {
            gameController.wrong();
        }
    }
}