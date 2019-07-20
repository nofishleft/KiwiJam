using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    public int initialDelay = 15;
    public static int initialDelayStatic;
    public int durationSeconds = 90;
    TextMeshProUGUI text;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        gm = GameObject.FindObjectOfType<GameManager>();
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(initialDelayCountdown());
    }

    string getMinutesSeconds(int time)
    {
        int min = Mathf.FloorToInt(time / 60);
        int sec = time - (min * 60);
        string secStr = sec.ToString();
        if (sec < 10)
        {
            secStr = '0' + secStr;
        }
        return min.ToString() + ':' + secStr.ToString();
    }

    IEnumerator initialDelayCountdown()
    {
        while (initialDelay > 0)
        {
            text.text = getMinutesSeconds(initialDelay);
            yield return new WaitForSeconds(1.0f);
            initialDelay--;
        }

        StartCoroutine(Countdown());

    }

    IEnumerator Countdown()
    {
        while (durationSeconds > 0)
        {
            text.text = getMinutesSeconds(durationSeconds);
            yield return new WaitForSeconds(1.0f);
            durationSeconds--;
        }

        gm.nextScene();

    }
}
