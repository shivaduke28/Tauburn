using UdonSharp;
using UnityEngine;
using VRCAudioLink;

namespace Tauburn.AudioLink
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), AddComponentMenu("Tauburn AudioLinkPreset")]
    public sealed class AudioLinkPreset : UdonSharpBehaviour
    {
        [SerializeField, Range(0, 2)] float gain = 1f;
        [SerializeField, Range(0, 2)] float treble = 1f;
        [SerializeField, Range(0, 2)] float bass = 1f;
        [SerializeField, Range(0, 1)] float fadeLength = 0.25f;
        [SerializeField, Range(0, 1)] float fadeExpFalloff = 0.75f;
        [Space]
        [SerializeField, Range(0, 0.168f)] float x0 = 0.0f;
        [SerializeField, Range(0.242f, 0.387f)] float x1 = 0.25f;
        [SerializeField, Range(0.461f, 0.628f)] float x2 = 0.5f;
        [SerializeField, Range(0.704f, 0.953f)] float x3 = 0.75f;
        [Space]
        [SerializeField, Range(0, 1)] float threshold0 = 0.45f;
        [SerializeField, Range(0, 1)] float threshold1 = 0.45f;
        [SerializeField, Range(0, 1)] float threshold2 = 0.45f;
        [SerializeField, Range(0, 1)] float threshold3 = 0.45f;

        AudioLinkController audioLinkController;


        public void Initialize(AudioLinkController audioLinkController)
        {
            this.audioLinkController = audioLinkController;
        }

        public void Load()
        {
            audioLinkController.gainSlider.value = gain;
            audioLinkController.trebleSlider.value = treble;
            audioLinkController.bassSlider.value = bass;
            audioLinkController.fadeLengthSlider.value = fadeLength;
            audioLinkController.fadeExpFalloffSlider.value = fadeExpFalloff;

            audioLinkController.x0Slider.value = x0;
            audioLinkController.x1Slider.value = x1;
            audioLinkController.x2Slider.value = x2;
            audioLinkController.x3Slider.value = x3;

            audioLinkController.threshold0Slider.value = threshold0;
            audioLinkController.threshold1Slider.value = threshold1;
            audioLinkController.threshold2Slider.value = threshold2;
            audioLinkController.threshold3Slider.value = threshold3;
        }

        public void ResetValues()
        {
            gain = 1f;
            treble = 1f;
            bass = 1f;
            fadeLength = 0.25f;
            fadeExpFalloff = 0.75f;
            x0 = 0f;
            x1 = 0.25f;
            x2 = 0.5f;
            x3 = 0.75f;

            threshold0 = 0.45f;
            threshold1 = 0.45f;
            threshold2 = 0.45f;
            threshold3 = 0.45f;
        }
    }
}
