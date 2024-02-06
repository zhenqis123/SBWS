using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class GeneratePointCloud : MonoBehaviour
{
    //private static int targetFrameRate = 10; // Frame rate control, point cloud is updated every frame
    private static int sampleCount; // Amount of samples for each frame
    private static int nodeCount = 4; // Amount of nodes
    public bool rgb_o = true; // Output rgb images or not
    private Vector3[] points; // All points sampled by the 3 depth cameras
    private int pointCount; // Amount of points in total
    private Color[] colours; // The corresponding colours of the points
    // saving related
    private string saveas = "E:\\pointclouddata";
    private int file_count = 0;

    private ColourPoints colourPoints1; // Reference to the main depth camera's script
    private ColourPoints colourPoints2;
    private ColourPoints colourPoints3;
    private ColourPoints colourPoints4;
    private static ParticleSystem pointCloud; // The particle system that displays the point cloud
    private ParticleSystem.Particle[] particles;

    private GameObject[] peds;
    private GameObject controller;
    private Animator anim;
    private int resolutionX;
    private int resolutionY;
    private Camera camera_1;
    private Camera camera_2;
    private Camera camera_3;
    private Camera camera_4;
    private Camera[] cameras = new Camera[4];
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Runtime Parameters Controller");
        sampleCount = controller.GetComponent<FrameRateController>().samplerate;
        resolutionX = controller.GetComponent<FrameRateController>().resolutionX;
        resolutionY = controller.GetComponent<FrameRateController>().resolutionY;
        //Application.targetFrameRate = targetFrameRate;
        pointCloud = GameObject.Find("Point Cloud Particle System").GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[sampleCount * 3 * nodeCount];
        points = new Vector3[sampleCount * 3 * nodeCount];
        pointCount = 0;
        colours = new Color[sampleCount * 3 * nodeCount];

        colourPoints1 = GameObject.Find("Node 1").transform.Find("Colour Camera").GetComponent<ColourPoints>();
        colourPoints2 = GameObject.Find("Node 2").transform.Find("Colour Camera").GetComponent<ColourPoints>();
        colourPoints3 = GameObject.Find("Node 3").transform.Find("Colour Camera").GetComponent<ColourPoints>();
        colourPoints4 = GameObject.Find("Node 4").transform.Find("Colour Camera").GetComponent<ColourPoints>();

        camera_1 = GameObject.Find("Node 1").transform.Find("Colour Camera").GetComponent<Camera>();
        camera_2 = GameObject.Find("Node 2").transform.Find("Colour Camera").GetComponent<Camera>();
        camera_3 = GameObject.Find("Node 3").transform.Find("Colour Camera").GetComponent<Camera>();
        camera_4 = GameObject.Find("Node 4").transform.Find("Colour Camera").GetComponent<Camera>();
        cameras[0] = camera_1;
        cameras[1] = camera_2;
        cameras[2] = camera_3;
        cameras[3] = camera_4;
        peds = GameObject.FindGameObjectsWithTag("Peds");
        if (!Directory.Exists(Path.Combine(saveas, "points")))//如果路径不存在

        {
            Directory.CreateDirectory(Path.Combine(saveas, "points"));//创建一个路径的文件夹
        }
        if (!Directory.Exists(Path.Combine(saveas, "joints")))//如果路径不存在
        {
            Directory.CreateDirectory(Path.Combine(saveas, "joints"));//创建一个路径的文件夹
        }
        if (!Directory.Exists(Path.Combine(saveas, "images")))//如果路径不存在
        {
            Directory.CreateDirectory(Path.Combine(saveas, "images"));//创建一个路径的文件夹
        }
        for (int i = 0; i<10; i++)
        {
            if (!Directory.Exists(Path.Combine(saveas, "joints",i.ToString())))//如果路径不存在
            {
                Directory.CreateDirectory(Path.Combine(saveas, "joints", i.ToString()));//创建一个路径的文件夹
            }
        }
        for(int i = 1; i<5; i++)
        {
            if (!Directory.Exists(Path.Combine(saveas, "images", i.ToString())))//如果路径不存在
            {
                Directory.CreateDirectory(Path.Combine(saveas, "images", i.ToString()));//创建一个路径的文件夹
            }
        }
        if (rgb_o)
        {
            // disabled some scrpts
            GameObject[] targets;
            targets = GameObject.FindGameObjectsWithTag("MainCamera");
            delete_script(targets);
            targets = GameObject.FindGameObjectsWithTag("Left");
            delete_script(targets);
            targets = GameObject.FindGameObjectsWithTag("Colour");
            foreach (GameObject tar in targets)
            {
                tar.GetComponent<ColourPoints>().enabled = false;
            }
        }

    }

    void delete_script(GameObject[] targets)
    {
        foreach(GameObject tar in targets)
        {
            tar.GetComponent<RenderDepth>().enabled = false;
            tar.GetComponent<LocatePoints>().enabled = false; 
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!rgb_o)
        {
            Array.Clear(points, 0, sampleCount * 3 * nodeCount);
            Array.Clear(colours, 0, sampleCount * 3 * nodeCount);
            string point_file = Path.Combine(saveas, "points", "point_" + file_count.ToString() + ".txt"); //saving in 'points' directory
            FileStream datafile = new FileStream(point_file, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(datafile);
            pointCount = 0;
            grabPoints(colourPoints1.points, colourPoints1.pointCount, colourPoints1.colours);
            grabPoints(colourPoints2.points, colourPoints2.pointCount, colourPoints2.colours);
            grabPoints(colourPoints3.points, colourPoints3.pointCount, colourPoints3.colours);
            grabPoints(colourPoints4.points, colourPoints4.pointCount, colourPoints4.colours);

            // Display the points as a points cloud
            updatePointCloud(points, colours, pointCount, writer);
            // write the joints information
            writer.Close();
            OutPutJoints();
        }
        else
        {
            //output the rgb images
            for (int i = 0; i < 4; i++)
            {
                Camera cam = cameras[i];
                string rgb_path = Path.Combine(saveas, "images", (i + 1).ToString(), "camera_" + file_count.ToString() + ".jpg");
                CaptureCamera(cam, new Rect(0, 0, resolutionX, resolutionY), rgb_path);
            }
            if (file_count == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    string camera_project = Path.Combine(saveas, "images", "camera_" + (i + 1).ToString() + ".txt");
                    FileStream dataFile = new FileStream(camera_project, FileMode.Create, FileAccess.Write);
                    StreamWriter writer_matrix = new StreamWriter(dataFile);
                    Camera cam = cameras[i];
                    Matrix4x4 ProjectionMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;
                    string matrixinfo = string.Format("{0}", ProjectionMatrix);
                    writer_matrix.Write(matrixinfo);
                    writer_matrix.Flush();
                    writer_matrix.Close();
                }
            }
        }
        // output the rgb
        //if (rgb_o)
        //{
            

        //}
        file_count++;

    }

    // Retrieve the world positions of the points sampled by the depth cameras
    void grabPoints(Vector3[] inputPoints, int inputCount, Color[] inputColours)
    {
        Array.Copy(inputPoints, 0, points, pointCount, inputCount);
        Array.Copy(inputColours, 0, colours, pointCount, inputCount);
        pointCount += inputCount;

        //for (int i = 0; i < inputCount; i++)
        //{
        //    points[pointCount + i] = inputPoints[i];
        //    colours[pointCount + i] = inputColours[i];
        //}
        //pointCount += inputCount;


        // Debug.Log("Test count   " + points.Length);
        // Debug.Log("Test count   " + pointCount);
        // Debug.Log("Test count   " + inputPoints.Length);
        // Debug.Log("Test count   " + inputCount);
    }

    // draw points within the particle system to display the points cloud
    void updatePointCloud(Vector3[] points, Color[] colours, int pointCount, StreamWriter writer)
    {
        string write_data;
        if (pointCloud == null)
        {
            Debug.Log("FAIL");
            return;
        }

        for (int i = 0; i < pointCount; ++i)
        {
            particles[i].position = points[i];
            particles[i].startSize = 0.015f;
            particles[i].startColor = colours[i];
            //particles[i].startColor = Color.red;
            write_data = string.Format("{0} {1} {2} {3} {4} {5}\n", points[i][0], points[i][1], points[i][2], colours[i][0], colours[i][1], colours[i][2]);
            writer.Write(write_data);
            writer.Flush();
        }

        pointCloud.SetParticles(particles);
        pointCloud.Pause();

        // debug info output to console
        //Debug.Log("Test count   " + pointCount);
        //Debug.Log("Test point 1 " + points[1]);
        //Debug.Log("Test point 2 " + points[pointCount-1]);
    }
    // output the pointcloud data into files
    void OutPutJoints()
    {
        string write_data;
        Transform keypoint;
        foreach (GameObject ped in peds)
        {
            int ID_num = ped.GetComponent<NPCBehaviourController>().Pedes_ID;
            string joints_file = Path.Combine(saveas , "joints", ID_num.ToString(), "joints_" + file_count.ToString() + ".txt"); //saving in 'points' directory
            FileStream datafile_j = new FileStream(joints_file, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(datafile_j);
            // get the keypoints information
            anim = ped.GetComponent<Animator>();
            keypoint = anim.GetBoneTransform(HumanBodyBones.Neck);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            foreach(Transform child in ped.GetComponentsInChildren<Transform>(true))
            {
                /*if (child.CompareTag("TopHead"))
                {
                    keypoint = child;
                    break;
                }*/
                if (child.CompareTag("Nose"))
                {
                    keypoint = child;
                    break;
                }
            }
            //keypoint = anim.GetBoneTransform(HumanBodyBones.Head);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.Hips);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftHand);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightHand);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            keypoint = anim.GetBoneTransform(HumanBodyBones.RightFoot);
            write_data = string.Format("{0} {1} {2}\n", keypoint.position.x, keypoint.position.y, keypoint.position.z);
            writer.Write(write_data);
            writer.Flush();
            writer.Close();

        }

    }
    void CaptureCamera(Camera camera, Rect rect, string diractory)

    {

        // 创建一个RenderTexture对象  

        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, -1);

        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();

        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  

        //camera2.targetTexture = rt;

        // camera2.Render();

        //ps: -------------------------------------------------------------------  

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        // camera2.targetTexture = null;

        RenderTexture.active = null; // JC: added to avoid errors  

        GameObject.Destroy(rt);

        // 最后将这些纹理数据，成一个png图片文件  

        byte[] bytes = screenShot.EncodeToPNG();
        //string filename = diractory + "/Screenshot.png";

        System.IO.File.WriteAllBytes(diractory, bytes);
        //Debug.Log(string.Format("截屏了一张照片: {0}: camera {1}", diractory, Camera_num));
        // do not return the value
        //return screenShot;
        Destroy(screenShot);//release
    }
}
