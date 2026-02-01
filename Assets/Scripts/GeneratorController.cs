using System.Collections;
using TMPro;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    [Header ("---- Generator GameObjects ----")] 
    public GameObject shutdownButtonClickable;
    public GameObject coolDownButtonClickable;
    public GameObject ventButtonClickable;
    public GameObject powerSwitchVFX;
    public GameObject powerSwitchClickable;
    public GameObject pressureMeterVFX;
    public GameObject pressureMeterClickable;
    public SpriteRenderer tempBackgroundGenerator;
    public SpriteRenderer tempBackgroundInteractable;
    public GameObject tempMenuClickable;
    public AudioManager audioManager;
    public Color green;
    public Color red;
    private bool backgroundDebounce = false;
    private bool meterDebounce = false;

    [Header ("---- Camera Stuff ----")]
    public Vector3 defaultCamPosition;
    public Vector3 tempCamPosition;
    public Vector3 pressureCamPosition;
    public Color defaultBackgroundColor;
    public Color interactBackgroundColor;

    [Header("----Stats----")]
    public UIController uiController;
    public Transform statsDisplayContainer;
    public float shakeMagnitude;
    public float stability;
    public float tempurature;
    public float psi;
    public bool switchActivated = false;
    public bool powerOn = true;

    [Header("----Audio Clips----")]
    public AudioClip pressureReleaseAudio;
    public AudioClip coolDownAudio;
    public AudioClip powerSwitchAudio;
    public AudioClip powerDownAudio;
    public AudioClip powerUpAudio;

    static private float baseTempSpeed = 0.6f;
    static private float basePsiSpeed = 0.32f;
    static private float stabilitySpeed = 0.25f;
    private float tempSpeed = baseTempSpeed;
    private float psiSpeed = basePsiSpeed;
    private double time = Time.timeAsDouble;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        tempSpeed = baseTempSpeed;
        psiSpeed = basePsiSpeed;
        stabilitySpeed = 0.2f;

        if (!powerOn)
        {
            tempSpeed = -0.9f;
            psiSpeed  = -0.7f;
            stabilitySpeed += 1.2f;
        }
        else if (switchActivated)
        {
            tempSpeed *= 0.4f;
            psiSpeed  *= 3.2f;
            stabilitySpeed *= -0.8f;
        }

        if (stability < 35f)
        {
            tempSpeed += 0.7f;
            psiSpeed += 0.8f;
        }

        if (tempurature > 50f)
        {
            stabilitySpeed += 0.4f;
            tempSpeed += 0.4f;

            if (!backgroundDebounce)
            {
                StartCoroutine(FlashBackground());
                backgroundDebounce = true;
            }
        }

        if (psi > 65f)
        {
            psiSpeed += 0.5f;
            if (!meterDebounce)
            {
                meterDebounce = true;
                stabilitySpeed += 0.4f;
                StartCoroutine(ShakeMeter());
            }
        }

        tempurature = Mathf.Clamp(tempurature += tempSpeed * Time.deltaTime, 15f, 100f);
        psi = Mathf.Clamp(psi += psiSpeed * Time.deltaTime, 10f, 100f);
        stability = Mathf.Clamp(stability -= stabilitySpeed * Time.deltaTime, 0f, 100f);
        
        uiController.UpdateText();
    }

    public void HandleClick(GameObject clickedObject)
    {
        if (clickedObject.name == "ReturnClickable")
        {
            cam.transform.position = defaultCamPosition;
            cam.backgroundColor = defaultBackgroundColor;

            foreach (Transform child in statsDisplayContainer)
            {
                child.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }

        else if (clickedObject == shutdownButtonClickable)
        {
            powerOn = !powerOn;
            uiController.UpdatePowerLabel();

            if (!powerOn)
            {
                stability -= 4f;
                audioManager.PlaySound(powerDownAudio, powerOn);

                switchActivated = false;
                Vector3 rotation = powerSwitchVFX.transform.eulerAngles;
                rotation.z = 50f;
                powerSwitchVFX.transform.eulerAngles = rotation;
            }
            else audioManager.PlaySound(powerUpAudio, powerOn);
        }

        else if (clickedObject == pressureMeterClickable)
        {
            cam.transform.position = pressureCamPosition;
            cam.backgroundColor = interactBackgroundColor;

            statsDisplayContainer.Find("PSI").GetComponent<TextMeshProUGUI>().enabled = false;
        }

        else if (clickedObject == tempMenuClickable)
        {
            cam.transform.position = tempCamPosition;
            cam.backgroundColor = interactBackgroundColor;

            statsDisplayContainer.Find("Celcius").GetComponent<TextMeshProUGUI>().enabled = false;
            statsDisplayContainer.Find("Farenheit").GetComponent<TextMeshProUGUI>().enabled = false;
        }

        else if (clickedObject == powerSwitchClickable)
        {
            Vector3 rotation = powerSwitchVFX.transform.eulerAngles;
            rotation.z *= -1;
            powerSwitchVFX.transform.eulerAngles = rotation;
            switchActivated = !switchActivated;

            audioManager.PlaySound(powerSwitchAudio);
        }

        else if (clickedObject == coolDownButtonClickable)
        {
            tempurature = Mathf.Clamp(tempurature - 9f, 0f, 100f);
            psi = Mathf.Clamp(psi + 5f, 10f, 100f);

            audioManager.PlaySound(coolDownAudio);
        }

        else if (clickedObject == ventButtonClickable)
        {
            psi = Mathf.Clamp(psi - 7f, 10f, 100f);
            tempurature = Mathf.Clamp(tempurature + 5f, 0f, 100f);

            audioManager.PlaySound(pressureReleaseAudio);
        }
    }

    IEnumerator FlashBackground()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            tempBackgroundGenerator.color = red;
            tempBackgroundInteractable.color = red;

            yield return new WaitForSeconds(0.4f);
            tempBackgroundGenerator.color = green;
            tempBackgroundInteractable.color = green;

            if (tempurature < 50f)
            {
                backgroundDebounce = false;
                yield break;
            }
        }
    }

    IEnumerator ShakeMeter()
    {
        Vector2 originalPosition = pressureMeterVFX.transform.position;

        while (true)
        {
            Vector2 newPosition = new Vector2(Random.Range(-shakeMagnitude, shakeMagnitude), Random.Range(-shakeMagnitude, shakeMagnitude));
            pressureMeterVFX.transform.position = originalPosition + newPosition;
            
            yield return new WaitForEndOfFrame();

            if (psi < 65f)
            {
                pressureMeterVFX.transform.position = originalPosition;
                meterDebounce = false;
                yield break;
            }
        }
    }
}
