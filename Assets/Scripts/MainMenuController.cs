using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public Camera cam;
    public Canvas mainCanvas;
    public Canvas instructionsCanvas;
    public Canvas modsCanvas;
    public Vector3 originalPosition;
    public Vector3 secondPosition;

    public BestTimeData bestTimeData;

    void Awake()
    {
        mainCanvas.enabled = true;
        instructionsCanvas.enabled = false;
        modsCanvas.enabled = false;
    }
    
    public void OnStartClicked()
    {
        DontDestroyOnLoad(modsCanvas.gameObject);
        SceneManager.LoadScene("Game");
    }

    public void OnInstructionsClicked()
    {
        mainCanvas.enabled = false;
        instructionsCanvas.enabled = true;
        cam.transform.position = secondPosition;
    }

    public void OnReturnClicked()
    {
        instructionsCanvas.enabled = false;
        modsCanvas.enabled = false;
        mainCanvas.enabled = true;
        cam.transform.position = originalPosition;
    }

    public void OnExitClicked()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

    public void OnModsClicked()
    {
        mainCanvas.enabled = false;
        modsCanvas.enabled = true;
        cam.transform.position = secondPosition;
    }
}
