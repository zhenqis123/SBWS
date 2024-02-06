using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using System.IO;
//using Unity.Barracuda;

/// <summary>
/// Define Joint points
/// </summary>
public class VNectBarracudaRunner17 : MonoBehaviour
{
    /// <summary>
    /// Neural network model
    /// </summary>


    public string jointPath = "E:/pointclouddata/joints/0";
    public string filename = "joints_";
    public int seqLength = 3000;
    public int seqCurrent = 0;

    public bool Verbose = true;

    public VNectModel17 VNectModel;

    public float[,] result;

    /// <summary>
    /// Coordinates of joint points
    /// </summary>
    private VNectModel17.JointPoint[] jointPoints;
    
    /// <summary>
    /// Number of joint points
    /// </summary>
    private const int JointNum = 15;

    
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
    

        // Disabel sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Init model


        StartCoroutine("WaitLoad");

    }

    private void Update()
    {
        if (!Lock)
        {
            //read joint
            readJoints();

            UpdateVNectModel();
        }
    }

    private bool readJoints()
    {
        result = new float[seqLength, 3];

        if (seqCurrent == seqLength)
        {
            seqCurrent = 0;

        }

        string fullfilename = Path.Combine(jointPath, filename, seqCurrent.ToString());
        if (!File.Exists(fullfilename))
        {
            
        }

        string input = File.ReadAllText(fullfilename);

        int i = 0, j = 0;
        
        foreach (var row in input.Split('\n'))
        {
            j = 0;
            foreach (var col in row.Trim().Split(' '))
            {
                result[i, j] = int.Parse(col.Trim());
                j++;
            }
            i++;
        }

        seqCurrent++;
        // print(result[0, 0]);
        // print(result[0, 1]);
        // print(result[0, 2]);
        print(seqCurrent);
        return true;
    }

    private IEnumerator WaitLoad()
    {
        // joints



        

        // Init VNect model
        jointPoints = VNectModel.Init();

        PredictPose();

        yield return new WaitForSeconds(WaitTimeModelLoad);

        // Init VideoCapture
        
        Lock = false;
        Msg.gameObject.SetActive(false);
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


    /*private IEnumerator ExecuteModelAsync()
    {
        // Create input and Execute model
        yield return _worker.StartManualSchedule(inputs);

        // Get outputs
        for (var i = 2; i < _model.outputs.Count; i++)
        {
            b_outputs[i] = _worker.PeekOutput(_model.outputs[i]);
        }

        // Get data from outputs
        offset3D = b_outputs[2].data.Download(b_outputs[2].shape);
        heatMap3D = b_outputs[3].data.Download(b_outputs[3].shape);

        // Release outputs
        for (var i = 2; i < b_outputs.Length; i++)
        {
            b_outputs[i].Dispose();
        }

        PredictPose();
    }*/

    /// <summary>
    /// Predict positions of each of joints based on network
    /// </summary>
    private void PredictPose()
    {
        for (var j = 0; j < JointNum; j++)
        {
            
           
            jointPoints[j].Now3D.x = result[j, 0];
            jointPoints[j].Now3D.y = result[j, 1];
            jointPoints[j].Now3D.z = result[j, 2];
        }

        // Calculate neck location
        jointPoints[PositionIndex15.neck.Int()].Now3D = (jointPoints[PositionIndex15.rShldrBend.Int()].Now3D + jointPoints[PositionIndex15.lShldrBend.Int()].Now3D) / 2f;

        // Calculate hip location
        var lc = (jointPoints[PositionIndex15.rThighBend.Int()].Now3D + jointPoints[PositionIndex15.lThighBend.Int()].Now3D) / 2f;
        //print(jointPoints[PositionIndex15.lThighBend.Int()].Now3D);
        //print(jointPoints[PositionIndex15.rThighBend.Int()].Now3D);
        //print(lc);
        //print(jointPoints[PositionIndex15.neck.Int()].Now3D);
        //jointPoints[PositionIndex15.hip.Int()].Now3D = (jointPoints[PositionIndex15.neck.Int()].Now3D + lc) * 0.5f;
        jointPoints[PositionIndex15.hip.Int()].Now3D = lc + (jointPoints[PositionIndex15.neck.Int()].Now3D - lc) * 0.2f;
        //print(jointPoints[PositionIndex15.hip.Int()].Now3D);

        // Calculate head location

        /*var cEar = (jointPoints[PositionIndex17.rEar.Int()].Now3D + jointPoints[PositionIndex17.lEar.Int()].Now3D) / 2f;
        var hv = cEar - jointPoints[PositionIndex17.neck.Int()].Now3D;
        var nhv = Vector3.Normalize(hv);
        var nv = jointPoints[PositionIndex17.Nose.Int()].Now3D - jointPoints[PositionIndex17.neck.Int()].Now3D;
        jointPoints[PositionIndex17.head.Int()].Now3D = jointPoints[PositionIndex17.neck.Int()].Now3D + nhv * Vector3.Dot(nhv, nv);*/






        // Calculate spine location
        //jointPoints[PositionIndex17.spine.Int()].Now3D = jointPoints[PositionIndex17.spine.Int()].Now3D;
        jointPoints[PositionIndex15.spine.Int()].Now3D = jointPoints[PositionIndex15.hip.Int()].Now3D + (jointPoints[PositionIndex15.neck.Int()].Now3D - jointPoints[PositionIndex15.hip.Int()].Now3D) * 0.1f;

        //foot end location
        var projectionL = Vector3.ProjectOnPlane(VNectModel.forward, Vector3.up);
        projectionL = projectionL.normalized * VNectModel.footLength;
        var footPosL = jointPoints[PositionIndex15.lFoot.Int()].Now3D;
        footPosL.y = footPosL.y - VNectModel.ankleHeight;
        jointPoints[PositionIndex15.lToe.Int()].Now3D = footPosL + projectionL;

        var projectionR = Vector3.ProjectOnPlane(VNectModel.forward, Vector3.up);
        projectionR = projectionR.normalized * VNectModel.footLength;
        var footPosR = jointPoints[PositionIndex15.rFoot.Int()].Now3D;
        footPosR.y = footPosR.y - VNectModel.ankleHeight;
        jointPoints[PositionIndex15.rToe.Int()].Now3D = footPosR + projectionR;

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
                jp.PrevPos3D[0] = jp.Pos3D;
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
    void KalmanUpdate(VNectModel17.JointPoint measurement)
    {
        measurementUpdate(measurement);
        measurement.Pos3D.x = measurement.X.x + (measurement.Now3D.x - measurement.X.x) * measurement.K.x;
        measurement.Pos3D.y = measurement.X.y + (measurement.Now3D.y - measurement.X.y) * measurement.K.y;
        measurement.Pos3D.z = measurement.X.z + (measurement.Now3D.z - measurement.X.z) * measurement.K.z;
        measurement.X = measurement.Pos3D;
    }

	void measurementUpdate(VNectModel17.JointPoint measurement)
    {
        measurement.K.x = (measurement.P.x + KalmanParamQ) / (measurement.P.x + KalmanParamQ + KalmanParamR);
        measurement.K.y = (measurement.P.y + KalmanParamQ) / (measurement.P.y + KalmanParamQ + KalmanParamR);
        measurement.K.z = (measurement.P.z + KalmanParamQ) / (measurement.P.z + KalmanParamQ + KalmanParamR);
        measurement.P.x = KalmanParamR * (measurement.P.x + KalmanParamQ) / (KalmanParamR + measurement.P.x + KalmanParamQ);
        measurement.P.y = KalmanParamR * (measurement.P.y + KalmanParamQ) / (KalmanParamR + measurement.P.y + KalmanParamQ);
        measurement.P.z = KalmanParamR * (measurement.P.z + KalmanParamQ) / (KalmanParamR + measurement.P.z + KalmanParamQ);
    }
}
