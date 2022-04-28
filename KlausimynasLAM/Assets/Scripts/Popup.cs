using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public Transform popup;

    private void OnEnable()
    {
        popup.localPosition = new Vector2(0f, -Screen.height);
        popup.LeanMoveLocalY(0f, 0.5f).setEaseOutExpo().delay = 0.5f;
    }

    public void CloseDialog()
    {
        popup.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo().setOnComplete(onComplete);
    }

    void onComplete()
    {
        gameObject.SetActive(false);
    }
}
