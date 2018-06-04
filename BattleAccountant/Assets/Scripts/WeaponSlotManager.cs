using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSlotManager : MonoBehaviour
{
    public AudioClip sound;
    public GameObject GameManager;
    public int SlotNumber;

    private AudioSource source;
    private Dropdown WeaponDropdown;
    // Use this for initialization
    void Start()
    {
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = sound;
        }
        WeaponDropdown = this.gameObject.transform.Find("WeaponSelector").GetComponent<Dropdown>();
        WeaponDropdown.onValueChanged.AddListener(WeaponChanged);
    }

    public void WeaponChanged(int change)
    {
        GameManager.GetComponent<MechManager>().ChangeMechWeapon(SlotNumber, change);
        PlaySound();
    }

    public void PlaySound()
    {
        source.PlayOneShot(sound);
    }
}
