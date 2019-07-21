using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioClip[] RotateSounds;
    public AudioClip[] SwapSounds;
    public AudioClip[] RageFullSounds;
    public AudioClip[] SelectSounds;

    static SFXPlayer player;

    // Start is called before the first frame update
    void Awake()
    {
        player = this;
    }
    
    public static void PlaySelectSound(Vector3 pos)
    {
        AudioClip clip = player.SelectSounds[Random.Range(0, player.SelectSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, pos, 0.4f);
    }

    public static void PlayRageFullSound()
    {
        AudioClip clip = player.RageFullSounds[Random.Range(0, player.RageFullSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, player.transform.position, 0.2f);
    }

    public static void PlaySwapSound(Vector3 pos)
    {
        AudioClip clip = player.SwapSounds[Random.Range(0, player.SwapSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, pos);
    }

    public static void PlayRotateSound(Vector3 pos)
    {
        AudioClip clip = player.RotateSounds[Random.Range(0, player.RotateSounds.Length)];
        AudioSource.PlayClipAtPoint(clip, pos);
    }
}
