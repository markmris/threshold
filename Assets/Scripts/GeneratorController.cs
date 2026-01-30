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

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void HandleClick(GameObject clickedObject)
    {
        if (clickedObject.name == "ReturnClickable")
        {
            cam.transform.position = defaultCamPosition;
            cam.backgroundColor = defaultBackgroundColor;
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
        }
    }
}
