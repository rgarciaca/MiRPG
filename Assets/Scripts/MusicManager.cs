using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicas;
    public static MusicManager instance;
    private AudioSource audioSrc;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSrc = GetComponent<AudioSource>();
        ReproducirMusica(0);
    }

    public void ReproducirMusica(int indice)
    {
        audioSrc.clip = musicas[indice];
        audioSrc.Play();
    }
}
