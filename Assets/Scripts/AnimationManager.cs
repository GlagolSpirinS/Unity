using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationIdle()
    {
        animator.SetInteger("Animation", 0);
    }

    public void SetAnimationWalk()
    {
        animator.SetInteger("Animation", 1);
    }

    public void SetAnimationRun()
    {
        animator.SetInteger("Animation", 2);
    }

    public void SetAnimationFire()
    {
        animator.SetTrigger("Fire");
    }

    public void SetAnimationReload()
    {
        animator.SetTrigger("Reload");
    }
}