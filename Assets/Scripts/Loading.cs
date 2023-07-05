using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using System.Linq;

public class Loading : MonoBehaviour
{
    public static Loading Instance { get; private set; }
    [SerializeField] GameObject loaderPopup;
    [SerializeField] Image progressBar;
    
    private void Awake()
    {
        if(Instance ==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }    
    }

    private void Start()
    {
        //LoadScene("GameScene", 5);
    }
    public  async void LoadScene(string sceneName,int maxTime,List<Func<bool>>action=null)
    {
        float time = 0f;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loaderPopup.SetActive(true);

        if (action == null)
        {
            while (time < maxTime)
            {
                await Task.Delay(50);
                time += 0.05f;
                progressBar.fillAmount = time / maxTime;
            }
        }
        else
        {
            bool allActionsDone = action.All(a => a.Invoke());
            while (time < maxTime && !allActionsDone)
            {
                await Task.Delay(50);
                time += 0.05f;
                progressBar.fillAmount = time / maxTime;
                allActionsDone = action.All(a => a.Invoke());
            }
        }
        progressBar.fillAmount = 1f;
        await Task.Delay(500);
        scene.allowSceneActivation = true;
        loaderPopup.SetActive(false);
    }    

    public async  void LoadSceneNormal(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        loaderPopup.SetActive(true);

        do
        {
            await Task.Delay(50);
            progressBar.fillAmount = scene.progress;
        }while(scene.progress > 0.9f);

        await Task.Delay(100);
        scene.allowSceneActivation = true;
        loaderPopup.SetActive(false);
    }    
}
