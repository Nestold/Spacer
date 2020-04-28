using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Globals;

public class PlayerController : MonoBehaviour
{
    PowerBtn aBtn, bBtn;
    Joystick joystick;
    GameplaySceneController gsc;
    AudioSource fireSound;


    bool fire = true;

    public bool godMode;

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();
        gsc = GameObject.Find("GameplaySceneController").GetComponent<GameplaySceneController>();

        joystick = GameObject.Find("Canvas").transform.Find("Controlls").Find("Fixed Joystick").GetComponent<Joystick>();
        aBtn = GameObject.Find("Canvas").transform.Find("Controlls").Find("ABtn").GetComponent<PowerBtn>();
        bBtn = GameObject.Find("Canvas").transform.Find("Controlls").Find("BBtn").GetComponent<PowerBtn>();

        aBtn.onClick.AddListener(A_OnClick);
        bBtn.onClick.AddListener(B_OnClick);

        if (!godMode)
            StartCoroutine(GodMode(2f));
        else
        {
            session.lifes = 3;
            session.armor = 3;
            session.autoFire = true;
            session.fireSpeed = 50;
            session.movemementSpeed = 10;
            session.weaponType = BulletType.Four_Red;
        }
    }
    private void A_OnClick()
    {
        if (!session.IsAutoFire() && fire)
        {
            if (game.isSounds)
                fireSound.Play();
            CreateBullet(session.weaponType, transform);
            StartCoroutine(FireTime());
        }
    }
    private void B_OnClick()
    {
        Debug.Log("Special Shoot");
    }
    private void FixedUpdate()
    {
        if ((joystick.Horizontal != 0f || joystick.Vertical != 0f) || (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f))
        {
            Vector2 move = new Vector2(joystick.Horizontal + Input.GetAxis("Horizontal"), joystick.Vertical + Input.GetAxis("Vertical"));
            move *= session.GetMovementSpeed();
            GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + move * Time.fixedDeltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !session.IsAutoFire() && fire)
        {
            CreateBullet(session.weaponType, transform);
            StartCoroutine(FireTime());
        }

        if(session.IsAutoFire())
        {
            if(aBtn.isPressed && fire)
            {
                if (game.isSounds)
                    fireSound.Play();
                CreateBullet(session.weaponType, transform);
                StartCoroutine(FireTime());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag.Equals("Bullet") || collision.tag.Equals("Enemy")) && !godMode)
        {
            if(session.armor != 0)
            {
                session.UnArmor();
                StartCoroutine(GodMode(0.5f));
            }
            else
            {
                if (session.movemementSpeed > 1)
                    session.movemementSpeed--;
                if (session.fireSpeed > 1)
                    session.fireSpeed--;
                session.autoFire = false;
                switch (session.weaponType)
                {
                    case BulletType.One_Blue:
                        session.weaponType = BulletType.One_Blue;
                        break;

                    case BulletType.Double_Green:
                        session.weaponType = BulletType.One_Blue;
                        break;

                    case BulletType.Triple_Blue:
                        session.weaponType = BulletType.Double_Green;
                        break;

                    case BulletType.Four_Red:
                        session.weaponType = BulletType.Triple_Blue;
                        break;

                    default:
                        session.weaponType = BulletType.Four_Red;
                        break;
                }
                Instantiate(Resources.Load<GameObject>("Prefabs/Ships/ShipAudioDeath"), transform.parent, false);
                if (session.UnLife())
                {
                    respawnPlayer = true;
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(gameObject);
                    if(session.adIsUsed)
                    {
                        gsc.EndSession().Invoke();
                    }
                    else
                    {
                        var lastChanceWindow = Instantiate(Resources.Load<GameObject>("Prefabs/UI/LastChanceWindow"), GameObject.Find("Canvas").transform, false);
                        lastChanceWindow.GetComponent<LastChanceWindowController>().Init(gsc.EndSession());
                    }
                }
            }
        }

        if (collision.tag.Equals("Item"))
        {
            if (collision.name.Contains("GreenGem"))
            {
                session.score += 100;
            }
            if (collision.name.Contains("RedGem"))
            {
                session.score += 200;
            }
            if (collision.name.Contains("PurpleGem"))
            {
                session.score += 300;
            }
            if (collision.name.Contains("SingleShot"))
            {
                if (session.weaponType != BulletType.One_Blue)
                    session.weaponType = BulletType.One_Blue;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("DoubleShot"))
            {
                if (session.weaponType != BulletType.Double_Green)
                    session.weaponType = BulletType.Double_Green;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("TripleShot"))
            {
                if (session.weaponType != BulletType.Triple_Blue)
                    session.weaponType = BulletType.Triple_Blue;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("FourShot"))
            {
                if (session.weaponType != BulletType.Four_Red)
                    session.weaponType = BulletType.Four_Red;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("ExtraLife_E"))
            {
                if (!session.extraLife[0])
                    session.extraLife[0] = true;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("ExtraLife_X"))
            {
                if (!session.extraLife[1])
                    session.extraLife[1] = true;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("ExtraLife_T"))
            {
                if (!session.extraLife[2])
                    session.extraLife[2] = true;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("ExtraLife_R"))
            {
                if (!session.extraLife[3])
                    session.extraLife[3] = true;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("ExtraLife_A"))
            {
                if (!session.extraLife[4])
                    session.extraLife[4] = true;
                else
                    session.score += 100;
            }
            if (collision.name.Contains("GetLife"))
            {
                session.AddLife();
            }
            if (collision.name.Contains("ExtraSpeed"))
            {
                session.movemementSpeed++;
            }
            if (collision.name.Contains("ExtraBullet"))
            {
                session.fireSpeed++;
            }
            if (collision.name.Contains("AutoFire"))
            {
                session.autoFire = true;
            }
            if (collision.name.Contains("Cash_10"))
            {
                session.cash += 10;
            }
            if (collision.name.Contains("Cash_50"))
            {
                session.cash += 50;
            }
            if (collision.name.Contains("Cash_100"))
            {
                session.cash += 100;
            }
            if (collision.name.Contains("Cash_200"))
            {
                session.cash += 200;
            }
        }
    }

    IEnumerator FireTime()
    {
        fire = false;
        yield return new WaitForSeconds(session.GetFireSpeed());
        fire = true;
    }
    IEnumerator GodMode(float duration)
    {
        godMode = true;
        yield return new WaitForSeconds(duration);
        godMode = false;
    }

    public void RestartAnimations()
    {
        foreach (var v in GetComponent<Animator>().parameters)
        {
            if (v.type == AnimatorControllerParameterType.Bool)
                GetComponent<Animator>().SetBool(v.name, false);
        }
    }
}
