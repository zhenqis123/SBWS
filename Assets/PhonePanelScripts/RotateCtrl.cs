using UnityEngine;
using System.Collections;

public class RotateCtrl : MonoBehaviour
{
    private bool onDrag = false; //是否被拖拽//
    public float speed = 6f; //旋转速度// 
    private float tempSpeed; //阻尼速度// 
    private float axisX; //鼠标沿水平方向移动的增量// 
    private float axisY; //鼠标沿竖直方向移动的增量// 
    private float cXY; //鼠标移动的距离//


    void OnMouseDown(){ //接受鼠标按下的事件// 
        print("OnMouseDown");
        axisX =0f;
        axisY =0f;
    }

    void OnMouseDrag () //鼠标拖拽时的操作// 
    {
        print("OnMouseDrag");
        onDrag = true;
        axisX = -Input.GetAxis ("Mouse X"); //获得鼠标增量// 
        axisY = Input.GetAxis ("Mouse Y");
        cXY = Mathf.Sqrt (axisX * axisX + axisY * axisY); //计算鼠标移动的长度// 
        if (cXY == 0f)
        {
            cXY =1f;
        }
    }

    float Rigid () //计算阻尼速度// 
    {
        if (onDrag)//在拖拽中
        {
            tempSpeed = speed;//速度恒定
        }
        else {//松开了
            if (tempSpeed> 0)//速度大于0
            {
                tempSpeed -= speed*2 * Time.deltaTime / cXY; //通过除以鼠标移动长度实现拖拽越长速度减缓越慢// 
            }
            else {
                tempSpeed = 0;
            }
        }
        return tempSpeed;
    }
    void Update () {
        gameObject.transform.Rotate (new Vector3 (axisY, axisX, 0) * Rigid (), Space.World);//全方位旋转

        //gameObject.transform.Rotate(new Vector3(gameObject.transform.rotation.x, axisX, 0) * Rigid(), Space.World);//只围绕y轴旋转
        if (!Input.GetMouseButton (0)) {
            onDrag = false;
        }

        //**********************************下方的判断是为了实现--点击屏幕任意位置都可对物体进行拖拽--的效果**************************
        //else if (Input.GetMouseButtonDown(0))//点击鼠标左键时
        //{
        //    axisX = 0f;
        //    axisY = 0f;
        //}
        //else if (Input.GetMouseButton(0))//持续按住鼠标左键时
        //{            
        //    MouseMove();
        //}
    }

    public void MouseMove()//拖拽
    {
        onDrag = true;
        axisX = -Input.GetAxis("Mouse X"); //获得鼠标增量// 
        axisY = Input.GetAxis("Mouse Y");
        cXY = Mathf.Sqrt(axisX * axisX + axisY * axisY); //计算鼠标移动的长度// 
        if (cXY == 0f)
        {
            cXY = 1f;
        }
    }
}
