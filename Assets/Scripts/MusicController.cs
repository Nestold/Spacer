using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    AudioSource music;
    AudioClip[] musics;
    [HideInInspector]
    public bool doItOnce = true;

    public void Start()
    {
        music = GetComponent<AudioSource>();
        musics = Resources.LoadAll<AudioClip>("Music/BackgroundMusic");
    }

    private void Update()
    {
        if(doItOnce)
        {
            if (!Globals.game.isMusic)
                music.Stop();
            else
                GetMusicClip();
            doItOnce = false;
        }

        if (!music.isPlaying && Globals.game.isMusic)
        {
            doItOnce = true;
            StartCoroutine(PlayMusic());
        }
    }

    IEnumerator PlayMusic()
    {
        doItOnce = false;
        yield return new WaitForSeconds(3f);
        GetMusicClip();
    }

    public void GetMusicClip()
    {
        var musicIndex = Globals.rand.Next(0, musics.Length);
        music.clip = musics[musicIndex];
        music.Play();
    }
}
