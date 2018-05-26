using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOnClick : MonoBehaviour {
    public AudioClip sound;

    private AudioSource source;

    // Use this for initialization
    void Start () {  
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound;
        }
        gameObject.GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }
	
	
}
