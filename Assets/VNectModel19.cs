using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position index of joint points
/// </summary>
public enum PositionIndex19 : int
{
    /*rShldrBend = 0,
    rForearmBend,
    rHand,
    rThumb2,
    rMid1,

    lShldrBend,
    lForearmBend,
    lHand,
    lThumb2,
    lMid1,

    lEar,
    lEye,
    rEar,
    rEye,
    Nose,

    rThighBend,
    rShin,
    rFoot,
    rToe,

    lThighBend,
    lShin,
    lFoot,
    lToe,

    abdomenUpper,*/
    neck = 0,
    Nose,
    hip,
    lShldrBend,
    lForearmBend,
    lHand,
    lThighBend,
    lShin,
    lFoot,
    rShldrBend,
    rForearmBend,
    rHand,
    rThighBend,
    rShin,
    rFoot,
    lEye,
    lEar,
    rEye,
    rEar,

    //Calculated coordinates
    //hip,
    head,
    //neck,
    spine,
    lToe,
    rToe,

    Count,
    None,
}

public static partial class EnumExtend
{
    public static int Int(this PositionIndex19 i)
    {
        return (int)i;
    }
}

public class VNectModel19 : MonoBehaviour
{

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
    private JointPoint[] jointPoints;
    public JointPoint[] JointPoints { get { return jointPoints; } }

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
    public float ankleHeight;
    public float footLength;
    public Vector3 forward;

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
        jointPoints = new JointPoint[PositionIndex19.Count.Int()];
        for (var i = 0; i < PositionIndex19.Count.Int(); i++) jointPoints[i] = new JointPoint();

        anim = ModelObject.GetComponent<Animator>();

