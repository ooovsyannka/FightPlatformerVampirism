using UnityEngine;

[RequireComponent(typeof(AnimationsHolder), typeof(SpriteRenderer), typeof(WeaponRender))]

public class WeaponAnimation : MonoBehaviour 
{
    private AnimationsHolder _animatorHolder;

    private void Start()
    {
        _animatorHolder = GetComponent<AnimationsHolder>();
    }

    public void PlayAnimation(bool isAttack, bool isReload)
    {
        _animatorHolder.PlayAttackAnimation(isAttack);
        _animatorHolder.PlayReloadAnimation(isReload);
    }
}
