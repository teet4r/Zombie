using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorID
{
    public class Int
    {
        public static readonly int MOVE = Animator.StringToHash("Move");
    }

    public class Trigger
    {
        public static readonly int RELOAD = Animator.StringToHash("Reload");
        public static readonly int DIE = Animator.StringToHash("Die");
    }
}
