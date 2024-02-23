using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BasicController : MonoBehaviour
{
    private int FrameRate = 0;
    public int initFrameRate = 60;
    public int MaxFrameRate = 150;
    public VNectBarracudaRunner15Basket[] VNectModels;

    private Vector3 initPosition;
    private Transform[] HeadTransforms;
    private Vector3[] HeadPositions;
    public Transform CameraTransform;
    private int FollowNum = 30;

    public Button[] FollowButtons;
    private float smoothing = 3.0f;
    // Start is called before the first frame update
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = initFrameRate;
        FrameRate = initFrameRate;
        Random.seed = 1;
        initPosition = new Vector3(18, 0, -18);
        for (int i = 0; i < VNectModels.Length; i++)
        {
            HeadTransforms[i] = VNectModels[i].jointPoints[PositionIndex15.head.Int()].Transform;
        }

        FollowNum = 30;
        for (int i = 0; i < 10; i++)
        {
            int tem = i;
            FollowButtons[i].onClick.AddListener(delegate { FollowCharacter(tem); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = FrameRate;
        Debug.Log("FollowNum: " + FollowNum);
    }

    void LateUpdate()
    {
        if (FollowNum == -1 || FollowNum >= VNectModels.Length)
        {
            // CameraTransform.position = Vector3.Lerp(CameraTransform.position, initPosition, smoothing * Time.deltaTime);
            Debug.Log("FollowNum: " + FollowNum);
            return;
        }
        else{
            Debug.Log("FollowNum: " + FollowNum);
        // Vector3 targetPosition = HeadTransforms[FollowNum].position;
        // CameraTransform.position = Vector3.Lerp(CameraTransform.position, targetPosition, smoothing * Time.deltaTime);
        // CameraTransform.LookAt(HeadTransforms[FollowNum]);
        }
    }
    
    public void StopFollow()
    {
        FollowNum = -1;
    }

    public void FollowCharacter(int num)
    {
        FollowNum = num;
    }
    
    public void FC1()
    {
        FollowNum = 0;
    }

    public void FC2()
    {
        FollowNum = 1;
    }

    public void FC3()
    {
        FollowNum = 2;
    }

    public void FC4()
    {
        FollowNum = 3;
    }

    public void FC5()
    {
        FollowNum = 4;
    }

    public void FC6()
    {
        FollowNum = 5;
    }

    public void FC7()
    {
        FollowNum = 6;
    }

    public void FC8()
    {
        FollowNum = 7;
    }

    public void FC9()
    {
        FollowNum = 8;
    }

    public void FC10()
    {
        FollowNum = 9;
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
