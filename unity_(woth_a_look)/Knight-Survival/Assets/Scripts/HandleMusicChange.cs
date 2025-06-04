using UnityEngine;

public class HandleMusicChange : MonoBehaviour
{
    public AudioSource music;
    public AudioSource music2;

    private int current = 0;

    void Start()
    {
        PlayCurrentTrack();
    }

    void Update()
    {
        ChangeMusicIfNoMusicIsPlaying();
    }

    private void ChangeMusicIfNoMusicIsPlaying()
    {
        if (!GetCurrentSource().isPlaying)
        {
            current = (current + 1) % 2;
            PlayCurrentTrack();
        }
    }

    void PlayCurrentTrack()
    {
        music.Stop();
        music2.Stop();

        GetCurrentSource().Play();
    }

    AudioSource GetCurrentSource()
    {
        return current == 0 ? music : music2;
    }
}
