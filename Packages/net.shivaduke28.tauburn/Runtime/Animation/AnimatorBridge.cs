using UdonSharp;
using UnityEngine;

namespace Tauburn.Animation
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn AnimatorBridge")]
    public sealed class AnimatorBridge : UdonSharpBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationIntParameter[] intParameters;
        [SerializeField] AnimationFloatParameter[] floatParameters;

        void Start()
        {
            foreach (var intParameter in intParameters)
            {
                intParameter.Initialize(animator);
            }

            foreach (var floatParameter in floatParameters)
            {
                floatParameter.Initialize(animator);
            }
        }
    }
}
