using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    private int FrameRate = 0;
    public int initFrameRate = 60;
    public int MaxFrameRate = 150;
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
        if(Time.timeScale < 5.0f)
        {
            Time.timeScale = Time.timeScale * 1.1f;
        }
    }

    public void SlowDown()
    {
        if(Time.timeScale > 0.1f)
        {
            Time.timeScale = Time.timeScale / 1.1f;
        }
    }

    public void ResetSpeed(){
        Time.timeScale = 1.0f;
    }

    public void SetFPS(float sliderValue)
    {
        FrameRate = (int)(sliderValue * MaxFrameRate);
    }

    public void Replay()
    {
        foreach (var character in VNectModels)
        {
            character.Replay();
        }
    }
}
