using Tauburn.Core;
using UdonSharp;
using UnityEngine;
using VRCAudioLink;

namespace Tauburn.AudioLink
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn AudioLinkBridge")]
    public sealed class AudioLinkBridge : IntParameterHandler
    {
        [SerializeField] AudioLinkController audioLinkController;
        [Space] [SerializeField] AudioLinkPreset[] audioLinkPresets;
        [SerializeField] AudioLinkFloatParameter[] audioLinkFloatParameters;
        [SerializeField] FloatParameterProvider[] floatParameterProviders;

        void Start()
        {
            foreach (var audioLinkPreset in audioLinkPresets)
            {
                audioLinkPreset.Initialize(audioLinkController);
            }
            foreach (var audioLinkFloatParameter in audioLinkFloatParameters)
            {
                audioLinkFloatParameter.Initialize(audioLinkController);
            }

            for (var i = 0; i < Mathf.Min(floatParameterProviders.Length, floatParameterProviders.Length); i++)
            {
                floatParameterProviders[i].Register(audioLinkFloatParameters[i]);
            }
        }

        public override void Set(int value)
        {
            if (0 <= value && value < audioLinkPresets.Length)
            {
                audioLinkPresets[value].Load();
            }
        }
    }
}
