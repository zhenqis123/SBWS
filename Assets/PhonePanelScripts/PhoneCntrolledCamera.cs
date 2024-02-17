using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneCntrolledCamera : MonoBehaviour
{
    public float moveSpeed = 1.0f;  // 摄像机移动速度
    
    public void MoveCameraUp()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    public void MoveCameraDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

}