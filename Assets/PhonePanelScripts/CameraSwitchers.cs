using UnityEngine;
using UnityEngine.UI;

public class CameraSwitchers : MonoBehaviour
{
    public Camera mainCamera;
    public Camera[] cameras;
    public Button[] buttons;

    void Start()
    {
        // 初始化时只启用主摄像机
        mainCamera.enabled = true;
        foreach (var camera in cameras)
        {
            camera.enabled = false;
        }

        // 设置每个按钮的点击事件
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 需要在闭包中捕获循环变量
            buttons[i].onClick.AddListener(() => SwitchCameras(index));
        }
    }

    public void SwitchCameras(int index)
    {
        if (cameras[index].enabled)
        {
            // 如果当前已经是对应的摄像机，就切换回主摄像机
            cameras[index].enabled = false;
            mainCamera.enabled = true;
        }
        else
        {
            // 否则切换到对应的摄像机
            mainCamera.enabled = false;
            foreach (var camera in cameras)
            {
                camera.enabled = false;
            }
            cameras[index].enabled = true;
        }
    }
}