using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxFill(float fillValue)
    {
        slider.maxValue = fillValue;
        slider.value = fillValue;
    }
    public void SetFill(float fillValue)
    {
        slider.value = fillValue;
    }
}
