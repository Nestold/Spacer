using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlureController : MonoBehaviour
{
    string sceneName = "";
    public void In()
    {
        GetComponent<Animator>().SetBool("In", true);
    }
    public void Out(string sceneName)
    {
        GetComponent<Animator>().SetBool("Out", true);
        this.sceneName = sceneName;
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void GoToScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}

public enum BlureType
{
    In,
    Out
}
