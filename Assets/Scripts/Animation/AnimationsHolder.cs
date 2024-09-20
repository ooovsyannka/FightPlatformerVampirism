using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]

public class AnimationsHolder : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayMoveAnimation(bool isMove)
    {
        _animator.SetBool(AnimationData.Params.IsMove, isMove);
    }

    public void PlayDieAnimation(bool isDie)
    {
        _animator.SetBool(AnimationData.Params.IsDie, isDie);
    }

    public void PlayDashAnimation(bool isDash)
    {
        _animator.SetBool(AnimationData.Params.IsDash, isDash);
    }

    public void PlayAttackAnimation(bool isAttack)
    {
        _animator.SetBool(AnimationData.Params.IsAttack, isAttack);
    }

    public void PlayReloadAnimation(bool isReload)
    {
        _animator.SetBool(AnimationData.Params.IsReload, isReload);
    }
}