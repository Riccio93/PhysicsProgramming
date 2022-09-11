using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct SoundLibrary
    {
        public string clipName;
        public AudioClip clipFile;
    }
    [SerializeField] private SoundLibrary[] soundLibrary;
    [SerializeField] private AudioSource ostSource;

    Dictionary<string, AudioClip> soundDictionary;

    private void Start()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach(SoundLibrary l in soundLibrary)
        {
            soundDictionary.Add(l.clipName, l.clipFile);
        }
        ostSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        ostSource.Play();
    }

    public void PlaySound(string name)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1);
        audioSource.clip = soundDictionary[name];
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }   
}
