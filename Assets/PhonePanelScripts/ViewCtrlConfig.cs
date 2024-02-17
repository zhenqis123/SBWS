 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Control
{
	public class ViewCtrlConfig : MonoBehaviour
	{
        #region   基础参数
        //用于控制操作旋转的物体
        public GameObject target ;
        //缩放系数
        public float distance = 56.0F;
        //左右滑动移动速度
        public float xSpeed = 20.0F;
        public float ySpeed = 1.0F;
        Vector3 tmp;
        //缩放限制系数
        public float yMinLimit = -20F;
        public float yMaxLimit = 80F;
        //摄像头的位置
        public float x = 0.0F;
        public float y = 0.0F;
        //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
        private Vector2 oldPosition1;
        private Vector2 oldPosition2;
        #endregion 
 
        //public GameObject moveObj;          //操作遥感移动的物体
 
        //初始化游戏信息设置
        void Start()
        {
            //this.transform.position = new Vector3(-52.05F,4.54F,-3.61F);
 
            Vector2 angles = transform.eulerAngles;
            x = angles.y-7F;//关于-7F是调节摄像机初始位置的偏移量
            y = angles.x-8F;//关于-8F是调节摄像机初始位置的偏移量
 
            Debug.Log("angles.x=" + angles.x);
            Debug.Log("angles.y=" + angles.y);
 
            // Make the rigid body not change rotation
            if (this.GetComponent<Rigidbody>())
                this.GetComponent<Rigidbody>().freezeRotation = true;
 
           
        }
 
        void Update()
        {
            //判断触摸数量为单点触摸
            if (Input.touchCount == 1)
            {
 
                //触摸类型为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //根据触摸点计算X与Y位置
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02F;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02F;
 
                    // moveObj.transform.position = Camera.main.transform.position;
                    // moveObj.transform.rotation = Camera.main.transform.rotation;
                }
            }
 
            //判断触摸数量为多点触摸
            if (Input.touchCount > 1)
            {
                //前两只手指触摸类型都为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved||Input.GetTouch(1).phase == TouchPhase.Moved)
    	        {
                    //计算出当前两点触摸点的位置
                    Vector3 tempPosition1 = Input.GetTouch(0).position;
                    Vector3 tempPosition2 = Input.GetTouch(1).position;
                    //函数返回真为放大，返回假为缩小
                    if (IsEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                    {
                        //放大系数超过10以后不允许继续放大
                        //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                        if (distance > 10F)
                        {
                            distance -=1F;
                        }
                    }
                    else
                    {
                        //缩小系数返回100后不允许继续缩小
                        //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                        if (distance < 100F)
                        {
                            distance += 1F;
                        }
                    }
                    //备份上一次触摸点的位置，用于对比
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                }
            }
        }
 
        //函数返回真为放大，返回假为缩小
        bool IsEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
        {
	        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
            float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
            float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
            if(leng1<leng2)
            {
    	         //放大手势
                 return true;
            }else
            {
    	        //缩小手势
                return false;
            }
        }
 
        //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
        void LateUpdate()
        {
            //target为我们绑定的物体变量，缩放旋转的参照物
            if (target)
            {
 
                //重置摄像机的位置
                ClampAngle(y, yMinLimit, yMaxLimit);
                Quaternion rotation = Quaternion.Euler(y, x, 0);
 
                tmp.Set(0.0F,0.0F,(-1)*distance);
                Vector3 position = rotation * tmp + target.transform.position;
 
                transform.rotation = rotation;
                transform.position = position;
 
                // moveObj.transform.position = Camera.main.transform.position;
                // moveObj.transform.rotation = Camera.main.transform.rotation;
            }
        }
 
        //物体翻滚的控制
        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
 
            return Mathf.Clamp(angle, min, max);
        }
 
 
	}//Class_end
}