using UnityEngine;

[RequireComponent(typeof(AnimationsHolder), typeof(SpriteRenderer))]

public class PlayerAnimation : MonoBehaviour
{
    private AnimationsHolder _animationsHolder;

    private void Awake()
    {
        _animationsHolder = GetComponent<AnimationsHolder>();
    }

    public void PlayAnimation(bool isMove, bool isDie, bool isDash)
    {
        _animationsHolder.PlayMoveAnimation(isMove);
        _animationsHolder.PlayDashAnimation(isDash);
        _animationsHolder.PlayDieAnimation(isDie);
    }
}