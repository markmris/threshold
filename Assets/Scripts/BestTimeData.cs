using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BestTimeData : MonoBehaviour
{
    private int bestTime = 0;
    private string bestTimeString = "0:00";

    void Start()
    {
        if (GameObject.Find("TimeData")) Destroy(gameObject);

        gameObject.name = "TimeData";
        DontDestroyOnLoad(gameObject);
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            StartCoroutine(SetText());
        }
    }

    public void CheckNewTime(int newTime, string _bestTimeString)
    {
        if (newTime > bestTime)
        {
            bestTime = newTime;
            bestTimeString = _bestTimeString;
        }
    }

    IEnumerator SetText()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        TextMeshProUGUI bestTimeText = GameObject.Find("BestTime").GetComponent<TextMeshProUGUI>();
        bestTimeText.text = "Best Time: " + bestTimeString;
    }
}
