using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    private AudioSource audioSource;
    private Dictionary<string, AudioClip> soundLibrary;
    public List<AudioClip> audioClips;

    void Start()
    {
        // Add an AudioSource component if it doesn't exist
        audioSource = gameObject.AddComponent<AudioSource>();
        soundLibrary = new Dictionary<string, AudioClip>();

        // Populate the dictionary with audio clips and their names
        foreach (AudioClip clip in audioClips)
        {
            soundLibrary.Add(clip.name, clip); // Add clip with its name as the key
        }
    }

    // Method to play a sound by name
    public void PlaySoundByName(string soundName)
    {
        // Check if the sound exists in the dictionary
        if (soundLibrary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundLibrary[soundName]);
        }
        else
        {
            Debug.LogWarning("Sound " + soundName + " not found in sound library.");
        }
    }
}
