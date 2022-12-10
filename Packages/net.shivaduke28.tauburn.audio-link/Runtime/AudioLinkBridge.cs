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
        [SerializeField] IntParameterSync intParameterSync;
        [Space] [SerializeField] AudioLinkPreset[] audioLinkPresets;
        [SerializeField] AudioLinkFloatParameter[] audioLinkFloatParameters;
        [SerializeField] FloatParameterProvider[] floatParameterProviders;

        void Start()
        {
            intParameterSync.Register(this);
            foreach (var preset in audioLinkPresets)
            {
                preset.Initialize(audioLinkController);
            }

            for (var i = 0; i < floatParameterProviders.Length; i++)
            {
                var audioLinkFloatParameter = audioLinkFloatParameters[i];
                audioLinkFloatParameter.Initialize(audioLinkController);
                floatParameterProviders[i].Register(audioLinkFloatParameter);
            }
        }

        public override void Set(int value)
        {
            if (0 <= value && value < audioLinkPresets.Length)
            {
                audioLinkPresets[value].Load();
            }
            else
            {
                Debug.LogError($"Preset index {value} is out of range. AudioLinkBridge has only {audioLinkPresets.Length} presets.");
            }
        }
    }
}
