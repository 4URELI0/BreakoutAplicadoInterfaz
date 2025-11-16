using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class MenuOpciones : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float volumenActual;

    public void AddVolume(float volumen)
    {
        volumenActual += volumen;

        // Limitar el volumen al rango de -6 a 6
        volumenActual = Mathf.Clamp(volumenActual, -6f, 6f);

        // Si el volumen es menor o igual a -6, poner el volumen a -80 dB (silencio)
        if (volumenActual == -6)
        {
            audioMixer.SetFloat("Volumen", -80f);
        }
        // Si el volumen es mayor que -6, ajustarlo directamente
        else audioMixer.SetFloat("Volumen", volumenActual);

        Debug.Log($"Nuevo valor del Music: {volumenActual}");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}