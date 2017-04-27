using System;
using UnityEngine;
using System.Collections;
using System.Net.Mime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingData : MonoBehaviour
{
    public bool isFinishLoading;
    public GameObject panelLoading;
    public Image imgLoad;
    public ShowMessage showmes;
    public float timeOut = 100;
    private bool startRunUpdate;
    void Start()
    {
        int d = PlayerPrefs.GetInt(KeySaving.ControlLoadata.ToString(), 1);
        if (d == 1 || INitData.instance.isLoadingdata)
        {
            INitData.instance.isLoadingdata = false;
          
            INitData.instance.LoadingData(OnsuccessInit, OnFailed, NotInternet);
            isSyndata = true;
            startRunUpdate = true;
        }
        else
        {
          //  Application.LoadLevel(SceneName.Main.ToString());
            SceneManager.LoadScene(SceneName.Main.ToString());

        }


    }

    void NotInternet(string mes)
    {
     //   Debug.Log("No internet connection");
        showmes.Show(mes);
        isSyndata = false;
    }
    void OnsuccessInit(string mes)
    {
        //   Debug.Log(mes);
        PlayerPrefs.SetString(KeySaving.MainData.ToString(), "");
        PlayerPrefs.SetInt(KeySaving.ControlLoadata.ToString(), 2);
    //    Application.LoadLevel(SceneName.Main.ToString());

        SceneManager.LoadScene(SceneName.Main.ToString());
        isSyndata = false;
    }
    void OnFailed(string mes)
    {
        showmes.Show("Loading failed please try again");
  //      Debug.Log("Loading failed " + mes);
        isSyndata = false;
    }



    void Update()
    {
        if (startRunUpdate)
        {
            if (!showmes.pnShow.activeSelf)
            {
                imgLoad.fillAmount += Time.deltaTime * 0.5f;
                if (imgLoad.fillAmount == 1)
                {
                    imgLoad.fillAmount = 0;
                }
                if (!isSyndata)
                {
                    isSyndata = true;
                    INitData.instance.LoadingData(OnsuccessInit, OnFailed, NotInternet);
                    startRunUpdate = true;
                }
            }

        }
    }

    bool isSyndata = false;
}