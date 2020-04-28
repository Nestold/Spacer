using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class HighscorePanelController : MonoBehaviour
{
    public Button okBtn;
    public Transform scoresHolder;

    private void Start()
    {
        okBtn.onClick.AddListener(() =>
        {
            if (Globals.game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        });

        for (int i = 0; i < scoresHolder.childCount; i++)
        {
            scoresHolder.GetChild(i).Find("Score").GetComponent<Text>().text = Globals.GetNumWithDots(Globals.game.player.highscore[i]);
        }
    }
}
