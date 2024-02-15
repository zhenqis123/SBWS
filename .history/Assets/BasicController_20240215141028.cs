using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    public int initFrameRate = 60;
    public VNectBarracudaRunner15Basket[] VNectModels;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        Random.seed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        foreach (var character in VNectModels)
        {
            character.ToggleAnimation();
        }
    }

}
