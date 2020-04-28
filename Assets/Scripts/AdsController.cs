using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using static Globals;

public class AdsController : IUnityAdsListener
{
    public AdsController()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(googleId, TestMode);
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error: " + message);
    }
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Finish");
        try
        {
            session.AddLife();
        }
        catch
        {
            Debug.Log("AddLife");
        }
        try
        {
            session.adIsUsed = true;
            respawnPlayer = true;
        }
        catch
        {
            Debug.Log("LastChanceWindowController");
        }
    }
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Start");
    }
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Ready");
    }
}
