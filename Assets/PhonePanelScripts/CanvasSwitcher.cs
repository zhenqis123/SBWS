using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject canvas1;
    public GameObject canvas2;

    public void SwitchCanvas()
    {
        bool isCanvas1Active = canvas1.activeSelf;
        canvas1.SetActive(!isCanvas1Active);
        canvas2.SetActive(isCanvas1Active);
    }
}