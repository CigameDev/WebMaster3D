using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private bool IsAds = false;
    List<Func<bool>> listCondition = new List<Func<bool>>();

    private void Awake()
    {
        listCondition.Add(()=>IsAds);
        StartCoroutine(LoadAd());

    }
    private void Start()
    {
        Loading.Instance.LoadScene("GameScene", 30, listCondition);
    }

    private IEnumerator LoadAd()
    {
        yield return new WaitForSeconds(5);
        IsAds = true;
    }    
}
