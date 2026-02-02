using System.Collections;
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
    public GameObject generator;
    public Vector3 originalPosition;
    public Vector3 secondPosition;

    public TextMeshProUGUI easterEggText;
    public string[] messages = 
    {
    "just click play already man", 
    "i'm waiting.....",
    "do-do-do-dooooooo",
    "OH MY GOODNESS I'M DYING YOU NEED TO CLICK PLAY TO SAVE ME!",
    "that usually works...",
    "you're telling me i spent hours making this game just for you sit here?",
    "this main menu must be interesting to you, huh?",
    "might as well enjoy this royalty-free elevator music then",
    "zzzzzz...",
    "how has your computer not gone into sleep mode yet?",
    "you should really make sure that's enabled in your settings, dude",
    "gotta contribute to a more green earth. use less power, you get the gist",
    "REDUCE, REUSE, RECYCLE BABY",
    "sorry, i'll stop"
    };

    void Awake()
    {
        mainCanvas.enabled = true;
        instructionsCanvas.enabled = false;
        modsCanvas.enabled = false;
    }

    void Start()
    {
        StartCoroutine(EasterEgg());
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

    IEnumerator EasterEgg()
    {
        yield return new WaitForSeconds(60f);
        generator.SetActive(false);
        easterEggText.gameObject.SetActive(true);

        while (true)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                easterEggText.text = messages[i];
                yield return new WaitForSeconds(Random.Range(7f, 10f));
            }
        }
    }
}
