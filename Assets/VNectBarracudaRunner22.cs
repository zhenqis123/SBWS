using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using System.IO;
/// <summary>
/// Define Joint points
/// </summary>
public class VNectBarracudaRunner22 : MonoBehaviour
{
    /// <summary>
    /// Neural network model
    /// </summary>

    // limit character's movable area
    /*public float xLowerLimit = 3.0f;
    public float xUpperLimit = 30.0f;
    public float zLowerLimit = -14.0f;
    public float zUpperLimit = -3.0f;*/
    public float xLowerLimit ;
    public float xUpperLimit ;
    public float zLowerLimit ;
    public float zUpperLimit ;

    public float xEndingOffset;
    public float zEndingOffset;
   

    //public string jointPath = "E:/pointclouddata/joints/0/";
    //public string jointPath = "E:/pointclouddata/CMU/01";
    public string jointPath = "E:/pointclouddata/CMU";
    //public string filename = "01_01_poses.txt";
    //fpublic int seqLength = 3000;
    public int seqCurrent = 0;
    public int seqLength = 0;

    public bool Verbose = true;

    public VNectModel22 VNectModel;

    public float[,] result;
    public float[,] fullSeq;
    private string[] allFiles;
    private int chosenFileIndex;
    private int totalFileCount;
    public string currentFile;



    /// <summary>
    /// Coordinates of joint points
    /// </summary>
    private VNectModel22.JointPoint[] jointPoints;
    
    /// <summary>
    /// Number of joint points
    /// </summary>
    private const int JointNum = 22;

    
    /// <summary>
    /// Number of joints in 2D image
    /// </summary>
    private int JointNum_Squared = JointNum * 2;
    
    /// <summary>
    /// Number of joints in 3D model
    /// </summary>
    private int JointNum_Cube = JointNum * 3;

    /// <summary>
    /// For Kalman filter parameter Q
    /// </summary>
    public float KalmanParamQ;

    /// <summary>
    /// For Kalman filter parameter R
    /// </summary>
    public float KalmanParamR;

    /// <summary>
    /// Lock to update VNectModel
    /// </summary>
    private bool Lock = true;

    /// <summary>
    /// Use low pass filter flag
    /// </summary>
    public bool UseLowPassFilter;

    /// <summary>
    /// For low pass filter
    /// </summary>
    public float LowPassParam;

    public Text Msg;
    public float WaitTimeModelLoad = 10f;
    private float Countdown = 0;
    public Texture2D InitImg;

    private void Start()
    {
        // Initialize 
        xLowerLimit = GetComponentInParent<CharacterCollisionDetection>().xLowerLimit;
        xUpperLimit = GetComponentInParent<CharacterCollisionDetection>().xUpperLimit;
        zLowerLimit = GetComponentInParent<CharacterCollisionDetection>().zLowerLimit;
        zUpperLimit = GetComponentInParent<CharacterCollisionDetection>().zUpperLimit;
        VNectModel.xRandomOffset = Random.Range(xLowerLimit, xUpperLimit);
        VNectModel.zRandomOffset = Random.Range(zLowerLimit, zUpperLimit);
        

        // Disabel sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Init model
        result = new float[22, 3];

        loadFileNames();

        //StartCoroutine("WaitLoad");
        WaitLoad();
        
    }

    private void loadFileNames()
    {
        allFiles = Directory.GetFiles(jointPath, "*.txt", System.IO.SearchOption.AllDirectories);
        print(allFiles[0]);
        print(allFiles[allFiles.Length-1]);
        print(allFiles.Length);
        totalFileCount = allFiles.Length;
    }

    private void Update()
    {
        if (!Lock)
        {
            readJoints();

            UpdateVNectModel();
        }
    }
    private bool readJoints()
    {
        result = new float[22, 3];

        if (seqCurrent == 0)
        {
            //随机放角色
            //VNectModel.xRandomOffset = Random.Range(xLowerLimit, xUpperLimit);
            //VNectModel.zRandomOffset = Random.Range(zLowerLimit, zUpperLimit);

            //手动加的负号
            //VNectModel.zRandomOffset = -VNectModel.zRandomOffset;
            //print(VNectModel.xRandomOffset);

            chosenFileIndex = Random.Range(0, totalFileCount);
            //string fullfilename = filename + seqCurrent.ToString() + ".txt";
            //fullfilename = Path.Combine(jointPath, filename);
            string fullfilename = allFiles[chosenFileIndex];
            if (!File.Exists(fullfilename))
            {

            }

            string input = File.ReadAllText(fullfilename);

            seqLength = input.Split('\n').Length / 52;
            fullSeq = new float[input.Split('\n').Length, 3];

            int i = 0, j = 0;

            foreach (var row in input.Split('\n'))
            {
                //print("row:  ");
                //print(row);
                if (row == "")
                {
                    break;
                }

                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    //print("col:    ");
                    //print(col);
                    //result[i, j] = float.Parse(col.Trim());
                    fullSeq[i, j] = float.Parse(col);
                    j++;
                }
                i++;
            }

            

        }
        

