using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public Button okBtn;

    private void Start()
    {
        okBtn.onClick.AddListener(() =>
        {
            if (Globals.game.isSounds)
                GameObject.Find("ButtonSound_1(Clone)").GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        });
    }
}
