using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.InputSystem;

public class BasicController : MonoBehaviour
{
    private int FrameRate = 0;
    public int initFrameRate = 60;
    public int MaxFrameRate = 150;
    public VNectBarracudaRunner15Basket[] VNectModels;
    public GameObject VRUI;

    private Transform CourtTransform;
    private Vector3 InitPos;
    private Vector3 CourtMove;
    private int FollowNum = -1;

    public Button[] FollowButtons;
    private float smoothing = 3.0f;

    private int count = 0;
    // Start is called before the first frame update
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = initFrameRate;
        FrameRate = initFrameRate;
        Random.seed = 1;
        CourtTransform = GameObject.Find("Basketball Court").transform;
        InitPos = CourtTransform.position;
        for(int i=0;i<FollowButtons.Length;i++)
        {
            int tem = i;
            FollowButtons[i].onClick.AddListener(() => FollowCharacter(tem));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = FrameRate;
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            StopFollow();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            makeInvisible();
        }

        CourtMove = CourtTransform.position - InitPos;
        for(int i = 0;i<VNectModels.Length;i++)
        {
            VNectModels[i].ApplyCourtMove(CourtMove);
        }
    }

    void LateUpdate()
    {
        count++;
    }
    
    public void StopFollow()
    {
        FollowNum = -1;
    }

    public void FollowCharacter(int num)
    {
        FollowNum = num;
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

    private void makeInvisible()
    {
        VRUI.SetActive(false);
    }
}
