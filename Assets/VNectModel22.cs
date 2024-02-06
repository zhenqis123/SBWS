using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position index of joint points
/// </summary>
public enum PositionIndex22 : int
{

    /*hip = 0,
    lThighBend,
    rThighBend,
    spine,
    lShin,
    rShin,
    spine2,
    lFoot,
    rFoot,
    spine3,
    lToe,
    rToe,
    neck,
    lCollar,
    rCollar,
    head,
    lShldrBend,
    rShldrBend,
    lForearmBend,
    rForearmBend,
    lHand,
    rHand,

    Nose,
    Count,
    None,*/

    hip = 0,
    lThighBend,
    rThighBend,
    spine,
    lShin,
    rShin,
    spine2,
    lFoot,
    rFoot,
    spine3,
    lToe,
    rToe,
    head,
    lCollar,
    rCollar,
    Nose,
    lShldrBend,
    rShldrBend,
    lForearmBend,
    rForearmBend,
    lHand,
    rHand,

    neck,
    Count,
    None,

    /*"pelvis",
    "left_hip",
    "right_hip",
    "spine1",
    "left_knee",
    "right_knee",
    "spine2",
    "left_ankle",
    "right_ankle",
    "spine3",
    "left_foot",
    "right_foot",
    "neck",
    "left_collar",
    "right_collar",
    "head",
    "left_shoulder",
    "right_shoulder",
    "left_elbow",
    "right_elbow",
    "left_wrist",
    "right_wrist", # 22 joints
    "left_index1",
    "left_index2",
    "left_index3",
    "left_middle1",
    "left_middle2",
    "left_middle3",
    "left_pinky1",
    "left_pinky2",
    "left_pinky3",
    "left_ring1",
    "left_ring2",
    "left_ring3",
    "left_thumb1",
    "left_thumb2",
    "left_thumb3",
    "right_index1",
    "right_index2",
    "right_index3",
    "right_middle1",
    "right_middle2",
    "right_middle3",
    "right_pinky1",
    "right_pinky2",
    "right_pinky3",
    "right_ring1",
    "right_ring2",
    "right_ring3",
    "right_thumb1",
    "right_thumb2",
    "right_thumb3",*/
}

public static partial class EnumExtend
{
    public static int Int(this PositionIndex22 i)
    {
        return (int)i;
    }
}

public class VNectModel22 : MonoBehaviour
{
    public float xRandomOffset;
    public float zRandomOffset;
    //public float xEndingOffset;
    //public float zEndingOffset;

    public class JointPoint
    {
        public Vector2 Pos2D = new Vector2();
        public float score2D;

        public Vector3 Pos3D = new Vector3();
        public Vector3 Now3D = new Vector3();
        public Vector3[] PrevPos3D = new Vector3[6];
        public float score3D;

        // Bones
        public Transform Transform = null;
        public Quaternion InitRotation;
        public Quaternion Inverse;
        public Quaternion InverseRotation;

        public JointPoint Child = null;
        public JointPoint Parent = null;

        // For Kalman filter
        public Vector3 P = new Vector3();
        public Vector3 X = new Vector3();
        public Vector3 K = new Vector3();
    }

    public class Skeleton
    {
        public GameObject LineObject;
        public LineRenderer Line;

        public JointPoint start = null;
        public JointPoint end = null;
    }

    private List<Skeleton> Skeletons = new List<Skeleton>();
    public Material SkeletonMaterial;

    public bool ShowSkeleton;
    private bool useSkeleton;
    public float SkeletonX;
    public float SkeletonY;
    public float SkeletonZ;
    public float SkeletonScale;

    // Joint position and bone
    public JointPoint[] jointPoints;
    public JointPoint[] JointPoints { get { return jointPoints; } }

    //public float[,] originalData = new float[22,3];

    private Vector3 initPosition; // Initial center position

    private Quaternion InitGazeRotation;
    private Quaternion gazeInverse;

    // UnityChan
    public GameObject ModelObject;
    public GameObject Nose;
    private Animator anim;

    // Move in z direction
    private float centerTall = 224 * 0.75f;
    private float tall = 224 * 0.75f;
    private float prevTall = 224 * 0.75f;
    public float ZScale = 0.8f;

    public float neckLength;
    public Vector3 forward;
    public float yOffset;

    private void Update()
    {
        if (jointPoints != null)
        {
            PoseUpdate();
        }
    }

