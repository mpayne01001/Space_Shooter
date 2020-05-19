using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    GameObject _explosionAudio;

    [SerializeField]
    GameObject _powerupAudio;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExplosionAudio()
    {
        var explosion = _explosionAudio.GetComponent<AudioSource>();
        if (explosion != null)
        {
            explosion.Play();
        }
    }

    public void PlayPowerupSound()
    {
        var powerup = _powerupAudio.GetComponent<AudioSource>();
        if (powerup != null)
        {
            powerup.Play();
        }
    }
}
