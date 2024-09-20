using UnityEngine;

public static class AnimationData
{
    public static class Params
    {
        public static readonly int IsMove = Animator.StringToHash(nameof(IsMove));
        public static readonly int IsDash = Animator.StringToHash(nameof(IsDash));
        public static readonly int IsDie = Animator.StringToHash(nameof(IsDie));
        public static readonly int IsAttack = Animator.StringToHash(nameof(IsAttack));
        public static readonly int IsReload = Animator.StringToHash(nameof(IsReload));
    }
}
