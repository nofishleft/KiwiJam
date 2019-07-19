using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    static CameraManager instance;
    AudioListener audioListener;
    AudioSource audioSource;
    public float waitInterval = 2f;
    public AudioClip[] bgAudioClips;
    int audioClipIndex = 0;

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
            //Play
            AudioClip clip = bgAudioClips[audioClipIndex];
            audioClipIndex = (audioClipIndex + 1) % bgAudioClips.Length;
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length + waitInterval);
        }
    }
}
