using Tauburn.Core;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Tauburn.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn FloatParameterSlider")]
    public sealed class FloatParameterSlider : FloatParameterView
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI valueText;
        [SerializeField] Animator animator;
        readonly int valueId = Animator.StringToHash("value");

        FloatParameterHandler parameterHandler;

        public override void UpdateView(float value)
        {
            valueText.text = value.ToString("N");
            animator.SetFloat(valueId, value);
            slider.SetValueWithoutNotify(value);
        }

        // called from Slider
        public void OnValueChange()
        {
            parameterHandler.Set(slider.value);
        }

        // FloatParameterProvider
        public override void Register(FloatParameterHandler parameterHandler)
        {
            this.parameterHandler = parameterHandler;
        }
    }
}
