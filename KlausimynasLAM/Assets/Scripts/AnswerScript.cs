using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public GameController gameController;

    public Color startColor;
    public Color startTextColor;

    private void Start()
    {
        startColor = GetComponent<Image>().color;
        startTextColor = transform.GetChild(0).GetComponent<Text>().color;
    }

    public void checkAnswer()
    {
        if (isCorrect)
        {
            GetComponent<Image>().color = new Color32(92, 176, 95, 255);
            gameController.correct();
        }
        else
        {
            GetComponent<Image>().color = new Color32(241, 85, 85, 255);
            gameController.wrong();
        }
    }
}