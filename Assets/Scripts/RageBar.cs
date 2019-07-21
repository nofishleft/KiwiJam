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
        slider = GetComponent<Slider>();
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, rage, 4 * Time.deltaTime);

        if (slider.value >= 0.99)
        {
            SFXPlayer.PlayRageFullSound();
        }
    }

    IEnumerator restart()
    {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;
        gm.restartScene();

    }
}
