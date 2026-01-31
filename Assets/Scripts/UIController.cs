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

    public GeneratorController generatorController;

    void Update()
    {
        int roundedPsi = Mathf.RoundToInt(generatorController.psi);
        int roundedTempCelcius = Mathf.RoundToInt(generatorController.tempurature);
        int roundedTempFarenheit = Mathf.RoundToInt((generatorController.tempurature * 1.8f) + 32);
        int roundedStability = Mathf.RoundToInt(generatorController.stability);

        psiReader.text = Convert.ToString(roundedPsi) + " PSI";
        constantPsiReader.text = Convert.ToString(roundedPsi) + " PSI";
        celciusReader.text = Convert.ToString(roundedTempCelcius) + "<sup>o</sup>C";
        constantCelciusReader.text = Convert.ToString(roundedTempCelcius) + "<sup>o</sup>C";
        farenheitReader.text = Convert.ToString(roundedTempFarenheit) + "<sup>o</sup>F";
        constantFarenheitReader.text = Convert.ToString(roundedTempFarenheit) + "<sup>o</sup>F";
        stabilityPercentage.text = Convert.ToString(roundedStability) + "%";
    }
}
