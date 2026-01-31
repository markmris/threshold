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
    public Color green;
    public Color red;
    private bool backgroundDebounce = false;

    [Header ("---- Camera Stuff ----")]
    public Vector3 defaultCamPosition;
    public Vector3 tempCamPosition;
    public Vector3 pressureCamPosition;
    public Color defaultBackgroundColor;
    public Color interactBackgroundColor;

    [Header("----Stats----")]
    public Transform statsDisplayContainer;
    public float stability;
    public float tempurature;
    public float psi;
    private bool switchActivated = false;
    private bool powerOn = true;

    static private float baseTempSpeed = 0.4f;
    static private float basePsiSpeed = 0.15f;
    static private float stabilitySpeed = 0.2f;
    private float tempSpeed = baseTempSpeed;
    private float psiSpeed = basePsiSpeed;

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
            tempSpeed = -0.4f;
            psiSpeed  = -0.3f;
            stabilitySpeed = -0.2f;
        }
        else if (switchActivated)
        {
            tempSpeed *= 0.5f;
            psiSpeed  *= 1.8f;
            stabilitySpeed += 0.3f;
        }

        if (stability < 35f)
        {
            tempSpeed += 0.7f;
            psiSpeed += 0.8f;
        }

        if (tempurature > 50f)
        {
            stabilitySpeed += 0.3f;

            if (!backgroundDebounce)
            {
                StartCoroutine(FlashBackground());
                backgroundDebounce = true;
            }
        }
        else
        {
            backgroundDebounce = false;
            StopCoroutine(FlashBackground());
        }

        if (psi > 80f)
        {
            stabilitySpeed += 0.4f;
        }

        tempurature += tempSpeed * Time.deltaTime;
        psi += psiSpeed * Time.deltaTime;
        stability -= stabilitySpeed * Time.deltaTime;

        tempurature = Mathf.Clamp(tempurature, 0f, 100f);
        psi = Mathf.Clamp(psi, 10f, 100f);
        stability = Mathf.Clamp(stability, 0f, 100f);
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
            stability -= 3f;

            switchActivated = false;
            Quaternion rotation = powerSwitchVFX.transform.rotation;
            rotation.z = 50f;
            powerSwitchVFX.transform.rotation = rotation;
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
            Quaternion rotation = powerSwitchVFX.transform.rotation;
            rotation.z *= -1;
            powerSwitchVFX.transform.rotation = rotation;
            switchActivated = !switchActivated;
        }

        else if (clickedObject == coolDownButtonClickable)
        {
            tempurature = Mathf.Clamp(tempurature - 9f, 0, 100);
            psi = Mathf.Clamp(psi - 5f, 0, 100);
        }

        else if (clickedObject == ventButtonClickable)
        {
            psi = Mathf.Clamp(psi - 7f, 0, 100);
            tempurature = Mathf.Clamp(psi - 5f, 0, 100);
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
        }
    }
}
