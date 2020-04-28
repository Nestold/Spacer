using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAudioController : MonoBehaviour
{
    private void Awake()
    {
        if (!Globals.game.isSounds)
            Destroy(gameObject);
    }
    private void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
            Destroy(gameObject);
    }
}
