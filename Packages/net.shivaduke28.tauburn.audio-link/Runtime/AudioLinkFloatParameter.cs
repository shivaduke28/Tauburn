using Tauburn.Core;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRCAudioLink;

namespace Tauburn.AudioLink
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn AudioLinkFloatParameter")]
    public sealed class AudioLinkFloatParameter : FloatParameterHandler
    {
        [SerializeField] AudioLinkParameterType type;
        AudioLinkController audioLinkController;

        public void Initialize(AudioLinkController audioLinkController)
        {
            this.audioLinkController = audioLinkController;
        }

        public override void Set(float value)
        {
            Slider slider;
            switch (type)
            {
                case AudioLinkParameterType.Gain:
                    slider = audioLinkController.gainSlider;
                    break;
                case AudioLinkParameterType.Treble:
                    slider = audioLinkController.trebleSlider;
                    break;
                case AudioLinkParameterType.Bass:
                    slider = audioLinkController.bassSlider;
                    break;
                case AudioLinkParameterType.FadeLength:
                    slider = audioLinkController.fadeLengthSlider;
                    break;
                case AudioLinkParameterType.FadeExpFalloff:
                    slider = audioLinkController.fadeExpFalloffSlider;
                    break;
                case AudioLinkParameterType.X0:
                    slider = audioLinkController.x0Slider;
                    break;
                case AudioLinkParameterType.X1:
                    slider = audioLinkController.x1Slider;
                    break;
                case AudioLinkParameterType.X2:
                    slider = audioLinkController.x2Slider;
                    break;
                case AudioLinkParameterType.X3:
                    slider = audioLinkController.x3Slider;
                    break;
                case AudioLinkParameterType.Threshold0:
                    slider = audioLinkController.threshold0Slider;
                    break;
                case AudioLinkParameterType.Threshold1:
                    slider = audioLinkController.threshold1Slider;
                    break;
                case AudioLinkParameterType.Threshold2:
                    slider = audioLinkController.threshold2Slider;
                    break;
                case AudioLinkParameterType.Threshold3:
                    slider = audioLinkController.threshold3Slider;
                    break;
                default:
                    return;
            }

            slider.normalizedValue = value;
        }
    }

    public enum AudioLinkParameterType
    {
        Gain,
        Treble,
        Bass,
        FadeLength,
        FadeExpFalloff,
        X0,
        X1,
        X2,
        X3,
        Threshold0,
        Threshold1,
        Threshold2,
        Threshold3,
    }
}
