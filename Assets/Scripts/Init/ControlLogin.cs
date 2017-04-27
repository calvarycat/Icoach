using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using StarseedGeneral.LitJson;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class ControlLogin : MonoBehaviour
{

    public const string userLogin = "http://vn1ln01.int.grs.net/api/user/login";
    public Dropdown drUserAM;
    RootObject dataAM;
    public ShowMessage showmes;
    private bool runUpdate = true;
    public GameObject LoadingPanel;

 


    void Start()
    {
       // LoginTime= DateTime.Today;
        int userId = PlayerPrefs.GetInt(KeySaving.Islogin.ToString(), -1);
        if (userId != -1)
        {
            string valueUser = PlayerPrefs.GetString(KeySaving.ValueAM.ToString(), "");
            if (!string.IsNullOrEmpty(valueUser))
            {
                runUpdate = false;
                AMInfor am = JsonMapper.ToObject<AMInfor>(valueUser);
                UserAuthentication.instance.aminfo = am;
                SceneManager.LoadScene(SceneName.Loading.ToString());
            }
            else
            {
                runUpdate = true;
                StartCoroutine(UserLogin());
            }
        }
        else
        {
            runUpdate = true;
            StartCoroutine(UserLogin());
        }

    }

  
   

    private bool isProcessLoading;

    private bool LoadUser = false;
    private bool isCanProcess = false;
    public GameObject pnWaitingInit;
    public GameObject MainInitAm;




    void Update()
    {
        if (runUpdate)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                showmes.Show("No internet connection!");
                LoadingPanel.SetActive(false);
                return;
            }

            if (!LoadUser && !isProcessLoading)
            {
                if (!showmes.pnShow.gameObject.activeSelf)
                {
               
                    StartCoroutine(UserLogin());
                }

            }
        }
    }

    private IEnumerator UserLogin()
    {
        if (!LoadUser)
        {
            LoadingPanel.SetActive(true);
            isProcessLoading = true;
            WWW dt = new WWW(userLogin);
            yield return dt;
            if (dt.error == null)
            {
                JsonData jsonvale = JsonMapper.ToObject(dt.text);
                int idx = int.Parse(jsonvale["success"].ToString());
                try
                {
                    dataAM = JsonMapper.ToObject<RootObject>(dt.text);
                    LoadDropdowUser();
                    LoadUser = true;
                    isProcessLoading = true;
                    LoadingPanel.SetActive(false);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    LoadUser = false;
                    LoadingPanel.SetActive(false);

                }
            }
            else
            {
                showmes.Show("Loading failed, please try again!");
                isProcessLoading = false;
                isShowFaild = true;
                LoadingPanel.SetActive(false);

            }
        }

    }

    private bool isShowFaild;

    List<string> listAM;
    public void LoadDropdowUser()
    {
        if (dataAM.result != null)
        {
            dataAM.result = dataAM.result.OrderBy(o => o.AM_Name).ToArray<AMInfor>();
            listAM = new List<string>();
            listAM.Add("Choose an AM member");
            foreach (AMInfor r in dataAM.result)
            {
                listAM.Add(r.AM_Name);
            }
            drUserAM.AddOptions(listAM);
            MainInitAm.SetActive(true);
            LoadUser = true;
        }
        else
        {
            isProcessLoading = false;
            LoadUser = false;
            MainInitAm.SetActive(false);
           
            showmes.Show("Loading failed, please try again!");
        }
        LoadingPanel.SetActive(false);

    }
    public void ChooseUserLogin(int id)
    {
        UserAuthentication.instance.dropuserID = id;
    }
    public void ProcessLoading()
    {

        if (UserAuthentication.instance.dropuserID > 0)
        {
            UserAuthentication.instance.aminfo = dataAM.result[UserAuthentication.instance.dropuserID - 1];
            string valueUser = JsonMapper.ToJson(dataAM.result[UserAuthentication.instance.dropuserID - 1]);
            PlayerPrefs.SetInt(KeySaving.Islogin.ToString(), UserAuthentication.instance.dropuserID - 1);
            PlayerPrefs.SetString(KeySaving.ValueAM.ToString(), valueUser);
            Invoke("LoadScene", 1f);
        }
        else
        {
            showmes.Show("Please choose a member");
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneName.Loading.ToString());
    }



}
[System.Serializable]
public class AMInfor
{
    public int AM_ID;
    public string AM_Name;
    public int RegionID;
    public string RegionName;
    public int SectorID;
    public string SectorName;
    public int TerritoryID;
    public string TerritoryName;
    public int TeamTypeID;
    public string TeamTypeName;
    public int TeamID;
    public string TeamName;
}

public class RootObject
{
    public int success;
    public int error;
    public string msg;
    public AMInfor[] result;
}
public class parseJSON
{
    public string title;
    public string id;
    public ArrayList but_title;
    public ArrayList but_image;
}