using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extrinsic_setup : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 pos = new(33.99f, 1.1f, 0.49f);
    public Vector3 gaze = new(-0.93f, -0.019f, -0.3662f);
    Transform extrinsic;
    void Start()
    {
    //    Vector3 pos = new Vector3();
    //    Vector3 gaze = new Vector3();
        //Vector3 up = new Vector3();
        extrinsic = gameObject.GetComponent<Transform>();
        //pos = new (33.99f, 1.1f, 0.49f);
        //up = new (33.62f, -0.0713f, 1.424f);
        //gaze = new(-0.93f, -0.019f, -0.3662f);
        extrinsic.position = pos;
        extrinsic.rotation = Quaternion.LookRotation(gaze);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
