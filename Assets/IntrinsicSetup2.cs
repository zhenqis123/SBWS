using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntrinsicSetup2 : MonoBehaviour
{

    private static float[,] intrinsics = { { 1208.95178f,       0.0f,               1012.43334f     }, 
                                           { 0.0f,              1209.42656f,        762.148531f     }, 
                                           { 0.0f,              0.0f,               1.0f            } };
    private static int resolutionX = 2048;
    private static int resolutionY = 1536;

    private float f = 35.0f; // f can be arbitrary, as long as sensor_size is resized to to make ax,ay consistient

    Camera cam;

    // Use this for initialization
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        changeCameraParam();
    }

    public void changeCameraParam()
    {
        float ax, ay, sizeX, sizeY;
        float x0, y0, shiftX, shiftY;
        int width, height;

        ax = intrinsics[0, 0];
        ay = intrinsics[1, 1];
        x0 = intrinsics[0, 2];
        y0 = intrinsics[1, 2];

        width = resolutionX;
        height = resolutionY;

        sizeX = f * width / ax;
        sizeY = f * height / ay;

        //PlayerSettings.defaultScreenWidth = width;
        //PlayerSettings.defaultScreenHeight = height;

        shiftX = -(x0 - width / 2.0f) / width;
        shiftY = (y0 - height / 2.0f) / height;

        cam.sensorSize = new Vector2(sizeX, sizeY);     // in mm, mx = 1000/x, my = 1000/y
        cam.focalLength = f;                            // in mm, ax = f * mx, ay = f * my
        cam.lensShift = new Vector2(shiftX, shiftY);    // W/2,H/w for (0,0), 1.0 shift in full W/H in image plane

        //Debug.Log("Updated " + cam.name + " intrinsics");

    }
}