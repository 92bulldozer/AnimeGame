using System.Collections;
using System.Collections.Generic;
using ECM.Controllers;
using EJ;
using UnityEngine;

public class AnimeCharacterController : BaseCharacterController
{
    protected override void Animate()
    {
        // If there is no animator, return

        if (animator == null)
            return;
        
        //movement.velocity.magnitude.Log();
        float characterAnimationSpeed = movement.velocity.magnitude.Remap(0, 2.5f, 0, 1);
        animator.SetFloat("MoveSpeed",characterAnimationSpeed,0.1f,Time.deltaTime);

        // Compute move vector in local space

        // var move = transform.InverseTransformDirection(moveDirection);
        //
        // // Update the animator parameters
        //
        // var forwardAmount = move.z;
        //
        // animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        // animator.SetFloat("Turn", Mathf.Atan2(move.x, move.z), 0.1f, Time.deltaTime);
        //
        // animator.SetBool("OnGround", movement.isGrounded);
        //
        // animator.SetBool("Crouch", isCrouching);
        //
        // if (!movement.isGrounded)
        //     animator.SetFloat("Jump", movement.velocity.y, 0.1f, Time.deltaTime);
        //
        // // Calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // // (This code is reliant on the specific run cycle offset in our animations,
        // // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        //
        // var runCycle = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1.0f);
        // var jumpLeg = (runCycle < 0.5f ? 1.0f : -1.0f) * forwardAmount;
        //
        // if (movement.isGrounded)
        //     animator.SetFloat("JumpLeg", jumpLeg);
    }
}
