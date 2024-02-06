using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderYModifier : MonoBehaviour
{
   
    public Transform hips;
    public Transform leftAnkle;
    public Transform rightAnkle;

    private CapsuleCollider colliderGeo;

    private float StartingColliderHeight = 0.86f;
    private float approximateAnkleRadius = 0.09f;
    private float startingHipHeight;
   // private float startingCharacterHeight;

    // Start is called before the first frame update
    void Start()
    {
        /*hips = FindGrandChildWithTag(transform, "hips");
        leftAnkle = FindGrandChildWithTag(transform, "left_ankle");
        rightAnkle = FindGrandChildWithTag(transform, "right_ankle");
        if (hips == null)
        {
            Debug.Log("1111" + hips.tag);
        }
        Debug.Log(hips.tag);
        if (leftAnkle == null)
        {
            Debug.Log(leftAnkle.tag);
        }
        Debug.Log(leftAnkle.tag);
        if (rightAnkle == null)
        {
            Debug.Log(rightAnkle.tag);
        }
        Debug.Log("1111" + rightAnkle.tag);*/
        //DisplayChildren(transform, "hips", hips);
        //Debug.Log(hips.tag);
        //DisplayChildren(transform, "left_ankle", leftAnkle);
        //Debug.Log(leftAnkle.tag);
        //DisplayChildren(transform, "left_wrist", rightAnkle);
        //Debug.Log(rightAnkle.tag);

        startingHipHeight = hips.transform.localPosition.y;
       // startingCharacterHeight = gameObject.transform.position.y;
        colliderGeo = gameObject.GetComponent<CapsuleCollider>();

        //modify collider dimension
        colliderGeo.center = new Vector3(colliderGeo.center.x, startingHipHeight, colliderGeo.center.z);
        colliderGeo.height = 2 * startingHipHeight;
    }

    // Update is called once per frame
    void Update()
    {
        
        colliderGeo.center = 
            new Vector3(colliderGeo.center.x, hips.transform.position.y, colliderGeo.center.z);
        colliderGeo.height = (hips.transform.position.y - 
            Mathf.Min(leftAnkle.transform.position.y, rightAnkle.transform.position.y) + approximateAnkleRadius) * 2;
       // colliderGeo.center =
       //     new Vector3(colliderGeo.center.x, StartingColliderHeight - gameObject.transform.position.y +
       //         startingCharacterHeight, colliderGeo.center.z);
    }


    /*public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }*/

    /*public Transform FindGrandChildWithTag(Transform trans, string match)
    {
        Transform result = null;
        foreach (Transform child in trans)
        {
            
            if (child.tag.Equals(match))
            {
                result = child;
            } else
            {
                result = FindGrandChildWithTag(child, match);
            }
            
        }
        return result;
    }*/

    void DisplayChildren(Transform trans, string tag, Transform result)
    {
        
        foreach (Transform child in trans)
        {
            if (child.CompareTag(tag))
            {
                Debug.Log(child.tag);
                result = child;
                return;
            }
            //Debug.Log(child.tag);
            if (child.childCount > 0)
            {
                DisplayChildren(child, tag, result);
            }
            
        }
        
    }
}


