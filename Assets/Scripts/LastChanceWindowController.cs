using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Globals;

public class LastChanceWindowController : MonoBehaviour
{
    public Button exitBtn, playBtn;

    UnityAction endSession;

    public void Init(UnityAction endSession)
    {
        this.endSession = endSession;
        exitBtn.onClick.AddListener(()=>
        {
            if (game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
            this.endSession.Invoke();
        });
        playBtn.onClick.AddListener(() =>
        {
            if (game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
            if (Advertisement.IsReady())
            {
                Advertisement.Show(placementId);
            }
        });
    }
}
