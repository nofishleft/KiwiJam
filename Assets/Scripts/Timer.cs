using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    public int durationSeconds = 90;
    public TextMeshProUGUI text;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        gm = GameObject.FindObjectOfType<GameManager>();
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    string getMinutesSeconds()
    {
        int min = Mathf.FloorToInt(durationSeconds / 60);
        int sec = durationSeconds - (min * 60);
        string secStr = sec.ToString();
        if (sec < 10)
        {
            secStr = '0' + secStr;
        }
        return min.ToString() + ':' + secStr.ToString();
    }

    IEnumerator Countdown()
    {
        while (durationSeconds > 0)
        {
            text.text = getMinutesSeconds();
            yield return new WaitForSeconds(1.0f);
            durationSeconds--;
        }

        gm.nextScene();

    }
}
