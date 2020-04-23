using UniRx;
using Zenject;

namespace Room
{
    public class Lamp
    {
        public readonly ReactiveProperty<bool> IsTurnOn;

        [Inject]
        public Lamp(Settings settings)
        {
            IsTurnOn = new ReactiveProperty<bool>(settings.LampIsOn);
        }
    }
}