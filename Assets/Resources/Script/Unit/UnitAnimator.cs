using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private EAnimationType eAnimationType = EAnimationType.Idle;
    private Blackboard blackboard;
    private Coroutine animationCoroutine;

    // 애니메이션 클립들을 저장할 딕셔너리
    private Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void InitAnimationController(Blackboard _blackboard, string _animationControllerName)
    {
        blackboard = _blackboard;
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
            blackboard.isAnimationPlaying = false;
        }

        animator = gameObject.GetComponent<Animator>();
        var controller = Resources.Load<RuntimeAnimatorController>($"Animation/UnitAnimation/{_animationControllerName}/{_animationControllerName}");
        animator.runtimeAnimatorController = controller;
        animator.Play(EAnimationType.Idle.ToString());

        // 애니메이션 클립 저장
        animationClips.Clear();
        foreach (var clip in controller.animationClips)
        {
            animationClips[clip.name] = clip;
        }
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = ConstValue.Layer_Character - Mathf.CeilToInt(transform.position.y * 10);
    }

    public void SetAnimation(EAnimationType _eAnimation, float _animationSpeed = 1.0f)
    { 
        if(animator == null) 
        { 
            return; 
        }
        if(eAnimationType == _eAnimation) // 같은 애니메이션이 들어오면 returb
        {
            return;
        }

        eAnimationType = _eAnimation;
        animator.speed = _animationSpeed;
        animator.Play(_eAnimation.ToString());

        if (_eAnimation == EAnimationType.Attack1 || _eAnimation == EAnimationType.Attack2)
        {
            blackboard.isAnimationPlaying = true;

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
            animationCoroutine = StartCoroutine(CheckAnimationEnd());
        }
    }

    private IEnumerator CheckAnimationEnd()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        blackboard.isAnimationPlaying = false;
        animationCoroutine = null;
    }

    public void CancelCurrentAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        blackboard.isAnimationPlaying = false;
        eAnimationType = EAnimationType.Death;
        animator.Play(EAnimationType.Death.ToString());
    }

    public EAnimationType GetEAnimationType()
    {
        return eAnimationType;
    }

    public Dictionary<string, AnimationClip> GetAnimationClips()
    {
        return animationClips;
    }

    public void OnEvent_Attack1()
    {
        if (blackboard != null && blackboard.myGameObject != null)
        {
            blackboard.myGameObject.SendMessage("Attack1", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnEvent_Skill()
    {
        if (blackboard != null && blackboard.myGameObject != null)
        {
            blackboard.myGameObject.SendMessage("Skill", SendMessageOptions.DontRequireReceiver);
        }
    }
}
