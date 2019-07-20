using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{

    Slider slider;
    [Range(0, 1)]
    public static float rage = 0;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, rage, 4 * Time.deltaTime);
    }
}
