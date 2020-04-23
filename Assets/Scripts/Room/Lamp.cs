using UniRx;
using Zenject;

namespace Room
{
    public class Lamp
    {
        public readonly ReactiveProperty<bool> IsTurnOn;

        [Inject]
        public Lamp(MySettings settings)
        {
            IsTurnOn = new ReactiveProperty<bool>(settings.LampIsOn);
        }
    }
}