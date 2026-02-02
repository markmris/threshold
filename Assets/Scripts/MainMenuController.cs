using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Camera cam;
    public Canvas mainCanvas;
    public Canvas instructionsCanvas;
    public Vector3 originalPosition;
    public Vector3 instructionsPosition;

    void Awake()
    {
        mainCanvas.enabled = true;
        instructionsCanvas.enabled = false;
    }
    
    public void OnStartClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnInstructionsClicked()
    {
        mainCanvas.enabled = false;
        instructionsCanvas.enabled = true;
        cam.transform.position = instructionsPosition;
    }

    public void OnReturnClicked()
    {
        instructionsCanvas.enabled = false;
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
}
