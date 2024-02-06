using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPoints : MonoBehaviour
{

    //private static int targetFrameRate = 10; // Frame rate control, point cloud is updated every frame
    private int resolutionX;
    private int resolutionY;
    private static int sampleCount; // amount of samples for each frame

    public Vector3[] points; // All points sampled by the 3 depth cameras
    public int pointCount; // Amount of points in total
    public Color[] colours; // The corresponding colours of the points

    private Camera cam; // The colour camera this script is attached to
    private LocatePoints locatePointsMain; // Reference to the main depth camera's script
    private LocatePoints locatePointsLeft; // Reference to the left depth camera's script
    private LocatePoints locatePointsRight; // Reference to the right depth camera's script
    //private static ParticleSystem pointCloud; // The particle system that displays the point cloud
    private Texture2D tex2d; // The texture that stores the colour
    private Vector3Int tempScreenPos; // Temp variable to store screen space position for each point
    private GameObject controller;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        //Debug.Log(cam.name);
        controller = GameObject.Find("Runtime Parameters Controller");
        sampleCount = controller.GetComponent<FrameRateController>().samplerate;
        resolutionX = controller.GetComponent<FrameRateController>().resolutionX;
        resolutionY = controller.GetComponent<FrameRateController>().resolutionY;
        locatePointsMain = transform.parent.Find("Depth Camera (main)").GetComponent<LocatePoints>();
        locatePointsLeft = transform.parent.Find("Depth Camera (left)").GetComponent<LocatePoints>();
        locatePointsRight = transform.parent.Find("Depth Camera (right)").GetComponent<LocatePoints>();

        // Screen.SetResolution(resWidth, resHeight, false);
        tex2d = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, false);
        points = new Vector3[sampleCount * 3];
        pointCount = 0;
        colours = new Color[sampleCount * 3];
    }

    // Update is called once per frame
    void Update()
    {
        pointCount = 0;

        // First collect the points that's been sampled by the depth cameras
        grabPoints(locatePointsMain.points, locatePointsMain.pointCount);
        grabPoints(locatePointsLeft.points, locatePointsLeft.pointCount);
        grabPoints(locatePointsRight.points, locatePointsRight.pointCount);

        // render a coloured texture
        RenderTexture tempRt = new RenderTexture(resolutionX, resolutionY, 0);

        tempRt.format = RenderTextureFormat.ARGB32;

        cam.targetTexture = tempRt;
        cam.Render();
        RenderTexture.active = tempRt;
        tex2d.ReadPixels(new Rect(0, 0, resolutionX, resolutionY), 0, 0);

        tex2d.Apply();

        // Project the points sampled from the depth cameras into the colour camera's screen space,
        // and determine each point's colour.
        for (int i = 0; i < pointCount; i++)
        {
            tempScreenPos = Vector3Int.RoundToInt(cam.WorldToScreenPoint(points[i]));

            // If a point is out of the scope of the colour camera, it is coloured with a placeholder colour blue.
            if (tempScreenPos.x < 0 || tempScreenPos.x >= resolutionX || 
                tempScreenPos.y < 0 || tempScreenPos.y >= resolutionY)
            {
                colours[i] = Color.blue;
                continue;
            }

            colours[i] = tex2d.GetPixel(tempScreenPos.x, tempScreenPos.y);
        }

        RenderTexture.active = null;
        cam.targetTexture = null;
        Destroy(tempRt);

        //Debug.Log("Test count1   " + points.Length);
        //Debug.Log("Test count2   " + pointCount);
        //Debug.Log("Test count3   " + colours.Length);

    }

    // Retrieve the world positions of the points sampled by the depth cameras
    void grabPoints(Vector3[] inputPoints, int inputCount)
    {
        
        for (int i = 0; i < inputCount; i++)
        {
            points[pointCount + i] = inputPoints[i];
        }
        pointCount += inputCount;
        
       // Debug.Log("Test count   " + points.Length);
       // Debug.Log("Test count   " + pointCount);
       // Debug.Log("Test count   " + inputPoints.Length);
       // Debug.Log("Test count   " + inputCount);
    }

}
