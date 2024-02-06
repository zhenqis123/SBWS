using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatePoints : MonoBehaviour
{
    
    //private static int targetFrameRate = 10; // Frame rate control, point cloud is updated every frame
    private int resolutionX; 
    private int resolutionY;
    private static int sampleCount; // Amount of samples for each frame
    private float randomAngle; // A random starting angle for when the lidar starts scanning, generated each start-up

    public Vector3[] points; // The points sampled from this camera during each frame
    public int pointCount; // The actual amount of points sampled. Less than 10k as nearest neighbour interpolation is used

    private Camera cam; // The depth camera this script is attached to.
    private Texture2D tex2d; // The texture that stores depth buffer that is extracted from the GPU
    private GameObject controller;
    // Start is called before the first frame upda
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        controller = GameObject.Find("Runtime Parameters Controller");
        sampleCount = controller.GetComponent<FrameRateController>().samplerate;
        resolutionX = controller.GetComponent<FrameRateController>().resolutionX;
        resolutionY = controller.GetComponent<FrameRateController>().resolutionY;
        //Debug.Log(cam.name);

        //Application.targetFrameRate = targetFrameRate;

        // Screen.SetResolution(resWidth, resHeight, false);
        tex2d = new Texture2D(resolutionX, resolutionY, TextureFormat.RGBAFloat, false);
        //data = new float[resWidth * resHeight * 4];

        points = new Vector3[sampleCount];
        randomAngle = Random.Range(0f, 2f * Mathf.PI) * 3.825f; // multiplied by 3.825f in advance for efficiency
    }

    // Update is called once per frame
    void Update()
    {
        // Get depth texture
        RenderTexture tempRt = new RenderTexture(resolutionX, resolutionY, 0);

        tempRt.format = RenderTextureFormat.ARGBFloat;

        cam.targetTexture = tempRt;
        cam.Render();
        RenderTexture.active = tempRt;
        tex2d.ReadPixels(new Rect(0, 0, resolutionX, resolutionY), 0, 0);

        tex2d.Apply();

        // data is the raw depth data which has been converted into linear range between 0-1
        // representing depth as a fraction between camera's near and far clipping plane
        // UPDATE: obsolete as GetRawTextureData uses too much resources, use GetPixel instead.
        //var data = tex2d.GetRawTextureData<float>();

        //debug info output to console
        //Debug.Log("Test pixel 1 " + Camera.main.farClipPlane * data[0]);
        //Debug.Log("Test pixel 6 " + data.Length);


        //rose pedal equation sub-sampling, 10k samples every frame/0.1s
        pointCount = 0;
        for (int n = sampleCount * Time.frameCount; n < sampleCount * Time.frameCount + sampleCount; n++)
        {
            float r = 418f * Mathf.Cos(randomAngle + 0.0065025f * n);
            int x = Mathf.RoundToInt(r * Mathf.Cos(0.0017f * n)) + 1046;
            int y = Mathf.RoundToInt(r * Mathf.Sin(0.0017f * n)) + 846;

            //calculate pixel world locations, store in Vector3[], sub-sampling using rose pedal equation
            points[pointCount] = cam.ScreenToWorldPoint(
                //new Vector3(x, y, cam.farClipPlane * data[(y * resolutionX + x) * 4]));
                new Vector3(x, y, cam.farClipPlane * tex2d.GetPixel(x, y).r));
            pointCount++;
        }


        // Let camera display in game view
        RenderTexture.active = null;
        cam.targetTexture = null;
        Destroy(tempRt);

    }

}
