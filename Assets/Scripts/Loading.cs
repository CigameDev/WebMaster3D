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
    [SerializeField] float _maxTimeLoading = 0f;
    [SerializeField] int _numberCondition = 0;
    [SerializeField]List<bool>_conditionDone = new List<bool>();

    Action<List<bool>> _onDone = null;
    float _loadingMaxvalue;
    bool _isLoadingStart = false;

    

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        fillImage.fillAmount = 0;
        
    }
    
    public Loading Init(Action<List<bool>> onDone = null)
    {
        for(int i=0;i<_numberCondition;i++)
        {
            _conditionDone.Add(false);
        }
        _loadingMaxvalue = 1f;
        fillImage.fillAmount = 0f;



        LoadingPopup.SetActive(true);
        _isLoadingStart = true;
        return this;
    }    

    public Loading Init(int numberCondition,Action<List<bool>>onDone = null)
    {
        _numberCondition = numberCondition;
        return Init(onDone);
    }
    private void Update()
    {
        //fillImage.fillAmount +=
    }


}
