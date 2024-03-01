using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class placeManager : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject spawnablePrefab;
    Camera arCam;
    GameObject spawnedObject;
    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
