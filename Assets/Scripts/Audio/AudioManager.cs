using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

    // If needs to have only one global instance, for UI sounds or main theme
    private static AudioManager instance;
    [SerializeField] private bool isGlobalInstance;

    // Array of sounds to manage
    [SerializeField] private Sound[] sounds;

    private void Awake() {
        //If needs to be the only one global audio manager
        if (isGlobalInstance) {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }
            else {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        //Adding the AudioSources to gameObject and setting one to each Sound
        foreach (Sound s in sounds)
            s.SetSource(gameObject.AddComponent<AudioSource>());
    }

    // Updating all sounds, because for some unknown reason, the Sound Monobehaviour's Update don't get called
    private void Update() {
        foreach (Sound s in sounds)
            s.Update();
    }

    // Playing the requested song
    public void Play(string soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s) s.Play();
        else Debug.LogWarning("Couldn't find Sound: " + soundName);
    }
    
    // Stopping the requested song
    public void Stop(string soundName) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s) s.Stop();
        else Debug.LogWarning("Couldn't find Sound: " + soundName);
    }

    // Setting loop for the requested song
    public void SetLoop(string soundName, bool loop) {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s) s.loop = loop;
        else Debug.LogWarning("Couldn't find Sound: " + soundName);
    }
}
