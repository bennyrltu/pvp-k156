using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect;

    [SerializeField]
    private QuizController gameController;

    [SerializeField]
    private Image buttonImage;

    public Color startColor;

    [SerializeField]
    public Color correctColor;

    [SerializeField]
    public Color wrongColor;

    public Image originalEllipse;

    [SerializeField]
    private Sprite correctEllipse;

    [SerializeField]
    private Sprite incorrectEllipse;

    [SerializeField]
    private Button Opt1;

    [SerializeField]
    private Button Opt2;

    [SerializeField]
    private Button Opt3;

    public bool firstClicked;
    public bool secondClicked;
    public bool thirdClicked;

    public void CheckAnswer()
    {
        if (isCorrect)
        {
            buttonImage.color = correctColor;
            originalEllipse.sprite = correctEllipse;
            gameController.Correct();
        }
        else
        {
            buttonImage.color = wrongColor;
            originalEllipse.sprite = incorrectEllipse;
            gameController.Wrong();
        }
    }

    public void ClickFirst()
    {
        Opt1.GetComponent<Image>().color = new Color32(83, 68, 197, 255);
        //Opt1.GetComponent<Image>().color = Color.gray;
        Opt1.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = new Color32(16, 16, 56, 255);
        Opt2.GetComponent<Image>().color = Color.white;
        Opt3.GetComponent<Image>().color = Color.white;
        firstClicked=true;
        Opt2.GetComponent<AnswerScript>().secondClicked=false;
        Opt3.GetComponent<AnswerScript>().thirdClicked=false;
        Opt2.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
        Opt3.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
    }

    public void ClickSecond()
    {
        Opt1.GetComponent<Image>().color = Color.white;
        //Opt2.GetComponent<Image>().color = Color.gray;
        Opt2.GetComponent<Image>().color = new Color32(83, 68, 197, 255);
        Opt2.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = new Color32(16, 16, 56, 255);
        Opt3.GetComponent<Image>().color = Color.white;
        Opt1.GetComponent<AnswerScript>().firstClicked=false;
        secondClicked=true;
        Opt3.GetComponent<AnswerScript>().thirdClicked=false;
        Opt1.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
        Opt3.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
    }

    public void ClickThird()
    {
        Opt1.GetComponent<Image>().color = Color.white;
        Opt2.GetComponent<Image>().color = Color.white;
        //Opt3.GetComponent<Image>().color = Color.gray;
        Opt3.GetComponent<Image>().color = new Color32(83, 68, 197, 255);
        Opt3.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = new Color32(16, 16, 56, 255);
        Opt1.GetComponent<AnswerScript>().firstClicked=false;
        Opt2.GetComponent<AnswerScript>().secondClicked=false;
        thirdClicked=true;
        Opt1.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
        Opt2.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().color = Color.white;
    }

    public void RestoreBools()
    {
        firstClicked=false;
        secondClicked=false;
        thirdClicked=false;
    }
}