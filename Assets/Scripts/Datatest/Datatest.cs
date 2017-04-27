using UnityEngine;
using System.Collections;

public class Datatest : MonoBehaviour
{
    public static Datatest instance;
    [HideInInspector]
    public string textdt;
    [HideInInspector]
    public string textAM;
    void Awake()
    {
        instance = this;
    }
  
   public void Start()
    {
      //TextAsset textFile = (TextAsset)Resources.Load("dataInit");
      //  textdt = textFile.text;    
    }
}
