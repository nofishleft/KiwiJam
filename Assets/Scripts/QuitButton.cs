using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{

    Button playButton;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {

        playButton = GetComponent<Button>();
        gm = GameObject.FindObjectOfType<GameManager>();
        playButton.onClick.AddListener(delegate
        {
            gm.quitGame();
        });

    }

}
