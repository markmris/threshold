using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI stabilityPercentage;
    public TextMeshProUGUI constantPsiReader;
    public TextMeshProUGUI constantCelciusReader;
    public TextMeshProUGUI constantFarenheitReader;
    public TextMeshPro celciusReader;
    public TextMeshPro farenheitReader;
    public TextMeshPro psiReader;
    public TextMeshPro powerLabel;

    public GeneratorController generatorController;

    public void UpdateText()
    {
        int roundedPsi = Mathf.RoundToInt(generatorController.psi);
        int roundedTempCelcius = Mathf.RoundToInt(generatorController.tempurature);
        int roundedTempFarenheit = Mathf.RoundToInt((generatorController.tempurature * 1.8f) + 32);
        int roundedStability = Mathf.RoundToInt(generatorController.stability);

        psiReader.text = Convert.ToString(roundedPsi) + " PSI";
        constantPsiReader.text = Convert.ToString(roundedPsi) + " PSI";
        celciusReader.text = Convert.ToString(roundedTempCelcius) + "°C";
        constantCelciusReader.text = Convert.ToString(roundedTempCelcius) + "°C";
        farenheitReader.text = Convert.ToString(roundedTempFarenheit) + "°F";
        constantFarenheitReader.text = Convert.ToString(roundedTempFarenheit) + "°F";
        stabilityPercentage.text = Convert.ToString(roundedStability) + "%";
    }

    public void UpdatePowerLabel()
    {
        if (generatorController.powerOn)
        {
            powerLabel.text = "ON";
            powerLabel.color = Color.green;
        }
        else
        {
            powerLabel.text = "OFF";
            powerLabel.color = Color.red;
        }
    }
}
