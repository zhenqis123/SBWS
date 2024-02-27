using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;
    public Camera mainCamera;


    void Start()
    {
        canvas1.SetActive(true);
        canvas2.SetActive(false);
    }

    public void SwitchCanvas()
    {
        mainCamera.enabled = true;
        bool isCanvas1Active = canvas1.activeSelf;
        canvas1.SetActive(!isCanvas1Active);
        canvas2.SetActive(isCanvas1Active);
    }
}