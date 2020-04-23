using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ControlPanel
{
    public class LightSwitchPresenter : MonoBehaviour
    {
        [Inject]
        public void Construct(LightSwitch lightSwitch)
        {
            var animator = GetComponent<Animator>();

            lightSwitch.IsTurnOn
                .Subscribe(isTurnOn => animator.Play(isTurnOn ? "turnOn" : "turnOff"));

            gameObject.OnMouseDownAsObservable()
              .Subscribe(_ => lightSwitch.IsTurnOn.SetValueAndForceNotify(!lightSwitch.IsTurnOn.Value));
        }
    }
}