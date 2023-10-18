using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepSound;
    [SerializeField] private AudioClip[] zombieSound;
    [SerializeField] private AudioClip blinded;
    private AudioSource zombieSoundAudioSource;
    private AudioSource zombieStepSoundAudioSource;
    [SerializeField] private float soundInterval = 2f; 
    private float timeSinceLastSound = 0f;
    public bool getBlinded;
    private void Start()
    {
        zombieStepSoundAudioSource = GetComponent<AudioSource>();
        zombieSoundAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!getBlinded)
        {


            timeSinceLastSound += Time.deltaTime;
            if (timeSinceLastSound >= soundInterval)
            {
                PlayRandomZombieSound();
                timeSinceLastSound = 0f;
            }
        }
        else
        {
            zombieSoundAudioSource.PlayOneShot(blinded);
            getBlinded = false;
        }

    }

    private void PlayRandomZombieSound()
    {
        int randomIndex = Random.Range(0, zombieSound.Length);
        zombieSoundAudioSource.PlayOneShot(zombieSound[randomIndex]);
    }
    public void StepSound()
    {
        int randomIndex = Random.Range(0, stepSound.Length);
        zombieSoundAudioSource.PlayOneShot(stepSound[randomIndex]);
    }
}
