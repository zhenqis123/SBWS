using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitionController : StateMachineBehaviour
{
    private int idleAnimationCount = 6;
    private int walkAnimationCount = 5;
    private int runAnimationCount = 6;
    private int stateCount = 3;

    private int idleAnimationSelect;
    private int walkAnimationSelect;
    private int runAnimationSelect;
    private int stateSelect;

    protected GameObject clone;

    private int xUpperLimit = 24;
    private int zUpperLimit = 12;
    private int xLowerLimit = 0;
    private int zLowerLimit = 0;

    private int y = 0;

    private System.Random random;
    void Start()
    {
        Random.seed = 11;
        //random = new System.Random(1);
    }

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.parent.position =
        //    new Vector3(animator.transform.parent.position.x, y, animator.transform.parent.position.z);
        // clone = Instantiate<GameObject>(animator.transform.parent.gameObject);
        // clone.transform.position =
        //     new Vector3(clone.transform.position.x, y, clone.transform.position.z);

        
        stateSelect = Random.Range(0, stateCount);
        //stateSelect = random.Next(stateCount);
        animator.SetInteger("State Index", stateSelect);
        //anim.SetInteger("State Index", 2);
        switch (stateSelect)
        {
            case 0:
                //Debug.Log("enter 0");
                idleAnimationSelect = Random.Range(0, idleAnimationCount);
                //idleAnimationSelect = random.Next(idleAnimationCount);
                //anim.SetInteger("State Index", stateSelect);
                animator.SetInteger("Idle Index", idleAnimationSelect);
                //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                break;

            case 1:
                //Debug.Log("enter 1");
                walkAnimationSelect = Random.Range(0, walkAnimationCount);
                //walkAnimationSelect = random.Next(walkAnimationCount);
                //anim.SetInteger("State Index", stateSelect);
                animator.SetInteger("Walk Index", walkAnimationSelect);
                //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                break;

            case 2:
                //Debug.Log("enter 2");
                runAnimationSelect = Random.Range(0, runAnimationCount);
                //runAnimationSelect = random.Next(runAnimationCount);
                //anim.SetInteger("State Index", stateSelect);
                animator.SetInteger("Run Index", runAnimationSelect);
                //gameObject.transform.Rotate(0, Random.Range(0, 360), 0);
                break;

            case 3:
                break;

        }

        animator.rootPosition = new Vector3(animator.rootPosition.x, y, animator.rootPosition.z);
        //animator.rootRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
