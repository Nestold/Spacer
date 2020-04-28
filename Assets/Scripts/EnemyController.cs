using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Globals;
public class EnemyController : MonoBehaviour
{
    public int lifes;
    public int arriveSpeed;
    public int chanceToShoot;
    public bool godMode;
    AudioSource fireSound;

    bool canFire = true;
    bool randomFire = false;

    Vector3 startingPos = Vector3.zero;

    Rigidbody2D rb;

    public BulletType weaponType;

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        startingPos = transform.localPosition;
        transform.localPosition = new Vector3(startingPos.x, 8, startingPos.z);
        rb.velocity = new Vector2(0, -arriveSpeed);
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.y <= startingPos.y)
            rb.velocity = new Vector2(0, 0);
    }

    private void Update()
    {
        if (canFire && randomFire)
        {
            if (game.isSounds)
                fireSound.Play();
            CreateBullet(weaponType, transform);
            StartCoroutine(CanFire());
        }

        var randFire = rand.Next(0, 1000);
        if (randFire <= chanceToShoot)
        {
            StartCoroutine(CanFire());
            randomFire = true;
        }
        else
            randomFire = false;
    }

    IEnumerator CanFire()
    {
        canFire = false;
        yield return new WaitForSeconds(1f);
        canFire = true;
    }

    void DropOnDeath()
    {
        GameObject[] items = Resources.LoadAll<GameObject>("Prefabs/Items");

        int itemIndex = rand.Next(0, items.Length);
        int dropChance = rand.Next(0, name.Contains("_Big") ? 80 : 100);

        if (dropChance < items[itemIndex].GetComponent<ItemController>().chanceToLoot)
        {
            var item = Instantiate(items[itemIndex], transform.parent.parent, false);
            item.transform.localPosition = transform.localPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag.Equals("Enemy") && collision.tag.Equals("Player_Bullet") && !godMode)
        {
            if ((lifes - 1) > 0)
                lifes--;
            else
            {
                if (name.Contains("_Big"))
                    session.score += 300;
                else
                    session.score += 100;
                DropOnDeath();
                Instantiate(Resources.Load<GameObject>("Prefabs/Ships/ShipAudioDeath"), transform.parent, false);
                Destroy(gameObject);
            }
        }
    }
}
