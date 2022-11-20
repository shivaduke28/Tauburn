using Tauburn.Core;
using UdonSharp;
using UnityEngine;

namespace Tauburn.Animation
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn AnimationIntParameter")]
    public sealed class AnimationIntParameter : IntParameterHandler
    {
        [SerializeField] string parameterName;
        Animator animator;
        int parameterId;

        public void Initialize(Animator animator)
        {
            this.animator = animator;
            parameterId = Animator.StringToHash(parameterName);
        }

        public override void Set(int value)
        {
            animator.SetInteger(parameterId, value);
        }
    }
}
