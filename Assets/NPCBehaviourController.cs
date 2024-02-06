using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviourController : MonoBehaviour
{

    public int Pedes_ID;

    private float xUpperLimit = 28f;
    private float zUpperLimit = -0.5f;
    private float xLowerLimit = 2f;
    private float zLowerLimit = -14f;

    private int y = 0;


    // Start is called before the first frame update
    void Start()
    {
        //  anim = GetComponent<Animator>();
        //  idleAnimationSelect = Random.Range(0, idleAnimationCount);
        //  walkAnimationSelect = Random.Range(0, walkAnimationCount);
        //  runAnimationSelect = Random.Range(0, runAnimationCount);
        //  stateSelect = Random.Range(0, stateCount);

        // Each character starts with a random idle animation
        //anim.SetInteger("State Index", 0);
        //anim.SetInteger("Idle Index", idleAnimationSelect);

        //duration = Random.Range(50, 70);

        // Make character start at a random place within the area defined
        Random.seed = 10 + Pedes_ID;
        gameObject.transform.position = 
            new Vector3(Random.Range(xLowerLimit, xUpperLimit), y, Random.Range(zLowerLimit, zUpperLimit));
        gameObject.transform.Rotate(0, Random.Range(0, 360), 0);

    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.transform.position.x < xLowerLimit || gameObject.transform.position.x > xUpperLimit)
        {
            turnBack();
            
        }
        if (gameObject.transform.position.z < zLowerLimit || gameObject.transform.position.z > zUpperLimit)
        {
            turnBack();
            
        }

/*
        //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
        // {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime == 0)
        {
            Debug.Log("start anim");
            gameObject.transform.position =
                new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }
            // {
            

            stateSelect = Random.Range(0, stateCount);
            anim.SetInteger("State Index", stateSelect);
            //anim.SetInteger("State Index", 2);
            switch (stateSelect)
            {
                case 0:
                    //Debug.Log("enter 0");
                    idleAnimationSelect = Random.Range(0, idleAnimationCount);
                    //anim.SetInteger("State Index", stateSelect);
                    anim.SetInteger("Idle Index", idleAnimationSelect);
                    //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                    break;

                case 1:
                    //Debug.Log("enter 1");
                    walkAnimationSelect = Random.Range(0, walkAnimationCount);
                    //anim.SetInteger("State Index", stateSelect);
                    anim.SetInteger("Walk Index", walkAnimationSelect);
                    //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                    break;

                case 2:
                    //Debug.Log("enter 2");
                    runAnimationSelect = Random.Range(0, runAnimationCount);
                    //anim.SetInteger("State Index", stateSelect);
                    anim.SetInteger("Run Index", runAnimationSelect);
                    //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                    break;

            }*/
           

       // }
        //frameCount++;
    }

    private void turnBack()
    {

        /*Vector3 dir = new Vector3((xUpperLimit - xLowerLimit) / 2, y, (zUpperLimit - zLowerLimit) / 2) - transform.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.RotateAround(transform.position, Vector3.up, angle);*/

        var relativePos = new Vector3((xUpperLimit + xLowerLimit) / 2, y, (zUpperLimit + zLowerLimit) / 2) - transform.position;

        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward);
        
        if (Vector3.Cross(forward, relativePos).y < 0)
        {
            //Do left stuff
            transform.Rotate(Vector3.up, -0.1f * angle);
        }
        else
        {
            //Do right stuff
            transform.Rotate(Vector3.up, 0.1f * angle);
        }

        //transform.rotation = Quaternion.LookRotation(relativePos);

    }
}
