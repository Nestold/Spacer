using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    IEnumerator Death()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((tag.Equals("Bullet") && collision.tag.Equals("Player")) || (tag.Equals("Player_Bullet") && collision.tag.Equals("Enemy")))
        {
            var explo = Instantiate(Resources.Load<GameObject>("Prefabs/Bullets/Explosion_Blue"), transform.parent, false);
            explo.transform.localPosition = transform.localPosition;
            Destroy(gameObject);
        }
    }

    internal void Init(string v, int angle = 0)
    {
        float velocity = 5f;

        var xmove = 0;

        if (angle > 0)
        {
            if(v.Equals("Player"))
                transform.Rotate(0, 0, 15);
            else
                transform.Rotate(0, 0, -15);
            xmove = -1;
        }
        else if (angle < 0)
        {
            if (v.Equals("Player"))
                transform.Rotate(0, 0, -15);
            else
                transform.Rotate(0, 0, 15);
            xmove = 1;
        }

        if (v.Equals("Player"))
        {
            tag = "Player_Bullet";
            GetComponent<Rigidbody2D>().velocity = new Vector2(xmove, velocity);
            StartCoroutine(Death());
        }
        else
        {
            tag = "Bullet";
            GetComponent<SpriteRenderer>().flipY = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xmove, -velocity);
            StartCoroutine(Death());
        }
    }
}
