using UniRx;
using UnityEngine;
using Zenject;

namespace Room
{
    public class LampPresenter : MonoBehaviour
    {
        public GameObject DomeOff;
        public GameObject DomeOn;
        public GameObject LampLight;

        [Inject]
        public void Construct(Lamp lamp)
        {
            lamp.IsTurnOn.Subscribe(LightSwitch);
        }

        private void LightSwitch(bool mode)
        {
            if (mode)
            {
                LampLight.SetActive(true);
                DomeOff.SetActive(false);
                DomeOn.SetActive(true);
            }
            else
            {
                LampLight.SetActive(false);
                DomeOff.SetActive(true);
                DomeOn.SetActive(false);
            }
        }
    }
}