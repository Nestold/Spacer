using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public Button infoBtn, highscoreBtn, optionsBtn, playBtn;
    AudioSource sounds;
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, transform);
        Globals.CreateBlure(BlureType.In, GameObject.Find("Canvas").transform);
        Globals.LoadData();
        if (!GameObject.Find("Music(Clone)"))
            DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/UI/Music"), transform.parent, false));
        if (!GameObject.Find("ButtonSound_0(Clone)"))
            DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/UI/ButtonSound_0"), transform.parent, false));
        if (!GameObject.Find("ButtonSound_1(Clone)"))
            DontDestroyOnLoad(Instantiate(Resources.Load<GameObject>("Prefabs/UI/ButtonSound_1"), transform.parent, false));
    }

    private void Start()
    {
        sounds = GameObject.Find("ButtonSound_0(Clone)").GetComponent<AudioSource>();
        infoBtn.onClick.AddListener(Info_OnClick);
        highscoreBtn.onClick.AddListener(Highscore_OnClick);
        optionsBtn.onClick.AddListener(Options_OnClick);
        playBtn.onClick.AddListener(Play_OnClick);

        StartCoroutine(MusicUp());
    }

    private void Play_OnClick()
    {
        if (Globals.game.isSounds)
            sounds.Play();

        Globals.CreateBlure(BlureType.Out, GameObject.Find("Canvas").transform, "GameplayScene");
        //SceneManager.LoadScene("GameplayScene");
    }
    private void Options_OnClick()
    {
        if (Globals.game.isSounds)
            sounds.Play();
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/OptionsPanel"), GameObject.Find("Canvas").transform, false);
    }
    private void Highscore_OnClick()
    {
        if (Globals.game.isSounds)
            sounds.Play();
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/HighscorePanel"), GameObject.Find("Canvas").transform, false);
    }
    private void Info_OnClick()
    {
        if (Globals.game.isSounds)
            sounds.Play();
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/InfoPanel"), GameObject.Find("Canvas").transform, false);
    }
    IEnumerator MusicUp()
    {
        yield return new WaitForSeconds(0.3f);
        if (GameObject.Find("Music(Clone)").GetComponent<AudioSource>().volume < 1f)
        {
            GameObject.Find("Music(Clone)").GetComponent<AudioSource>().volume += 0.1f;
            StartCoroutine(MusicUp());
        }
    }
}
