using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionAudio;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("Audio Source on Explosion is NULL!");

        _audioSource.clip = _explosionAudio;
        _audioSource.Play();

        Destroy(this.gameObject, 2.35f);
    }
}
