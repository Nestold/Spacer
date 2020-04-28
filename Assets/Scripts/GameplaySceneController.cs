using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Globals;

public class GameplaySceneController : MonoBehaviour
{
    public Button pauseBtn;
    
    public Transform lifesHolder, armorHolder, extraLifeHolder;
    public Text highscore, pl1Score, pl2Score, level, cash;

    public Slider extraSpeed, extraBullet, extraTime;

    bool extraLifeGet = false;
    AudioSource sounds;

    Transform[] enemySets;

    private void Awake()
    {
        CreateBlure(BlureType.In, GameObject.Find("Canvas").transform);
        session = new Session();
        Time.timeScale = 1f;
        enemySets = Resources.LoadAll<Transform>("Prefabs/EnemySets");
        extraTime.maxValue = session.time;
        extraTime.value = session.time;
        StartCoroutine(TimeCounter());

        sounds = GameObject.Find("ButtonSound_0(Clone)").GetComponent<AudioSource>();
    }

    private void Start()
    {
        pauseBtn.onClick.AddListener(Pause_OnClick);

        highscore.text = GetNumWithDots(game.player.highscore[0]);

        Instantiate(Resources.Load<GameObject>("Prefabs/Ships/Player/Player_0"), transform.parent, false);
        enemySets[session.actualEnemySet] = Instantiate(enemySets[session.actualEnemySet], transform.parent, false);
        StartCoroutine(MusicDown());
    }

    private void Update()
    {
        UpdateLifes();
        UpdateArmor();
        UpdateExtraLife();
        UpdateScore();
        UpdateLevel();
        UpdateEnemySets();
        UpdateStatistics();

        if(respawnPlayer)
        {
            Respawn();
            respawnPlayer = false;
        }
    }

    private void UpdateStatistics()
    {
        extraSpeed.value = session.movemementSpeed;
        extraBullet.value = session.fireSpeed;
        cash.text = GetNumWithDots(session.cash) + " Z";
    }

    private void UpdateEnemySets()
    {
        if (enemySets[session.actualEnemySet].childCount == 0 && session.actualEnemySet < enemySets.Length - 1)
        {
            Debug.Log(session.level % 10);
            if (!GameObject.Find("Canvas").transform.Find("ShopPanel(Clone)") && session.level % 10 == 0)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopPanel"), GameObject.Find("Canvas").transform, false);
            }
            if (!GameObject.Find("Canvas").transform.Find("ShopPanel(Clone)"))
            {
                Destroy(enemySets[session.actualEnemySet].gameObject);
                session.actualEnemySet++;
                session.level++;
                enemySets[session.actualEnemySet] = Instantiate(enemySets[session.actualEnemySet], transform.parent, false);
                extraTime.value = session.time;
            }
        }
        else if (enemySets[enemySets.Length - 1].childCount == 0)
        {
            Debug.Log(session.level % 10);
            if (!GameObject.Find("Canvas").transform.Find("ShopPanel(Clone)") && session.level % 10 == 0)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopPanel"), GameObject.Find("Canvas").transform, false);
            }
            if (!GameObject.Find("Canvas").transform.Find("ShopPanel(Clone)"))
            {
                session.actualEnemySet = 0;
                enemySets = Resources.LoadAll<Transform>("Prefabs/EnemySets");
                foreach (Transform g in enemySets)
                {
                    for (int i = 0; i < g.childCount; i++)
                    {
                        g.GetChild(i).GetComponent<EnemyController>().lifes++;
                        g.GetChild(i).GetComponent<EnemyController>().chanceToShoot++;
                    }
                }
                enemySets[session.actualEnemySet] = Instantiate(enemySets[session.actualEnemySet], transform.parent, false);
                session.level++;
            }
        }
    }
    private void UpdateLevel()
    {
        level.text = Globals.session.level.ToString();
    }
    private void UpdateScore()
    {
        pl1Score.text = GetNumWithDots(session.score);
    }
    private void UpdateExtraLife()
    {
        for (int i = 0; i < extraLifeHolder.childCount; i++)
        {
            if (Globals.session.extraLife[i])
                extraLifeHolder.GetChild(i).gameObject.SetActive(true);
            else
                extraLifeHolder.GetChild(i).gameObject.SetActive(false);
        }

        foreach (bool b in Globals.session.extraLife)
        {
            if (b)
                extraLifeGet = true;
            else
            {
                extraLifeGet = false;
                break;
            }
        }
        if (extraLifeGet)
        {
            Globals.session.AddLife();
            Globals.session.extraLife = new bool[] { false, false, false, false, false };
        }
    }
    private void UpdateArmor()
    {
        for (int i = 0; i < armorHolder.childCount; i++)
        {
            if (i < Globals.session.armor)
                armorHolder.GetChild(i).gameObject.SetActive(true);
            else
                armorHolder.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void UpdateLifes()
    {
        for (int i = 0; i < lifesHolder.childCount; i++)
        {
            if (i < Globals.session.lifes)
                lifesHolder.GetChild(i).gameObject.SetActive(true);
            else
                lifesHolder.GetChild(i).gameObject.SetActive(false);
        }
    }
    public UnityAction EndSession()
    {
        return new UnityAction(() =>
        {
            game.player.AddHighscore(session.score);
            game.Save();
            CreateBlure(BlureType.Out, GameObject.Find("Canvas").transform, "MainScene");
            //SceneManager.LoadScene("MainScene");
        });
    }
    private void Pause_OnClick()
    {
        if (game.isSounds)
            sounds.Play();
        Time.timeScale = 0.00001f;
        Instantiate(Resources.Load<GameObject>("Prefabs/UI/PausePanel"), GameObject.Find("Canvas").transform, false);
    }
    public void Respawn()
    {
        var window = GameObject.Find("Canvas").transform.Find("LastChanceWindow(Clone)");
        if (window != null)
            Destroy(window.gameObject);
        StartCoroutine(ShipDestroyed());
    }
    IEnumerator ShipDestroyed()
    {
        Debug.Log("Start respawning");
        yield return new WaitForSeconds(2f);
        Instantiate(Resources.Load<GameObject>("Prefabs/Ships/Player/Player_0"), transform.parent, false);
    }
    IEnumerator TimeCounter()
    {
        yield return new WaitForSeconds(1f);
        if (extraTime.value > 0)
        {
            extraTime.value -= 1;
            StartCoroutine(TimeCounter());
        }
    }
    IEnumerator MusicDown()
    {
        yield return new WaitForSeconds(0.3f);
        if (GameObject.Find("Music(Clone)").GetComponent<AudioSource>().volume > 0.5f)
        {
            GameObject.Find("Music(Clone)").GetComponent<AudioSource>().volume -= 0.15f;
            StartCoroutine(MusicDown());
        }
    }
}
