using UnityEngine;
using UnityEngine.UI;

public class ChangeSlider : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
    }

    public void AddValue(float value)
    {
        slider.value += value;
        Debug.Log($"Nuevo valor del Slider: {slider.value}");
    }

    // Método para inicializar el Slider con el valor actual
    private void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
