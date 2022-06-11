using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confirm : MonoBehaviour
{
    [SerializeField]
    private Button Opt1;

    [SerializeField]
    private Button Opt2;

    [SerializeField]
    private Button Opt3;
    public void ConfirmSelection()
    {
        if (Opt1.GetComponent<AnswerScript>().firstClicked == true)
        {
            Opt1.GetComponent<AnswerScript>().CheckAnswer();
            Opt1.GetComponent<AnswerScript>().RestoreBools();
        }

        if (Opt2.GetComponent<AnswerScript>().secondClicked == true)
        {
            Opt2.GetComponent<AnswerScript>().CheckAnswer();
            Opt2.GetComponent<AnswerScript>().RestoreBools();
        }

        if (Opt3.GetComponent<AnswerScript>().thirdClicked == true)
        {
            Opt3.GetComponent<AnswerScript>().CheckAnswer();
            Opt3.GetComponent<AnswerScript>().RestoreBools();
        }
    }
}
