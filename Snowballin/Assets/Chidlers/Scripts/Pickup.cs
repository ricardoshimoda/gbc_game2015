using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : StateMachineBehaviour
{
    [SerializeField] NavMeshAI script;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ChidlerFunctions Pickedup = animator.GetComponent<ChidlerFunctions>();
        //Pickedup.PickUp();
        //GameObject tempSnowball = GameObject.Instantiate(bullet, rotation.position, rotation.rotation);
        // animator.SetInteger("State", 0);
        ChidlerFunctions Pickedup = animator.GetComponent<ChidlerFunctions>();
        Pickedup.PickUp();
        //script.SpawnBullet();
        //  Debug.Log(count);
        // else count = 0;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
