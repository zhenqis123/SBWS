using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    private int FrameRate = 0;
    public int initFrameRate = 60;
    public VNectBarracudaRunner15Basket[] VNectModels;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = initFrameRate;
        FrameRate = initFrameRate;
        Random.seed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = FrameRate;
    }

    public void PauseGame()
    {
        foreach (var character in VNectModels)
        {
            character.ToggleAnimation();
        }
    }

    public void AddupFPS()
    {
        FrameRate += 5;
    }

    public void DeductFPS()
    {
        FrameRate -= 5;
    }

    public void SpeedUp()
    {
        time.timeScale = 1.5f;
    }

    public void SlowDown()
    {
        time.timeScale = 0.5f;
    }
}
