using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;

    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Image buttonImage;

    public Color startColor;

    [SerializeField]
    private Color correctColor;

    [SerializeField]
    private Color wrongColor;

    private void Start()
    {
        startColor = buttonImage.color;
    }

    public void CheckAnswer()
    {
        if (isCorrect)
        {
            buttonImage.color = correctColor;
            gameController.Correct();
        }
        else
        {
            buttonImage.color = wrongColor;
            gameController.Wrong();
        }
    }
}