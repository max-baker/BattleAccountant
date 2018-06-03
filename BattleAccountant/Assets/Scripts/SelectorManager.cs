using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectorManager : MonoBehaviour
{
    public AudioClip sound;

    private AudioSource source;
    private Dropdown CrewDropdown;
    private Dropdown MechDropdown;
    // Use this for initialization
    void Start()
    {
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound;
        }
        CrewDropdown = this.gameObject.transform.Find("CrewSelector").GetComponent<Dropdown>();
        MechDropdown = this.gameObject.transform.Find("MechSelector").GetComponent<Dropdown>();
        CrewDropdown.onValueChanged.AddListener(CrewValueChanged);
        MechDropdown.onValueChanged.AddListener(MechValueChanged);
    }

    public void MechValueChanged(int change)
    {
        PlaySound();
    }

    public void CrewValueChanged(int change)
    {
        PlaySound();
        if (CrewDropdown.options[CrewDropdown.value].text == "None")
        {
            MechDropdown.interactable = false;
        }
        else
        {
            MechDropdown.interactable = true;
        }
    }
    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }
}
