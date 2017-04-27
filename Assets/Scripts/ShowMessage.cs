using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour {
    public Text mesage;
    public GameObject pnShow;
	// Use this for initialization
	void Start () {
	
	}
	public void Show(string sms)
    {
        pnShow.gameObject.SetActive(true);
        mesage.text = sms;
    }
    public void CloseMesage()
    {
        pnShow.SetActive(false);
        mesage.text = null;
    }
}
