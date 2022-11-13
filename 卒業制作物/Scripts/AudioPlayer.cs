using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private AudioClip enterClip;

    public void AudioPlay()
    {
        audio.PlayOneShot(clip);
    }
    public void OnCursor_Enter()
    {
        audio.PlayOneShot(enterClip);
    }

}
