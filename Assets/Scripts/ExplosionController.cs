using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private void Awake()
    {
        if (!Globals.game.isSounds)
            GetComponent<AudioSource>().Stop();
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
