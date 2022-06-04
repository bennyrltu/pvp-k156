using UnityEngine;
using UnityEngine.SceneManagement;

public class Inactivity : MonoBehaviour
{
    [SerializeField]
    int InactivityTimeInSeconds;

    private Vector3 prevPosition = Vector3.zero;
    void ShowGameHintInvoke()
    {
        CancelInvoke();
        Invoke("ResetAfterInactivity", InactivityTimeInSeconds);
    }
    void ResetAfterInactivity()
    {
        SceneManager.LoadScene("MainScene");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown || Input.mousePosition != prevPosition)
            ShowGameHintInvoke();
        prevPosition = Input.mousePosition;
    }
}
