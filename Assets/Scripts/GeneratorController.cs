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

    [Header ("---- Camera Positions ----")]
    public Vector2 tempCamPosition;
    public Vector2 pressureCamPosition;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void HandleClick(GameObject clickedObject)
    {
        switch (clickedObject)
        {
            
        }
    }
}
