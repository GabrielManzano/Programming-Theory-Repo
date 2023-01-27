using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerParent : MonoBehaviour
{
    public static ManagerParent Instance;
    private void Awake(){
        if(Instance != null){
            Destroy(gameObject);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
