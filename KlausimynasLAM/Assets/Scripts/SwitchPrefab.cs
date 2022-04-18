using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchPrefab : MonoBehaviour
{
    public GameObject currentPrefab;
    public GameObject nextPrefab;

    public void SwitchToAnotherPrefab()
    {
        currentPrefab.SetActive(false);
        nextPrefab.SetActive(true);
    }
}