using UnityEngine.Audio;
using UnityEngine;

public class Sound : MonoBehaviour {
    
    // Sound name, to identify when using the AudioManager
    [SerializeField] public new string name;
    // AudioClips that this songs has, can be one or more
    // When playing, they will be selected randomly to be played
    [SerializeField] private AudioClip[] samples;

    // Volume of this sound and it's variance
    [SerializeField] [Range(0f, 1f)] private float volume = 1f;
    [SerializeField] [Range(0f, 1f)] private float volumeVariance = .1f;
    // Pitch of this sound and it's variance
    [SerializeField] [Range(.1f, 3f)] private float pitch = 1f;
    [SerializeField] [Range(0f, 1f)] private float pitchVariance = .1f;

    // Further customization without changing the samples
    // Delay to begin the AudioClip
    // Cooldown to finish the AudioClip
    [SerializeField] private float[] delays;
    [SerializeField] private float[] cooldowns;
    private float cooldownCurr = 0;
    private int clipCurr = 0;

    // Flag to signalize if this Sound should be playing or not
    private bool playing = false;
    // Flag to signalize if this Sound must be played in loop
    [SerializeField] public bool loop = false;
    // Flag to signalize if this Sound must be stopped it's AudioClip execution
    // This is necessary for different sounds, eg: the sound of a footstep continues even if was the last step given, but a monster growl stops if the monster dies
    [SerializeField] public bool forceStop = false;

    // AudioSource to emit the sound
    [HideInInspector] public AudioSource source;

    // Simple function to get a random AudioClip from the samples
    private int GetRandomAudio() {
        return (int) UnityEngine.Random.Range(0f, samples.Length);
    }

    // Setting source of this Sound
    public void SetSource(AudioSource source) { this.source = source; }

    // Playing and stopping this sound
    public void Play() {
        playing = true;
        // Getting random sample and setting delay
        clipCurr = GetRandomAudio();
        if (delays.Length > 0)
            cooldownCurr = delays[clipCurr % delays.Length];
    }
    public void Stop() { playing = false; }

    public void Update() {
        // If has a source point
        if (source != null) {
            // And needs to be played
            if (playing) {
                // And isn't already beeing played and has 0 cooldown time
                if (!source.isPlaying && cooldownCurr == 0) {
                    // Will apply the new AudioClip to play
                    source.clip = samples[clipCurr];
                    source.volume = volume *
                        (1f + UnityEngine.Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
                    source.pitch = pitch *
                        (1f + UnityEngine.Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
                    source.Play();

                    // Reset cooldown
                    if (cooldowns.Length > 0) cooldownCurr = cooldowns[clipCurr % cooldowns.Length];

                    // If in loop, will call Play again to set the delay and the new AudioClip
                    if (loop) Play();
                    // Otherwise, there's no need to play another clip
                    else playing = false;
                }
            }
            //If the clip is not being played and must force the stop
            else if (forceStop)
                source.Stop();
        }

        // Decreasing cooldown
        cooldownCurr = Mathf.Max(0, cooldownCurr - Time.deltaTime);
    }
}