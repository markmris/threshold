using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    private TextMeshProUGUI timer;
    private int minutes = 0;
    private int seconds = 0;
    public int totalTime;

    public BestTimeData bestTimeData;

    void Start()
    {
        StartCoroutine(CountTime());
        timer = transform.GetComponent<TextMeshProUGUI>();
        bestTimeData = GameObject.Find("TimeData").GetComponent<BestTimeData>();
    }

    IEnumerator CountTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            seconds++;
            minutes = seconds / 60;

            timer.text = Convert.ToString(minutes) + ':' + (seconds % 60).ToString("00");
        }
    }

    public void SaveTime()
    {
        totalTime = seconds;
        bestTimeData.CheckNewTime(totalTime, timer.text);
    }
}
