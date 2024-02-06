using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetection : MonoBehaviour
{

    public VNectModel22[] VNectModels;

    // limit character's movable area
    public float xLowerLimit = 3.0f;
    public float xUpperLimit = 30.0f;
    public float zLowerLimit = -14.0f;
    public float zUpperLimit = -3.0f;

    // define how close two characters can be
    public float minDistance = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        //while (true)
        //{
            bool check = false;
        
            foreach (var character in VNectModels)
            {
                foreach(var character2 in VNectModels)
                {
                    if (character == character2)
                    {
                        continue;
                    }
                    if (Vector3.Distance(character.JointPoints[PositionIndex22.hip.Int()].Transform.position, character2.JointPoints[PositionIndex22.hip.Int()].Transform.position) < minDistance)
                    {
                        character2.xRandomOffset = Random.Range(xLowerLimit, xUpperLimit); 
                        character2.zRandomOffset = Random.Range(zLowerLimit, zUpperLimit);
                        check = true;
                    }
                }
            }
            /*if (check == false)
            {
                break;
            }*/
        //}
        
    }

    
}
