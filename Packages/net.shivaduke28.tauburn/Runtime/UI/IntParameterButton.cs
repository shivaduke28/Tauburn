using Tauburn.Core;
using UdonSharp;
using UnityEngine;

namespace Tauburn.UI
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), AddComponentMenu("Tauburn IntParameterButton")]
    public sealed class IntParameterButton : IntParameterView
    {
        [SerializeField] int parameterValue;
        [SerializeField] Animator animator;
        readonly int isOnId = Animator.StringToHash("isOn");
        IntParameterHandler parameterHandler;

        public override void UpdateView(int value)
        {
            animator.SetBool(isOnId, parameterValue == value);
        }

        // called from Button
        public void OnClick()
        {
            parameterHandler.Set(parameterValue);
        }

        // IntParameterProvider
        public override void Register(IntParameterHandler parameterHandler)
        {
            this.parameterHandler = parameterHandler;
        }
    }
}