    /// <summary>
    /// Initialize joint points
    /// </summary>
    /// <returns></returns>
    public JointPoint[] Init()
    {
        jointPoints = new JointPoint[PositionIndex22.Count.Int()];
        for (var i = 0; i < PositionIndex22.Count.Int(); i++) jointPoints[i] = new JointPoint();

        anim = ModelObject.GetComponent<Animator>();

        // Right Arm
        jointPoints[PositionIndex22.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        jointPoints[PositionIndex22.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        jointPoints[PositionIndex22.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
        
        // Left Arm
        jointPoints[PositionIndex22.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        jointPoints[PositionIndex22.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        jointPoints[PositionIndex22.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);

        // added
        jointPoints[PositionIndex22.lCollar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
        jointPoints[PositionIndex22.rCollar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
        jointPoints[PositionIndex22.spine2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Chest);
        jointPoints[PositionIndex22.spine3.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.UpperChest);
        // Face

        jointPoints[PositionIndex22.Nose.Int()].Transform = Nose.transform;

        // Right Leg
        jointPoints[PositionIndex22.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        jointPoints[PositionIndex22.rShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        jointPoints[PositionIndex22.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        jointPoints[PositionIndex22.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);

        // Left Leg
        jointPoints[PositionIndex22.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        jointPoints[PositionIndex22.lShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        jointPoints[PositionIndex22.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        jointPoints[PositionIndex22.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);

        // etc
        jointPoints[PositionIndex22.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
        jointPoints[PositionIndex22.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
        jointPoints[PositionIndex22.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex22.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
        //jointPoints[PositionIndex22.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);

        // Child Settings
        // Right Arm
        //jointPoints[PositionIndex22.rCollar.Int()].Child = jointPoints[PositionIndex22.rShldrBend.Int()];
        //jointPoints[PositionIndex22.lShldrBend.Int()].Parent = jointPoints[PositionIndex22.lCollar.Int()];
        //jointPoints[PositionIndex22.rCollar.Int()].Parent = jointPoints[PositionIndex22.spine3.Int()];
        //jointPoints[PositionIndex22.rCollar.Int()].Child = jointPoints[PositionIndex22.rShldrBend.Int()];
        //jointPoints[PositionIndex22.rCollar.Int()].Child = jointPoints[PositionIndex22.rShldrBend.Int()];
        jointPoints[PositionIndex22.rShldrBend.Int()].Child = jointPoints[PositionIndex22.rForearmBend.Int()];
        jointPoints[PositionIndex22.rForearmBend.Int()].Child = jointPoints[PositionIndex22.rHand.Int()];
        jointPoints[PositionIndex22.rForearmBend.Int()].Parent = jointPoints[PositionIndex22.rShldrBend.Int()];

        // Left Arm
        //jointPoints[PositionIndex22.lCollar.Int()].Child = jointPoints[PositionIndex22.lShldrBend.Int()];
        //jointPoints[PositionIndex22.lShldrBend.Int()].Parent = jointPoints[PositionIndex22.lCollar.Int()];
        //jointPoints[PositionIndex22.lCollar.Int()].Parent = jointPoints[PositionIndex22.spine3.Int()];
        //jointPoints[PositionIndex22.lCollar.Int()].Child = jointPoints[PositionIndex22.lShldrBend.Int()];
        //jointPoints[PositionIndex22.lCollar.Int()].Child = jointPoints[PositionIndex22.lShldrBend.Int()];
        jointPoints[PositionIndex22.lShldrBend.Int()].Child = jointPoints[PositionIndex22.lForearmBend.Int()];
        jointPoints[PositionIndex22.lForearmBend.Int()].Child = jointPoints[PositionIndex22.lHand.Int()];
        jointPoints[PositionIndex22.lForearmBend.Int()].Parent = jointPoints[PositionIndex22.lShldrBend.Int()];

        // Fase

        // Right Leg
        jointPoints[PositionIndex22.rThighBend.Int()].Child = jointPoints[PositionIndex22.rShin.Int()];
        jointPoints[PositionIndex22.rShin.Int()].Child = jointPoints[PositionIndex22.rFoot.Int()];
        jointPoints[PositionIndex22.rFoot.Int()].Child = jointPoints[PositionIndex22.rToe.Int()];
        jointPoints[PositionIndex22.rFoot.Int()].Parent = jointPoints[PositionIndex22.rShin.Int()];

        // Left Leg
        jointPoints[PositionIndex22.lThighBend.Int()].Child = jointPoints[PositionIndex22.lShin.Int()];
        jointPoints[PositionIndex22.lShin.Int()].Child = jointPoints[PositionIndex22.lFoot.Int()];
        jointPoints[PositionIndex22.lFoot.Int()].Child = jointPoints[PositionIndex22.lToe.Int()];
        jointPoints[PositionIndex22.lFoot.Int()].Parent = jointPoints[PositionIndex22.lShin.Int()];

        // etc
        //jointPoints[PositionIndex22.neck.Int()].Child = jointPoints[PositionIndex22.lCollar.Int()];
        //jointPoints[PositionIndex22.neck.Int()].Child = jointPoints[PositionIndex22.rCollar.Int()];
        //jointPoints[PositionIndex22.spine3.Int()].Parent = jointPoints[PositionIndex22.spine2.Int()];
        //jointPoints[PositionIndex22.spine2.Int()].Parent = jointPoints[PositionIndex22.spine.Int()];
        //jointPoints[PositionIndex22.spine.Int()].Parent = jointPoints[PositionIndex22.hip.Int()];
        //jointPoints[PositionIndex22.hip.Int()].Child = jointPoints[PositionIndex22.spine.Int()];
        //jointPoints[PositionIndex22.spine.Int()].Child = jointPoints[PositionIndex22.spine2.Int()];
        //jointPoints[PositionIndex22.spine2.Int()].Child = jointPoints[PositionIndex22.spine3.Int()];
        //jointPoints[PositionIndex22.spine3.Int()].Child = jointPoints[PositionIndex22.neck.Int()];
        jointPoints[PositionIndex22.spine.Int()].Child = jointPoints[PositionIndex22.neck.Int()];
        jointPoints[PositionIndex22.neck.Int()].Child = jointPoints[PositionIndex22.head.Int()];
        jointPoints[PositionIndex22.head.Int()].Child = jointPoints[PositionIndex22.Nose.Int()];

        useSkeleton = ShowSkeleton;
        if (useSkeleton)
        {
            // Line Child Settings
            // Right Arm
            AddSkeleton(PositionIndex22.rShldrBend, PositionIndex22.rForearmBend);
            AddSkeleton(PositionIndex22.rForearmBend, PositionIndex22.rHand);
            

            // Left Arm
            AddSkeleton(PositionIndex22.lShldrBend, PositionIndex22.lForearmBend);
            AddSkeleton(PositionIndex22.lForearmBend, PositionIndex22.lHand);
            

            // Fase
            /*AddSkeleton(PositionIndex22.lEar, PositionIndex22.Nose);
            AddSkeleton(PositionIndex22.rEar, PositionIndex22.Nose);*/

            // Right Leg
            AddSkeleton(PositionIndex22.rThighBend, PositionIndex22.rShin);
            AddSkeleton(PositionIndex22.rShin, PositionIndex22.rFoot);
            AddSkeleton(PositionIndex22.rFoot, PositionIndex22.rToe);

            // Left Leg
            AddSkeleton(PositionIndex22.lThighBend, PositionIndex22.lShin);
            AddSkeleton(PositionIndex22.lShin, PositionIndex22.lFoot);
            AddSkeleton(PositionIndex22.lFoot, PositionIndex22.lToe);

            // etc
            //AddSkeleton(PositionIndex22.spine3, PositionIndex22.neck);
            //AddSkeleton(PositionIndex22.spine2, PositionIndex22.spine3);
            //AddSkeleton(PositionIndex22.spine, PositionIndex22.spine2);

            AddSkeleton(PositionIndex22.spine, PositionIndex22.neck);
            AddSkeleton(PositionIndex22.neck, PositionIndex22.head);
            AddSkeleton(PositionIndex22.head, PositionIndex22.Nose);
            //AddSkeleton(PositionIndex22.neck, PositionIndex22.rShldrBend);
            //AddSkeleton(PositionIndex22.neck, PositionIndex22.lShldrBend);
            AddSkeleton(PositionIndex22.neck, PositionIndex22.rCollar);
            AddSkeleton(PositionIndex22.rCollar, PositionIndex22.rShldrBend);
            AddSkeleton(PositionIndex22.neck, PositionIndex22.lCollar);
            AddSkeleton(PositionIndex22.lCollar, PositionIndex22.lShldrBend);

            AddSkeleton(PositionIndex22.rThighBend, PositionIndex22.rShldrBend);
            AddSkeleton(PositionIndex22.lThighBend, PositionIndex22.lShldrBend);
            AddSkeleton(PositionIndex22.rShldrBend, PositionIndex22.spine);
            AddSkeleton(PositionIndex22.lShldrBend, PositionIndex22.spine);
            AddSkeleton(PositionIndex22.rThighBend, PositionIndex22.spine);
            AddSkeleton(PositionIndex22.lThighBend, PositionIndex22.spine);
            AddSkeleton(PositionIndex22.lThighBend, PositionIndex22.rThighBend);
        }

        // Set Inverse
        var forward = TriangleNormal(jointPoints[PositionIndex22.hip.Int()].Transform.position, jointPoints[PositionIndex22.lThighBend.Int()].Transform.position, jointPoints[PositionIndex22.rThighBend.Int()].Transform.position);
        foreach (var jointPoint in jointPoints)
        {
            if (jointPoint.Transform != null)
            {
                jointPoint.InitRotation = jointPoint.Transform.rotation;
            }

            if (jointPoint.Child != null)
            {
                jointPoint.Inverse = GetInverse(jointPoint, jointPoint.Child, forward);
                jointPoint.InverseRotation = jointPoint.Inverse * jointPoint.InitRotation;
            }
        }
        var hip = jointPoints[PositionIndex22.hip.Int()];
        initPosition = jointPoints[PositionIndex22.hip.Int()].Transform.position;
        hip.Inverse = Quaternion.Inverse(Quaternion.LookRotation(forward));
        hip.InverseRotation = hip.Inverse * hip.InitRotation;

        //record neck length
        //var midneck = (jointPoints[PositionIndex22.lCollar.Int()].Now3D + jointPoints[PositionIndex22.rCollar.Int()].Now3D) / 2f;
        //var neckVector = jointPoints[PositionIndex22.head.Int()].Transform.position - midneck;
        var neckVector = jointPoints[PositionIndex22.head.Int()].Transform.position - jointPoints[PositionIndex22.neck.Int()].Transform.position;
        neckLength = Mathf.Sqrt(neckVector.x * neckVector.x + neckVector.y * neckVector.y + neckVector.z * neckVector.z);

        // For Head Rotation
        var head = jointPoints[PositionIndex22.head.Int()];
        head.InitRotation = jointPoints[PositionIndex22.head.Int()].Transform.rotation;
        var gaze = jointPoints[PositionIndex22.Nose.Int()].Transform.position - jointPoints[PositionIndex22.head.Int()].Transform.position;
        head.Inverse = Quaternion.Inverse(Quaternion.LookRotation(gaze));
        head.InverseRotation = head.Inverse * head.InitRotation;
        
        var lHand = jointPoints[PositionIndex22.lHand.Int()];
        //var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex22.lMid1.Int()].Pos3D, jointPoints[PositionIndex22.lThumb2.Int()].Pos3D);
        lHand.InitRotation = lHand.Transform.rotation;
        //lHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex22.lThumb2.Int()].Transform.position - jointPoints[PositionIndex22.lMid1.Int()].Transform.position, lf));
        lHand.InverseRotation = lHand.Inverse * lHand.InitRotation;

        var rHand = jointPoints[PositionIndex22.rHand.Int()];
        //var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex22.rThumb2.Int()].Pos3D, jointPoints[PositionIndex22.rMid1.Int()].Pos3D);
        rHand.InitRotation = jointPoints[PositionIndex22.rHand.Int()].Transform.rotation;
        //rHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex22.rThumb2.Int()].Transform.position - jointPoints[PositionIndex22.rMid1.Int()].Transform.position, rf));
        rHand.InverseRotation = rHand.Inverse * rHand.InitRotation;

        jointPoints[PositionIndex22.hip.Int()].score3D = 1f;
        jointPoints[PositionIndex22.neck.Int()].score3D = 1f;
        jointPoints[PositionIndex22.Nose.Int()].score3D = 1f;
        jointPoints[PositionIndex22.head.Int()].score3D = 1f;
        jointPoints[PositionIndex22.spine.Int()].score3D = 1f;


        return JointPoints;
    }

    public void PoseUpdate()
    {
        // caliculate movement range of z-coordinate from height
        var t1 = Vector3.Distance(jointPoints[PositionIndex22.head.Int()].Pos3D, jointPoints[PositionIndex22.neck.Int()].Pos3D);
        var t2 = Vector3.Distance(jointPoints[PositionIndex22.neck.Int()].Pos3D, jointPoints[PositionIndex22.spine.Int()].Pos3D);
        var pm = (jointPoints[PositionIndex22.rThighBend.Int()].Pos3D + jointPoints[PositionIndex22.lThighBend.Int()].Pos3D) / 2f;
        var t3 = Vector3.Distance(jointPoints[PositionIndex22.spine.Int()].Pos3D, pm);
        var t4r = Vector3.Distance(jointPoints[PositionIndex22.rThighBend.Int()].Pos3D, jointPoints[PositionIndex22.rShin.Int()].Pos3D);
        var t4l = Vector3.Distance(jointPoints[PositionIndex22.lThighBend.Int()].Pos3D, jointPoints[PositionIndex22.lShin.Int()].Pos3D);
        var t4 = (t4r + t4l) / 2f;
        var t5r = Vector3.Distance(jointPoints[PositionIndex22.rShin.Int()].Pos3D, jointPoints[PositionIndex22.rFoot.Int()].Pos3D);
        var t5l = Vector3.Distance(jointPoints[PositionIndex22.lShin.Int()].Pos3D, jointPoints[PositionIndex22.lFoot.Int()].Pos3D);
        var t5 = (t5r + t5l) / 2f;
        var t = t1 + t2 + t3 + t4 + t5;


        // Low pass filter in z direction
        tall = t * 0.7f + prevTall * 0.3f;
        prevTall = tall;

        if (tall == 0)
        {
            tall = centerTall;
        }
        var dz = (centerTall - tall) / centerTall * ZScale;

        /*// movement and rotatation of center
        var forward = TriangleNormal(jointPoints[PositionIndex22.hip.Int()].Pos3D, jointPoints[PositionIndex22.lThighBend.Int()].Pos3D, jointPoints[PositionIndex22.rThighBend.Int()].Pos3D);
        jointPoints[PositionIndex22.hip.Int()].Transform.position = jointPoints[PositionIndex22.hip.Int()].Pos3D * 0.005f + new Vector3(initPosition.x, initPosition.y, initPosition.z + dz);
        jointPoints[PositionIndex22.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward) * jointPoints[PositionIndex22.hip.Int()].InverseRotation;*/

        // movement and rotatation of center
        forward = TriangleNormal(jointPoints[PositionIndex22.hip.Int()].Pos3D, jointPoints[PositionIndex22.lThighBend.Int()].Pos3D, jointPoints[PositionIndex22.rThighBend.Int()].Pos3D);
        //print(forward);
        //jointPoints[PositionIndex22.hip.Int()].Transform.position = jointPoints[PositionIndex22.hip.Int()].Pos3D /** 0.005f*/ + new Vector3(initPosition.x, initPosition.y, initPosition.z + dz);
        jointPoints[PositionIndex22.hip.Int()].Transform.position = jointPoints[PositionIndex22.hip.Int()].Pos3D /** 0.005f*/ + new Vector3(xRandomOffset, yOffset, zRandomOffset); //offset applied
        //jointPoints[PositionIndex22.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward, hipDirectionReference()) * jointPoints[PositionIndex22.hip.Int()].InverseRotation;
        jointPoints[PositionIndex22.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward, jointPoints[PositionIndex22.spine.Int()].Pos3D - jointPoints[PositionIndex22.hip.Int()].Pos3D) * jointPoints[PositionIndex22.hip.Int()].InverseRotation;

        //slightly offset character so its feet touches the ground
        //jointPoints[PositionIndex22.hip.Int()].Transform.position.y = jointPoints[PositionIndex22.hip.Int()].Transform.position.y - 0.05f;

        // rotate each of bones
        foreach (var jointPoint in jointPoints)
        {
            if (jointPoint.Parent != null)
            {
                var fv = jointPoint.Parent.Pos3D - jointPoint.Pos3D;
                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, fv) * jointPoint.InverseRotation;
            }
            else if (jointPoint.Child != null)
            {
                jointPoint.Transform.rotation = Quaternion.LookRotation(jointPoint.Pos3D - jointPoint.Child.Pos3D, forward) * jointPoint.InverseRotation;
            }
        }

        // Head Rotation
        /*var gaze = jointPoints[PositionIndex22.Nose.Int()].Pos3D - jointPoints[PositionIndex22.head.Int()].Pos3D;
        var f = TriangleNormal(jointPoints[PositionIndex22.Nose.Int()].Pos3D, jointPoints[PositionIndex22.rEar.Int()].Pos3D, jointPoints[PositionIndex22.lEar.Int()].Pos3D);
        var head = jointPoints[PositionIndex22.head.Int()];
        head.Transform.rotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;*/
        var gaze = jointPoints[PositionIndex22.Nose.Int()].Pos3D - jointPoints[PositionIndex22.head.Int()].Pos3D;
        var parallel = jointPoints[PositionIndex22.lShldrBend.Int()].Pos3D - jointPoints[PositionIndex22.rShldrBend.Int()].Pos3D;
        parallel = parallel.normalized * 0.1f;
        var nextToNose = jointPoints[PositionIndex22.Nose.Int()].Pos3D + parallel;
        //var nextToNose = jointPoints[PositionIndex22.nose.Int()].Pos3D;
        //nextToNose.x = nextToNose.x + 0.1f;
        //nextToNose.z = nextToNose.z + 0.1f;
        var f = TriangleNormal(jointPoints[PositionIndex22.Nose.Int()].Pos3D, jointPoints[PositionIndex22.head.Int()].Pos3D, nextToNose);
        var head = jointPoints[PositionIndex22.head.Int()];
        head.Transform.rotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;

        // Wrist rotation (Test code)
        /*var lHand = jointPoints[PositionIndex22.lHand.Int()];
        var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex22.lMid1.Int()].Pos3D, jointPoints[PositionIndex22.lThumb2.Int()].Pos3D);
        lHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex22.lThumb2.Int()].Pos3D - jointPoints[PositionIndex22.lMid1.Int()].Pos3D, lf) * lHand.InverseRotation;

        var rHand = jointPoints[PositionIndex22.rHand.Int()];
        var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex22.rThumb2.Int()].Pos3D, jointPoints[PositionIndex22.rMid1.Int()].Pos3D);
        //rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex22.rThumb2.Int()].Pos3D - jointPoints[PositionIndex22.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;
        rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex22.rThumb2.Int()].Pos3D - jointPoints[PositionIndex22.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;*/

        foreach (var sk in Skeletons)
        {
            var s = sk.start;
            var e = sk.end;

            sk.Line.SetPosition(0, new Vector3(s.Pos3D.x * SkeletonScale + SkeletonX, s.Pos3D.y * SkeletonScale + SkeletonY, s.Pos3D.z * SkeletonScale + SkeletonZ));
            sk.Line.SetPosition(1, new Vector3(e.Pos3D.x * SkeletonScale + SkeletonX, e.Pos3D.y * SkeletonScale + SkeletonY, e.Pos3D.z * SkeletonScale + SkeletonZ));
        }
    }

    Vector3 TriangleNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 d1 = a - b;
        Vector3 d2 = a - c;

        Vector3 dd = Vector3.Cross(d1, d2);
        dd.Normalize();

        return dd;
    }

    /*Vector3 hipDirectionReference()
    {
        //float middle = (jointPoints[PositionIndex22.lThighBend.Int()].Transform.position.y + jointPoints[PositionIndex22.rThighBend.Int()].Transform.position.y) / 2f;
        float middle = (originalData[1,1] + originalData[2, 1]) / 2f;
        //Debug.Log("lh: " + originalData[1, 0] + originalData[1, 1]+ originalData[1, 2] + "   rh: " + originalData[2, 0]+ originalData[2, 1]+ originalData[2, 2]);
        //Debug.Log("middle: " + middle + jointPoints[PositionIndex22.hip.Int()].Transform.position.y);
        Debug.Log("middle: " + originalData[0, 1]);
        

        if (middle < originalData[0,1])
        {
            print("up");
            return Vector3.up;
        } 
        else
        {
            print("down");
            return Vector3.down;
        }
    }*/

    private Quaternion GetInverse(JointPoint p1, JointPoint p2, Vector3 forward)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
    }

    /// <summary>
    /// Add skelton from joint points
    /// </summary>
    /// <param name="s">position index</param>
    /// <param name="e">position index</param>
    private void AddSkeleton(PositionIndex22 s, PositionIndex22 e)
    {
        var sk = new Skeleton()
        {
            LineObject = new GameObject("Line"),
            start = jointPoints[s.Int()],
            end = jointPoints[e.Int()],
        };

        sk.Line = sk.LineObject.AddComponent<LineRenderer>();
        sk.Line.startWidth = 0.04f;
        sk.Line.endWidth = 0.01f;
        
        // define the number of vertex
        sk.Line.positionCount = 2;
        sk.Line.material = SkeletonMaterial;

        Skeletons.Add(sk);
    }
}
