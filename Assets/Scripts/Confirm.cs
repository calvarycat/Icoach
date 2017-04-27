using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Confirm : MonoBehaviour
{
    public GameObject confirm;
    public Text txttitle;
    public Text txtcontent;


	// Use this for initialization
	void Start () {
	
	}

    public void InitConfirm(string title,string content)
    {
        txttitle.text = title;
        txtcontent.text = content;
    }

    public void Clean()
    {
        txttitle.text = "";
        txtcontent.text = "";
    }

    public void ShowConfirm(string title, string content)
    {
        InitConfirm(title, content);
        confirm.SetActive(true);
    }

    public void CloseConfirm()
    {
        Clean();
        confirm.SetActive(false);
    }
}
