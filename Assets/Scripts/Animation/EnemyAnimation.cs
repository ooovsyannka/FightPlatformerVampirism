using UnityEngine;

[RequireComponent(typeof(AnimationsHolder), typeof(SpriteRenderer))]

public class EnemyAnimation : MonoBehaviour
{
    private AnimationsHolder _animationsHolder;

    private void Awake()
    {
        _animationsHolder = GetComponent<AnimationsHolder>();
    }

    public void PlayAnimation(bool isMove, bool isDie)
    {
        _animationsHolder.PlayMoveAnimation(isMove);
        _animationsHolder.PlayDieAnimation(isDie);
    }
}