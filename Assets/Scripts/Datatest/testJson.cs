using UnityEngine;
using System.Collections;
using StarseedGeneral.LitJson;
using UnityEngine.UI;

public enum SwipeDirection
{
    None=0,
    Left=1,
    Right=2,
    Up=4,
    Down=8,
}
public class testJson : MonoBehaviour
{

    public string json;
    public Text log;
    // Use this for initialization
    void Start()
    {
      //  RootObjectSendReport ob = JsonMapper.ToObject<RootObjectSendReport>(json);
       

        string res = ReplateText(json);
        Debug.Log(res);
    }

    string ReplateText(string txt)
    {
        string lt = txt.Replace("'", " ");
        lt = lt.Replace("\\", " sss ");
        return lt;
    }
    // Update is called once per frame

   

  

   
}
