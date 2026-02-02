using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public Camera cam;
    public Canvas canvas;
    public TextMeshProUGUI gameOverText;

    public TimerController timerController;

    public void EndGame()
    {
        timerController.SaveTime();

        Destroy(GameObject.Find("ModsCanvas"));

        foreach (Transform child in canvas.transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in GameObject.Find("AudioManager").transform)
        {
            child.GetComponent<AudioSource>().volume = 0;
        }

        gameOverText.gameObject.SetActive(true);

        StartCoroutine(ReloadMainMenu());
    }

    IEnumerator ReloadMainMenu()
    {
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene("MainMenu");
    }
}
