using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
public class CameraManager : MonoBehaviour
{

    static CameraManager instance;
    AudioListener audioListener;
    AudioSource audioSource;
    public float waitInterval = 2f;
    public string[] sceneNames; // Corresponding to the index in bgAudioClips
    public AudioList[] bgAudioClips; // By level

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        audioListener = GetComponent<AudioListener>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayTrack());
    }

    IEnumerator PlayTrack()
    {
        while (true)
        {

            // Find the index of the current scene
            string currentSceneName = SceneManager.GetActiveScene().name;
            int sceneIndex = -1;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (sceneNames[i] == currentSceneName)
                {
                    sceneIndex = i;
                    break;
                }
            }

            if (sceneIndex == -1)
            {
                yield return new WaitForSeconds(waitInterval);
            }

            //Play
            AudioClip clip = bgAudioClips[sceneIndex].list[Random.Range(0, bgAudioClips[sceneIndex].list.Length)];
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length + waitInterval);
        }
    }
}

[System.Serializable]
public class AudioList
{
    public AudioClip[] list;
}