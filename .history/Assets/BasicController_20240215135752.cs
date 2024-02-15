using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    public VNectModel22[] VNectModels;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.1f;
        Debug.Log(Time.realtimeSinceStartup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        VNectModel.ToggleAnimation();
        // Time.timeScale = 3;
    }

}
