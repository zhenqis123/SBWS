using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    public void SwitchCamera()
    {
        bool isCamera1Active = camera1.enabled;
        camera1.enabled = !isCamera1Active;
        camera2.enabled = isCamera1Active;
    }
}