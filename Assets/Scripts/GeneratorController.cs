using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    [Header ("---- Generator GameObjects ----")] 
    public GameObject shutdownButtonClickable;
    public GameObject powerSwitchVFX;
    public GameObject powerSwitchClickable;
    public GameObject pressureMeterVFX;
    public GameObject pressureMeterClickable;
    public GameObject tempBackground;
    public GameObject tempMenuClickable;

    [Header ("---- Camera Stuff ----")]
    public Vector3 defaultCamPosition;
    public Vector3 tempCamPosition;
    public Vector3 pressureCamPosition;
    public Color defaultBackgroundColor;
    public Color interactBackgroundColor;

    private float stability = 40f;
    private float tempurature = 15f;
    private float psi = 10f;
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
            tempSpeed += 0.5f;
            psiSpeed += 0.7f;
        }

        if (tempurature > 50f)
        {
            stabilitySpeed += 0.2f;
        }
        if (psi > 80f)
        {
            stabilitySpeed += 0.4f;
        }

        tempurature += tempSpeed * Time.deltaTime;
        psi += psiSpeed * Time.deltaTime;
        stability -= stabilitySpeed * Time.deltaTime;

        tempurature = Mathf.Clamp(tempurature, 0f, 100f);
        psi = Mathf.Clamp(psi, 0f, 100f);
        stability = Mathf.Clamp(stability, 0f, 100f);
    }

    public void HandleClick(GameObject clickedObject)
    {
        if (clickedObject.name == "ReturnClickable")
        {
            cam.transform.position = defaultCamPosition;
            cam.backgroundColor = defaultBackgroundColor;
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
        }

        else if (clickedObject == tempMenuClickable)
        {
            cam.transform.position = tempCamPosition;
            cam.backgroundColor = interactBackgroundColor;
        }

        else if (clickedObject == powerSwitchClickable)
        {
            Quaternion rotation = powerSwitchVFX.transform.rotation;
            rotation.z *= -1;
            powerSwitchVFX.transform.rotation = rotation;
            switchActivated = !switchActivated;
        }
    }
}
