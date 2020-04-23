using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace ControlPanel
{
    public class DetonatorPresenter : MonoBehaviour
    {
        [Inject]
        public void Construct(Detonator detonator)
        {
            var animator = GetComponent<Animator>();

            detonator.Press.Subscribe(_ => animator.Play("ButtonPressDown"));

            gameObject.OnMouseDownAsObservable().Subscribe(_ => detonator.Press.Execute());
            gameObject.OnMouseExitAsObservable().Subscribe(_ => animator.Play("ButtonUp"));
            gameObject.OnMouseUpAsObservable().Subscribe(_ => animator.Play("ButtonUp"));
        }
    }
}