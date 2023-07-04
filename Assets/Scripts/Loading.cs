using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Loading : MonoBehaviour
{
    public static Loading Instance { get; private set; }
    [SerializeField] Image fillImage;
    [SerializeField] GameObject LoadingPopup;
    [SerializeField] float _maxTimeLoading = 30f;
    [SerializeField] int _numberCondition = 0;
    [SerializeField]
    List<bool> _conditionDone = new List<bool>();


    Action<List<bool>> _onDone = null;

    float _loadingMaxvalue;
    bool _isLoadingStart = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        fillImage.fillAmount = 0;
        
    }
    private void Start()
    {
        StartCoroutine(IELoad(5));
        SceneManager.LoadSceneAsync(1);



    }

    //public Loading Init(Action<List<bool>>oneDone = null)
    //{
    //    _conditionDone.Clear();
    //    for(int i=0;i< _numberCondition; i++)
    //    {
    //        _conditionDone.Add(false);
    //    }    
    //}
    //public Loading Init(int numberCondition, Action<List<bool>> onDone = null)
    //{
    //    _numberCondition = numberCondition;
    //    return Init(onDone);
    //}
    public Loading SetMaxTimeLoading(float maxTime)
    {
        _maxTimeLoading = maxTime;
        return this;
    }
    private IEnumerator IELoad(float time)
    {
        float deltatime = 0f;
        while(deltatime < time)
        {
            deltatime += Time.deltaTime;
            yield return null;
            fillImage.fillAmount = deltatime/time;
        }
        this.gameObject.SetActive(false);
    }    
}
