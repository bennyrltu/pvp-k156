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
        Opt1.GetComponent<Image>().color = Color.gray;
        Opt2.GetComponent<Image>().color = Color.white;
        Opt3.GetComponent<Image>().color = Color.white;
        firstClicked=true;
        secondClicked=false;
        thirdClicked=false;
    }

    public void ClickSecond()
    {
        Opt1.GetComponent<Image>().color = Color.white;
        Opt2.GetComponent<Image>().color = Color.gray;
        Opt3.GetComponent<Image>().color = Color.white;
        firstClicked=false;
        secondClicked=true;
        thirdClicked=false;
    }

    public void ClickThird()
    {
        Opt1.GetComponent<Image>().color = Color.white;
        Opt2.GetComponent<Image>().color = Color.white;
        Opt3.GetComponent<Image>().color = Color.gray;
        firstClicked=false;
        secondClicked=false;
        thirdClicked=true;
    }

    public void RestoreBools()
    {
        firstClicked=false;
        secondClicked=false;
        thirdClicked=false;
    }
}