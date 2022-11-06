using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDrop_SE : MonoBehaviour
{
    public AudioClip SE;
    private AudioSource audio;
    private bool destroyCheck;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        destroyCheck = false;
    }

    private void Update()
    {
        if (!destroyCheck) return;
        if (audio.isPlaying)
        {
            Destroy(this);
        }
    }


    /// <summary>
    /// ‰½‚©‚É“–‚½‚Á‚½Žž‚É–Â‚ç‚·SE
    /// </summary>
    /// <param name="collision"></param>
    public void SEPlay()
    {
        audio.PlayOneShot(SE);
        destroyCheck = true;
    }
}
