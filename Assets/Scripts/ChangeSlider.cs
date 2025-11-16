using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ChangeSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private AudioMixer audioMixer;

    void Awake()
    {
        slider = GetComponent<Slider>();
        float currentVolume;
        audioMixer.GetFloat("Volumen", out currentVolume);
        Debug.Log("Volumen Actual: del mixer " + currentVolume);
        slider.value = currentVolume;
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
