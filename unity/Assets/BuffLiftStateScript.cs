using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffLiftStateScript : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        animator.SetInteger("to_lift", animator.GetInteger("to_lift") - 1);     // decrement, we have lifted
        /*
        UnityEngine.Debug.LogFormat("Entering done\n" +
                                    "Current clip: {0}\n" +
                                    "Param: {1} len={2}\n" +
                                    "Current: {3} len={4}\n" +
                                    "Next: {5}\n",
                                    animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
                                    stateInfo.fullPathHash, stateInfo.length,
                                    animator.GetCurrentAnimatorStateInfo(0).fullPathHash,
                                    animator.GetCurrentAnimatorStateInfo(0).length,
                                    animator.GetNextAnimatorStateInfo(0).fullPathHash);
        */
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //AnimatorStateInfo upcoming_state = animator.GetCurrentAnimatorStateInfo(0);
        //upcoming_state.speed = Math.Max(animator.GetInteger("to_lift"), 1);
        //animator.speed = System.Math.Max(animator.GetInteger("to_lift"), 1);
                // not allowed to speed up individual states/clips in the animator, have to speed up
                // the whole animator (but that's fine for now...)
                // decided to control this whenever we click, makes it less jarring

        /*
        UnityEngine.Debug.LogFormat("Starting\n" +
                                    "Current clip: {0}\n" +
                                    "Param: {1} len={2}\n" +
                                    "Current: {3} len={4}\n" +
                                    "Next: {5}\n",
                                    animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
                                    stateInfo.fullPathHash, stateInfo.length,
                                    animator.GetCurrentAnimatorStateInfo(0).fullPathHash,
                                    animator.GetCurrentAnimatorStateInfo(0).length,
                                    animator.GetNextAnimatorStateInfo(0).fullPathHash);
        */
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