        //copy section of full sequence to results
        //Array.Copy(fullSeq,seqCurrent*52, result,0, 22);
        //result = fullSeq[(0..22),(0..3)];
        for (int i = 0; i < JointNum; i++)
        {
            
            for (int j = 0; j < 3; j++)
            {
                //print("col:    ");
                //print(col);
                //result[i, j] = float.Parse(col.Trim());
                result[i, j] = fullSeq[seqCurrent*52+i,j];
                
            }
            
        }

        if (seqCurrent == 0)
        {
            VNectModel.xRandomOffset = xEndingOffset - result[PositionIndex22.hip.Int(),0];
            VNectModel.zRandomOffset = zEndingOffset - result[PositionIndex22.hip.Int(), 2];
            /*Debug.Log("xRandom");
            Debug.Log(VNectModel.xRandomOffset);
            Debug.Log("hip");
            Debug.Log(VNectModel.jointPoints[PositionIndex22.hip.Int()].Transform.position.x);*/
            if (name == "Animation Driver (9)")
            {
                print("changing seq now!!!");
            }
        }
        /*if (seqCurrent == 1)
        {
            print(VNectModel.xRandomOffset);

        }*/
      

        seqCurrent++;
        // print(result[0, 0]);
        // print(result[0, 1]);
        // print(result[0, 2]);
        //print(seqCurrent);

        if (seqCurrent == seqLength)
        {
            //find new file randomly

            seqCurrent = 0;
            seqLength = 0;

            xEndingOffset = VNectModel.jointPoints[PositionIndex22.hip.Int()].Transform.position.x;
            zEndingOffset = VNectModel.jointPoints[PositionIndex22.hip.Int()].Transform.position.z;

            //print(VNectModel.xRandomOffset);
        }

        if (name == "Animation Driver (9)")
        {
            print(VNectModel.jointPoints[PositionIndex22.hip.Int()].Transform.position.x);

        }
        
