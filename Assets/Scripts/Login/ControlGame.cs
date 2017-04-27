using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Contexts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlGame : MonoBehaviour
{
    public ShowMessage showMes;
    public GameObject MainPage;
    public GameObject LoGin;
    public InputField username, password;
    public GameObject loadingPanel;
    public GameObject pnSetting;
    public GameObject btnSetting;

    public void OnSettingClick()
    {
        if (pnSetting.activeSelf)
        {
            pnSetting.SetActive(false);
        }
        else
        {
            pnSetting.SetActive(true);
        }

    }


    private string mesage;
    public GameObject Popup;
    public Text txtMessage;

    public void ShowMesage(string mes)
    {
        txtMessage.text = mes;
        Popup.SetActive(true);
    }

    public void CloseMesage()
    {
        mesage = "";
        Popup.SetActive(true);
        Popup.SetActive(false);
    }

    public GameObject waitRespone;
  

    public void LogOut()
    {
        PlayerPrefs.SetInt(KeySaving.Login.ToString(), -1);
    }

    public void OnSynchonizeDataClick()
    {
       
      // PlayerPrefs.SetInt(KeySaving.ControlLoadata.ToString(),1); 
        INitData.instance.isLoadingdata = true;
        //   Application.LoadLevel(SceneName.Loading.ToString());
        SceneManager.LoadScene(SceneName.Loading.ToString());
    }

    public GameObject pnsetting;//for sync data

    public void OnnotSyncClick()
    {

        pnSetting.SetActive(false);
    }

}
