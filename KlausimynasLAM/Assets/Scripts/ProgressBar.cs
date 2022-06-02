using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Slider anwseredslider;
    float fillSpeed = 0.5f;
    private float targetProgress = 0.0f;

    private void Start()
    {
        slider.value=0.0f;
        anwseredslider.value=0.0f;
    }

    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            anwseredslider.value += fillSpeed * Time.deltaTime;
        }
    }

    public void Increase(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }

}
