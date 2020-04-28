using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    public Toggle musicToggle, soundToogle;
    public Button resumeBtn, exitBtn;

    private void Start()
    {
        musicToggle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>((x) =>
        {
            if (Globals.game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();

            if (x)
            {
                Globals.game.isMusic = false;
                GameObject.Find("Music(Clone)").GetComponent<MusicController>().doItOnce = true;
            }
            else
            {
                Globals.game.isMusic = true;
                GameObject.Find("Music(Clone)").GetComponent<MusicController>().doItOnce = true;
            }
        }));
        soundToogle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>((x) =>
        {
            if (x)
            {
                Globals.game.isSounds = false;
            }
            else
            {
                Globals.game.isSounds = true;
            }
            if (Globals.game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
        }));

        if (resumeBtn != null)
            resumeBtn.onClick.AddListener(Resume_OnClick);
        exitBtn.onClick.AddListener(Exit_OnClick);

        musicToggle.isOn = !Globals.game.isMusic;
        soundToogle.isOn = !Globals.game.isSounds;
    }

    private void Exit_OnClick()
    {
        if (Globals.game.isSounds)
            GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();

        if (resumeBtn != null)
        {
            Time.timeScale = 1f;
            Globals.CreateBlure(BlureType.Out, transform.parent, "MainScene");
        }
        else
            Destroy(gameObject);
    }
    private void Resume_OnClick()
    {
        if (Globals.game.isSounds)
            GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }
}
