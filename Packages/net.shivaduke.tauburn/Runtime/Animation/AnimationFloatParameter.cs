using Tauburn.Core;
using UdonSharp;
using UnityEngine;

namespace Tauburn.Animation
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public sealed class AnimationFloatParameter : FloatParameterHandler
    {
        [SerializeField] string parameterName;
        Animator animator;
        int parameterId;

        public void Initialize(Animator animator)
        {
            this.animator = animator;
            parameterId = Animator.StringToHash(parameterName);
        }

        public override void Set(float value)
        {
            animator.SetFloat(parameterId, value);
        }
    }
}