        // Right Arm
        jointPoints[PositionIndex19.rShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
        jointPoints[PositionIndex19.rForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
        jointPoints[PositionIndex19.rHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightHand);
        //jointPoints[PositionIndex19.rThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightThumbIntermediate);
        //jointPoints[PositionIndex19.rMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightMiddleProximal);
        // Left Arm
        jointPoints[PositionIndex19.lShldrBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperArm);
        jointPoints[PositionIndex19.lForearmBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        jointPoints[PositionIndex19.lHand.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftHand);
        //jointPoints[PositionIndex19.lThumb2.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate);
        //jointPoints[PositionIndex19.lMid1.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftMiddleProximal);

        // Face
        jointPoints[PositionIndex19.lEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex19.lEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftEye);
        jointPoints[PositionIndex19.rEar.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex19.rEye.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightEye);
        jointPoints[PositionIndex19.Nose.Int()].Transform = Nose.transform;

        // Right Leg
        jointPoints[PositionIndex19.rThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        jointPoints[PositionIndex19.rShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        jointPoints[PositionIndex19.rFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        jointPoints[PositionIndex19.rToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.RightToes);

        // Left Leg
        jointPoints[PositionIndex19.lThighBend.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        jointPoints[PositionIndex19.lShin.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
        jointPoints[PositionIndex19.lFoot.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        jointPoints[PositionIndex19.lToe.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.LeftToes);

        // etc
        //jointPoints[PositionIndex19.abdomenUpper.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);
        jointPoints[PositionIndex19.hip.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Hips);
        jointPoints[PositionIndex19.head.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Head);
        jointPoints[PositionIndex19.neck.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Neck);
        jointPoints[PositionIndex19.spine.Int()].Transform = anim.GetBoneTransform(HumanBodyBones.Spine);

        // Child Settings
        // Right Arm
        jointPoints[PositionIndex19.rShldrBend.Int()].Child = jointPoints[PositionIndex19.rForearmBend.Int()];
        jointPoints[PositionIndex19.rForearmBend.Int()].Child = jointPoints[PositionIndex19.rHand.Int()];
        jointPoints[PositionIndex19.rForearmBend.Int()].Parent = jointPoints[PositionIndex19.rShldrBend.Int()];

        // Left Arm
        jointPoints[PositionIndex19.lShldrBend.Int()].Child = jointPoints[PositionIndex19.lForearmBend.Int()];
        jointPoints[PositionIndex19.lForearmBend.Int()].Child = jointPoints[PositionIndex19.lHand.Int()];
        jointPoints[PositionIndex19.lForearmBend.Int()].Parent = jointPoints[PositionIndex19.lShldrBend.Int()];

        // Fase

        // Right Leg
        jointPoints[PositionIndex19.rThighBend.Int()].Child = jointPoints[PositionIndex19.rShin.Int()];
        jointPoints[PositionIndex19.rShin.Int()].Child = jointPoints[PositionIndex19.rFoot.Int()];
        jointPoints[PositionIndex19.rFoot.Int()].Child = jointPoints[PositionIndex19.rToe.Int()];
        jointPoints[PositionIndex19.rFoot.Int()].Parent = jointPoints[PositionIndex19.rShin.Int()];

        // Left Leg
        jointPoints[PositionIndex19.lThighBend.Int()].Child = jointPoints[PositionIndex19.lShin.Int()];
        jointPoints[PositionIndex19.lShin.Int()].Child = jointPoints[PositionIndex19.lFoot.Int()];
        jointPoints[PositionIndex19.lFoot.Int()].Child = jointPoints[PositionIndex19.lToe.Int()];
        jointPoints[PositionIndex19.lFoot.Int()].Parent = jointPoints[PositionIndex19.lShin.Int()];

        // etc
        jointPoints[PositionIndex19.spine.Int()].Child = jointPoints[PositionIndex19.neck.Int()];
        jointPoints[PositionIndex19.neck.Int()].Child = jointPoints[PositionIndex19.head.Int()];
        //jointPoints[PositionIndex19.head.Int()].Child = jointPoints[PositionIndex19.Nose.Int()];

        useSkeleton = ShowSkeleton;
        if (useSkeleton)
        {
            // Line Child Settings
            // Right Arm
            AddSkeleton(PositionIndex19.rShldrBend, PositionIndex19.rForearmBend);
            AddSkeleton(PositionIndex19.rForearmBend, PositionIndex19.rHand);
            //AddSkeleton(PositionIndex19.rHand, PositionIndex19.rThumb2);
            //AddSkeleton(PositionIndex19.rHand, PositionIndex19.rMid1);

            // Left Arm
            AddSkeleton(PositionIndex19.lShldrBend, PositionIndex19.lForearmBend);
            AddSkeleton(PositionIndex19.lForearmBend, PositionIndex19.lHand);
            //AddSkeleton(PositionIndex19.lHand, PositionIndex19.lThumb2);
           // AddSkeleton(PositionIndex19.lHand, PositionIndex19.lMid1);

            // Fase
            AddSkeleton(PositionIndex19.lEar, PositionIndex19.Nose);
            AddSkeleton(PositionIndex19.rEar, PositionIndex19.Nose);

            // Right Leg
            AddSkeleton(PositionIndex19.rThighBend, PositionIndex19.rShin);
            AddSkeleton(PositionIndex19.rShin, PositionIndex19.rFoot);
            AddSkeleton(PositionIndex19.rFoot, PositionIndex19.rToe);

            // Left Leg
            AddSkeleton(PositionIndex19.lThighBend, PositionIndex19.lShin);
            AddSkeleton(PositionIndex19.lShin, PositionIndex19.lFoot);
            AddSkeleton(PositionIndex19.lFoot, PositionIndex19.lToe);

            // etc
            AddSkeleton(PositionIndex19.spine, PositionIndex19.neck);
            AddSkeleton(PositionIndex19.neck, PositionIndex19.head);
            AddSkeleton(PositionIndex19.head, PositionIndex19.Nose);
            AddSkeleton(PositionIndex19.neck, PositionIndex19.rShldrBend);
            AddSkeleton(PositionIndex19.neck, PositionIndex19.lShldrBend);
            AddSkeleton(PositionIndex19.rThighBend, PositionIndex19.rShldrBend);
            AddSkeleton(PositionIndex19.lThighBend, PositionIndex19.lShldrBend);
            AddSkeleton(PositionIndex19.rShldrBend, PositionIndex19.spine);
            AddSkeleton(PositionIndex19.lShldrBend, PositionIndex19.spine);
            AddSkeleton(PositionIndex19.rThighBend, PositionIndex19.spine);
            AddSkeleton(PositionIndex19.lThighBend, PositionIndex19.spine);
            AddSkeleton(PositionIndex19.lThighBend, PositionIndex19.rThighBend);
        }

        // Set Inverse
        var forward = TriangleNormal(jointPoints[PositionIndex19.hip.Int()].Transform.position, jointPoints[PositionIndex19.lThighBend.Int()].Transform.position, jointPoints[PositionIndex19.rThighBend.Int()].Transform.position);
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
        var hip = jointPoints[PositionIndex19.hip.Int()];
        initPosition = jointPoints[PositionIndex19.hip.Int()].Transform.position;
        hip.Inverse = Quaternion.Inverse(Quaternion.LookRotation(forward));
        hip.InverseRotation = hip.Inverse * hip.InitRotation;

        //record neck length
        var neckVector = jointPoints[PositionIndex15.head.Int()].Transform.position - jointPoints[PositionIndex15.neck.Int()].Transform.position;
        neckLength = Mathf.Sqrt(neckVector.x * neckVector.x + neckVector.y * neckVector.y + neckVector.z * neckVector.z);

        //record ankle height foot length
        ankleHeight = jointPoints[PositionIndex15.lFoot.Int()].Transform.position.y;
        var heel = jointPoints[PositionIndex15.lFoot.Int()].Transform.position;
        heel.y = heel.y - ankleHeight;
        var footBase = anim.GetBoneTransform(HumanBodyBones.LeftToes).position - heel;
        footLength = Mathf.Sqrt(footBase.x * footBase.x + footBase.y * footBase.y + footBase.z * footBase.z);

        // For Head Rotation
        /*var head = jointPoints[PositionIndex19.head.Int()];
        head.InitRotation = jointPoints[PositionIndex19.head.Int()].Transform.rotation;
        var gaze = jointPoints[PositionIndex19.Nose.Int()].Transform.position - jointPoints[PositionIndex19.head.Int()].Transform.position;
        head.Inverse = Quaternion.Inverse(Quaternion.LookRotation(gaze));
        head.InverseRotation = head.Inverse * head.InitRotation;*/






        
        var lHand = jointPoints[PositionIndex19.lHand.Int()];
        //var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex19.lMid1.Int()].Pos3D, jointPoints[PositionIndex19.lThumb2.Int()].Pos3D);
        lHand.InitRotation = lHand.Transform.rotation;
        //lHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex19.lThumb2.Int()].Transform.position - jointPoints[PositionIndex19.lMid1.Int()].Transform.position, lf));
        lHand.InverseRotation = lHand.Inverse * lHand.InitRotation;

        var rHand = jointPoints[PositionIndex19.rHand.Int()];
        //var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex19.rThumb2.Int()].Pos3D, jointPoints[PositionIndex19.rMid1.Int()].Pos3D);
        rHand.InitRotation = jointPoints[PositionIndex19.rHand.Int()].Transform.rotation;
        //rHand.Inverse = Quaternion.Inverse(Quaternion.LookRotation(jointPoints[PositionIndex19.rThumb2.Int()].Transform.position - jointPoints[PositionIndex19.rMid1.Int()].Transform.position, rf));
        rHand.InverseRotation = rHand.Inverse * rHand.InitRotation;

        jointPoints[PositionIndex19.hip.Int()].score3D = 1f;
        jointPoints[PositionIndex19.neck.Int()].score3D = 1f;
        jointPoints[PositionIndex19.Nose.Int()].score3D = 1f;
        jointPoints[PositionIndex19.head.Int()].score3D = 1f;
        jointPoints[PositionIndex19.spine.Int()].score3D = 1f;


        return JointPoints;
    }

    public void PoseUpdate()
    {
        // caliculate movement range of z-coordinate from height
        var t1 = Vector3.Distance(jointPoints[PositionIndex19.head.Int()].Pos3D, jointPoints[PositionIndex19.neck.Int()].Pos3D);
        var t2 = Vector3.Distance(jointPoints[PositionIndex19.neck.Int()].Pos3D, jointPoints[PositionIndex19.spine.Int()].Pos3D);
        var pm = (jointPoints[PositionIndex19.rThighBend.Int()].Pos3D + jointPoints[PositionIndex19.lThighBend.Int()].Pos3D) / 2f;
        var t3 = Vector3.Distance(jointPoints[PositionIndex19.spine.Int()].Pos3D, pm);
        var t4r = Vector3.Distance(jointPoints[PositionIndex19.rThighBend.Int()].Pos3D, jointPoints[PositionIndex19.rShin.Int()].Pos3D);
        var t4l = Vector3.Distance(jointPoints[PositionIndex19.lThighBend.Int()].Pos3D, jointPoints[PositionIndex19.lShin.Int()].Pos3D);
        var t4 = (t4r + t4l) / 2f;
        var t5r = Vector3.Distance(jointPoints[PositionIndex19.rShin.Int()].Pos3D, jointPoints[PositionIndex19.rFoot.Int()].Pos3D);
        var t5l = Vector3.Distance(jointPoints[PositionIndex19.lShin.Int()].Pos3D, jointPoints[PositionIndex19.lFoot.Int()].Pos3D);
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

        // movement and rotatation of center
        var forward = TriangleNormal(jointPoints[PositionIndex19.hip.Int()].Pos3D, jointPoints[PositionIndex19.lThighBend.Int()].Pos3D, jointPoints[PositionIndex19.rThighBend.Int()].Pos3D);
        jointPoints[PositionIndex19.hip.Int()].Transform.position = jointPoints[PositionIndex19.hip.Int()].Pos3D * 0.005f + new Vector3(initPosition.x, initPosition.y, initPosition.z + dz);
        jointPoints[PositionIndex19.hip.Int()].Transform.rotation = Quaternion.LookRotation(forward) * jointPoints[PositionIndex19.hip.Int()].InverseRotation;

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
        /*var gaze = jointPoints[PositionIndex19.Nose.Int()].Pos3D - jointPoints[PositionIndex19.head.Int()].Pos3D;
        var f = TriangleNormal(jointPoints[PositionIndex19.Nose.Int()].Pos3D, jointPoints[PositionIndex19.rEar.Int()].Pos3D, jointPoints[PositionIndex19.lEar.Int()].Pos3D);
        var head = jointPoints[PositionIndex19.head.Int()];
        head.Transform.rotation = Quaternion.LookRotation(gaze, f) * head.InverseRotation;*/




        
        // Wrist rotation (Test code)
        var lHand = jointPoints[PositionIndex19.lHand.Int()];
        //var lf = TriangleNormal(lHand.Pos3D, jointPoints[PositionIndex19.lMid1.Int()].Pos3D, jointPoints[PositionIndex19.lThumb2.Int()].Pos3D);
        //lHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex19.lThumb2.Int()].Pos3D - jointPoints[PositionIndex19.lMid1.Int()].Pos3D, lf) * lHand.InverseRotation;

        var rHand = jointPoints[PositionIndex19.rHand.Int()];
        //var rf = TriangleNormal(rHand.Pos3D, jointPoints[PositionIndex19.rThumb2.Int()].Pos3D, jointPoints[PositionIndex19.rMid1.Int()].Pos3D);
        //rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex19.rThumb2.Int()].Pos3D - jointPoints[PositionIndex19.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;
        //rHand.Transform.rotation = Quaternion.LookRotation(jointPoints[PositionIndex19.rThumb2.Int()].Pos3D - jointPoints[PositionIndex19.rMid1.Int()].Pos3D, rf) * rHand.InverseRotation;

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

    private Quaternion GetInverse(JointPoint p1, JointPoint p2, Vector3 forward)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(p1.Transform.position - p2.Transform.position, forward));
    }

    /// <summary>
    /// Add skelton from joint points
    /// </summary>
    /// <param name="s">position index</param>
    /// <param name="e">position index</param>
    private void AddSkeleton(PositionIndex19 s, PositionIndex19 e)
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
