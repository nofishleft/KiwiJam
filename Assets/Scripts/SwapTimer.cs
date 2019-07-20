using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapTimer : MonoBehaviour
{
    Image filled;

    // Start is called before the first frame update
    void Start()
    {
        filled = GetComponent<Image>();
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (true)
        {
            float percent = TileController.timeStatic / TileController.timeStaticMax;

            filled.fillAmount = percent;
            yield return null;
        }
    }
}
