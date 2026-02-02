using System.Collections;
using TMPro;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    [Header ("---- Generator GameObjects ----")] 
    public GameObject shutdownButtonClickable;
    public GameObject powerLabelClickable;
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
    public GameOver gameOverScript;
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

    static private float baseTempSpeed = 0.6f;
    static private float basePsiSpeed = 0.32f;
    static private float stabilitySpeed = 0.25f;
    private float tempSpeed = baseTempSpeed;
    private float psiSpeed = basePsiSpeed;
    private float time;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        time = Time.time;
    }

    void Update()
    {
        tempSpeed = baseTempSpeed;
        psiSpeed = basePsiSpeed;
        stabilitySpeed = 0.2f;

        if (tempurature >= 80f || psi >= 80f || stability <= 0)
        {
            gameOverScript.EndGame();
            Destroy(this);
        }

        if (!powerOn)
        {
            tempSpeed = -0.9f;
            psiSpeed  = -0.7f;
            stabilitySpeed += .4f;
        }
        else if (switchActivated)
        {
            tempSpeed *= 0.4f;
            psiSpeed  *= 3.2f;
            stabilitySpeed *= -1f;
        }

        if (stability < 40f)
        {
            tempSpeed += 0.7f;
            psiSpeed += 0.8f;
        }

        if (tempurature > 55f)
        {
            stabilitySpeed += 0.6f;
            tempSpeed += 0.4f;

            if (!backgroundDebounce)
            {
                StartCoroutine(FlashBackground());
                backgroundDebounce = true;
            }
        }

        if (psi > 55f)
        {
            psiSpeed += 0.7f;
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

        else if (clickedObject == shutdownButtonClickable || clickedObject == powerLabelClickable)
        {
            if (CheckDebounce()) return;

            powerOn = !powerOn;
            uiController.UpdatePowerLabel();
            audioManager.GeneratorPowerSound(powerOn);

            if (!powerOn)
            {
                stability -= 4f;
                audioManager.PlaySound(powerDownAudio);

                switchActivated = false;
                Vector3 rotation = powerSwitchVFX.transform.eulerAngles;
                rotation.z = 50f;
                powerSwitchVFX.transform.eulerAngles = rotation;
            }
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
            if (CheckDebounce()) return;

            Vector3 rotation = powerSwitchVFX.transform.eulerAngles;
            rotation.z *= -1;
            powerSwitchVFX.transform.eulerAngles = rotation;
            switchActivated = !switchActivated;

            audioManager.PlaySound(powerSwitchAudio);
        }

        else if (clickedObject == coolDownButtonClickable)
        {
            if (CheckDebounce()) return;

            tempurature = Mathf.Clamp(tempurature - 9f, 0f, 100f);
            psi = Mathf.Clamp(psi + 5f, 10f, 100f);

            audioManager.PlaySound(coolDownAudio);
        }

        else if (clickedObject == ventButtonClickable)
        {
            if (CheckDebounce()) return;

            psi = Mathf.Clamp(psi - 7f, 10f, 100f);
            tempurature = Mathf.Clamp(tempurature + 5f, 0f, 100f);

            audioManager.PlaySound(pressureReleaseAudio);
        }
    }

    bool CheckDebounce()
    {
        if (Time.time - time < 1.5f) return true;

        else
        {
            time = Time.time;
            return false;
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
