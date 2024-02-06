using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppTimeScaleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Time.timeScale = 0.1f;
        //Debug.Log(Time.realtimeSinceStartup);
    }
    
}
