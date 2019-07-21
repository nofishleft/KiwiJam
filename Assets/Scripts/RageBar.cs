using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{

    Slider slider;
    [Range(0, 1)]
    public static float rage = 0;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rage = 0;
        slider = GetComponent<Slider>();
        gm = GameObject.FindObjectOfType<GameManager>();
        StartCoroutine(UpdateFunction());
    }

    // Update is called once per frame
    IEnumerator UpdateFunction()
    {
        yield return null;
        while (slider.value < 0.99)
        {
            slider.value = Mathf.Lerp(slider.value, rage, 4 * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(restart());
    }

    IEnumerator restart()
    {
        SFXPlayer.PlayRageFullSound();
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;
        gm.restartScene();

    }
}
