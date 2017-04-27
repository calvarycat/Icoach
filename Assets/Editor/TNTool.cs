using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class TNTool : MonoBehaviour {
    [MenuItem("MyMenu/Delete All key")]
    static  void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }
  
}
