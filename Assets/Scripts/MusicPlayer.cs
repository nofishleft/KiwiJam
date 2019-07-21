using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] MainMenu;

    private int TrackIndex = -1;
    public AudioClip[] PlayTracks;
    
    public AudioClip[] RotationPhase;

    public AudioSource source;

    [Range(0,1)]
    public float TrackVolume;

    private void Start()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }

        source.Stop();
        source.clip = MainMenu[Random.Range(0, MainMenu.Length)];
        source.volume = TrackVolume;
        source.loop = true;
        source.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        StopAllCoroutines();
        ++TrackIndex;
        StartCoroutine(Play(RotationPhase[Random.Range(0, RotationPhase.Length)], PlayTracks[TrackIndex]));
    }

    IEnumerator Play(AudioClip initial, AudioClip next)
    {
        source.Stop();
        source.clip = initial;
        source.loop = true;
        source.volume = TrackVolume;
        source.Play();
        while (LevelData.Started == false)
        {
            yield return null;
        }

        while (source.volume > 0)
        {
            source.volume = Mathf.Clamp(source.volume - Time.deltaTime, 0, 1);
            yield return null;
        }
        source.Stop();
        source.clip = next;
        source.loop = true;
        source.volume = TrackVolume;
        source.Play();
    }
}
