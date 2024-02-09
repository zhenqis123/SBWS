using UnityEngine;
using UnityEngine.EventSystems;

public class RectangleControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform objectToMove;  // 要移动的物体
    public float maxSpeed = 10f;    // 最大速度

    private Vector2 initialPosition; // 圆盘按钮的初始位置
    private Vector2 currentPosition; // 当前位置
    private bool isPressed;          // 标记圆盘按钮是否被按住

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (isPressed)
        {
            Vector2 direction = (currentPosition - initialPosition).normalized;  // 计算方向

            float distance = Vector2.Distance(currentPosition, initialPosition);
            float speed = Mathf.Clamp(distance, 0f, maxSpeed);  // 根据距离计算速度（限制在最大速度范围内）

            // 应用方向和速度到物体的移动
            objectToMove.Translate(new Vector3(0f, direction.y, 0f) * speed * Time.deltaTime, Space.World);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        currentPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //currentPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}