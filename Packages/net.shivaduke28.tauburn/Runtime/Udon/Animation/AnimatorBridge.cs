using Tauburn.Core;
using UdonSharp;
using UnityEngine;

namespace Tauburn.Animation
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn AnimatorBridge")]
    public sealed class AnimatorBridge : UdonSharpBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationIntParameter[] intParameters;
        [SerializeField] IntParameterSync[] intParameterSyncs;
        [SerializeField] AnimationFloatParameter[] floatParameters;
        [SerializeField] FloatParameterSync[] floatParameterSyncs;

        void Start()
        {
            for (var i = 0; i < floatParameters.Length; i++)
            {
                var floatParameter = floatParameters[i];
                floatParameter.Initialize(animator);
                floatParameterSyncs[i].Register(floatParameter);
            }

            for (var i = 0; i < intParameters.Length; i++)
            {
                var intParameter = intParameters[i];
                intParameter.Initialize(animator);
                intParameterSyncs[i].Register(intParameter);
            }
        }
    }
}