        return true;
    }

    private void WaitLoad()
    {
        // joints
        //print("test");


        // Init VNect model
        jointPoints = VNectModel.Init();
        // print("test");
        PredictPose();

        //yield return new WaitForSeconds(WaitTimeModelLoad);

        // Init VideoCapture

        Lock = false;
        //Msg.gameObject.SetActive(false);
    }

    private const string inputName_1 = "input.1";
    private const string inputName_2 = "input.4";
    private const string inputName_3 = "input.7";
    /*
    private const string inputName_1 = "0";
    private const string inputName_2 = "1";
    private const string inputName_3 = "2";
    */

    private void UpdateVNectModel()
    {

        PredictPose();
        //StartCoroutine(ExecuteModelAsync());
    }

    /// <summary>
    /// Tensor has input image
    /// </summary>
    /// <returns></returns>
    

    /// <summary>
    /// Predict positions of each of joints based on network
    /// </summary>
    private void PredictPose()
    {
        for (var j = 0; j < JointNum; j++)
        {
            if (j == 6 || j == 9)
            {
                continue;
            }

            jointPoints[j].Now3D.x = result[j, 0];
            jointPoints[j].Now3D.y = result[j, 1];
            jointPoints[j].Now3D.z = result[j, 2];

            //VNectModel.originalData[j, 0] = result[j, 0];
            //VNectModel.originalData[j, 1] = result[j, 1];
            //VNectModel.originalData[j, 2] = result[j, 2];
        }

        /*// Calculate hip location
        var lc = (jointPoints[PositionIndex.rThighBend.Int()].Now3D + jointPoints[PositionIndex.lThighBend.Int()].Now3D) / 2f;
        jointPoints[PositionIndex.hip.Int()].Now3D = (jointPoints[PositionIndex.abdomenUpper.Int()].Now3D + lc) / 2f;*/

        // Calculate neck location
        jointPoints[PositionIndex22.neck.Int()].Now3D = (jointPoints[PositionIndex22.lCollar.Int()].Now3D + jointPoints[PositionIndex22.rCollar.Int()].Now3D) / 2f;

        // Calculate head location
        /*var cEar = (jointPoints[PositionIndex22.rEar.Int()].Now3D + jointPoints[PositionIndex22.lEar.Int()].Now3D) / 2f;
        var hv = cEar - jointPoints[PositionIndex22.neck.Int()].Now3D;
        var nhv = Vector3.Normalize(hv);
        var nv = jointPoints[PositionIndex22.Nose.Int()].Now3D - jointPoints[PositionIndex22.neck.Int()].Now3D;
        jointPoints[PositionIndex22.head.Int()].Now3D = jointPoints[PositionIndex22.neck.Int()].Now3D + nhv * Vector3.Dot(nhv, nv);*/

        //head location is extrapolated from spine
        //var neckSeg = jointPoints[PositionIndex22.neck.Int()].Now3D - jointPoints[PositionIndex22.spine3.Int()].Now3D;
        var neckSeg = new Vector3(jointPoints[PositionIndex22.neck.Int()].Now3D.x - result[9, 0], jointPoints[PositionIndex22.neck.Int()].Now3D.y - result[9, 1], jointPoints[PositionIndex22.neck.Int()].Now3D.z - result[9, 2]);
        neckSeg = neckSeg.normalized * VNectModel.neckLength * 2.2f;
        jointPoints[PositionIndex22.head.Int()].Now3D = jointPoints[PositionIndex22.neck.Int()].Now3D + neckSeg;

        /*// Calculate spine location
        jointPoints[PositionIndex22.spine.Int()].Now3D = jointPoints[PositionIndex22.abdomenUpper.Int()].Now3D;*/

        //foot end location
        //var projectionL = Vector3.ProjectOnPlane(VNectModel.forward, Vector3.up);
        //projectionL = projectionL.normalized * VNectModel.footLength;
        //var footPosL = jointPoints[PositionIndex15.lFoot.Int()].Now3D;
        //footPosL.y = footPosL.y - VNectModel.ankleHeight;
        jointPoints[PositionIndex22.lToe.Int()].Now3D.y = jointPoints[PositionIndex22.lToe.Int()].Now3D.y - 0.09f;

        //var projectionR = Vector3.ProjectOnPlane(VNectModel.forward, Vector3.up);
        //projectionR = projectionR.normalized * VNectModel.footLength;
        //var footPosR = jointPoints[PositionIndex15.rFoot.Int()].Now3D;
        //footPosR.y = footPosR.y - VNectModel.ankleHeight;
        jointPoints[PositionIndex22.rToe.Int()].Now3D.y = jointPoints[PositionIndex22.rToe.Int()].Now3D.y - 0.09f;

        // Kalman filter
        foreach (var jp in jointPoints)
        {
            KalmanUpdate(jp);
        }

        // Low pass filter
        if (UseLowPassFilter)
        {
            foreach (var jp in jointPoints)
            {
                jp.PrevPos3D[0] = jp.Pos3D /*+ new Vector3(VNectModel.xRandomOffset,0,VNectModel.zRandomOffset)*/;
                for (var i = 1; i < jp.PrevPos3D.Length; i++)
                {
                    jp.PrevPos3D[i] = jp.PrevPos3D[i] * LowPassParam + jp.PrevPos3D[i - 1] * (1f - LowPassParam);
                }
                jp.Pos3D = jp.PrevPos3D[jp.PrevPos3D.Length - 1];
            }
        }
    }

    /// <summary>
    /// Kalman filter
    /// </summary>
    /// <param name="measurement">joint points</param>
    void KalmanUpdate(VNectModel22.JointPoint measurement)
    {
        measurementUpdate(measurement);
        measurement.Pos3D.x = measurement.X.x + (measurement.Now3D.x - measurement.X.x) * measurement.K.x;
        measurement.Pos3D.y = measurement.X.y + (measurement.Now3D.y - measurement.X.y) * measurement.K.y;
        measurement.Pos3D.z = measurement.X.z + (measurement.Now3D.z - measurement.X.z) * measurement.K.z;
        measurement.X = measurement.Pos3D;
    }

	void measurementUpdate(VNectModel22.JointPoint measurement)
    {
        measurement.K.x = (measurement.P.x + KalmanParamQ) / (measurement.P.x + KalmanParamQ + KalmanParamR);
        measurement.K.y = (measurement.P.y + KalmanParamQ) / (measurement.P.y + KalmanParamQ + KalmanParamR);
        measurement.K.z = (measurement.P.z + KalmanParamQ) / (measurement.P.z + KalmanParamQ + KalmanParamR);
        measurement.P.x = KalmanParamR * (measurement.P.x + KalmanParamQ) / (KalmanParamR + measurement.P.x + KalmanParamQ);
        measurement.P.y = KalmanParamR * (measurement.P.y + KalmanParamQ) / (KalmanParamR + measurement.P.y + KalmanParamQ);
        measurement.P.z = KalmanParamR * (measurement.P.z + KalmanParamQ) / (KalmanParamR + measurement.P.z + KalmanParamQ);
    }
}
