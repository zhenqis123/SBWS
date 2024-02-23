using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicController : MonoBehaviour
{
    private int FrameRate = 0;
    public int initFrameRate = 60;
    public int MaxFrameRate = 150;
    public VNectBarracudaRunner15Basket[] VNectModels;

    private Transform[] VNectTransforms;
    private Vector3[] VNectPositions;
    private Transform CameraTransform;
    private int FollowNum;

    public Button[] FollowButtons;
    private float smoothing = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = initFrameRate;
        FrameRate = initFrameRate;
        Random.seed = 1;
        for (int i = 0; i < VNectModels.Length; i++)
        {
            VNectTransforms[i] = VNectModels[i].transform;
        }
        CameraTransform = Camera.main.transform;

        FollowNum = -1;
        for (int i = 0; i < FollowButtons.Length; i++)
        {
            FollowButtons[i].onClick.AddListener(delegate { FollowCharacter(FollowNum); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = FrameRate;
    }

    void LateUpdate()
    {
        if (FollowNum == -1)
        {
            return;
        }
        Vector3 targetPosition = VNectTransforms[FollowNum].position;
        CameraTransform.position = Vector3.Lerp(CameraTransform.position, targetPosition, smoothing * Time.deltaTime);
        CameraTransform.LookAt(VNectTransforms[FollowNum]);
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
}
