using Tauburn.Core;
using UnityEngine;
using VRCAudioLink;

namespace Tauburn.AudioLink
{
    public class AudioLinkParameterController : IntParameterHandler
    {
        [SerializeField] AudioLinkPreset[] audioLinkPresets;
        [SerializeField] AudioLinkFloatParameter[] audioLinkFloatParameters;
        [SerializeField] AudioLinkController audioLinkController;

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
