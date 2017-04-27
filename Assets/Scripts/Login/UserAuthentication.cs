using UnityEngine;
using System.Collections;

public class UserAuthentication : MonoBehaviour
{

    public static UserAuthentication instance;
    public AMInfor aminfo;
    public int dropuserID;
    public bool isLogin;

    void Awake()
    {

        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

   
 
}
