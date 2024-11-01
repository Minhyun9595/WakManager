using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animation animation;
    public Animator animator;
    void Start()
    {
        animation = gameObject.GetComponent<Animation>();
        animator = gameObject.GetComponent<Animator>();

        animator.Play("Medieval_Warrior_Attack2");
    }

    void Update()
    {
        
    }
}
