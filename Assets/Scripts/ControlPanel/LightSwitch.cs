using UniRx;

namespace ControlPanel
{
    public class LightSwitch
    {
        public readonly ReactiveProperty<bool> IsTurnOn;

        public LightSwitch(bool mode)
        {
            IsTurnOn = new ReactiveProperty<bool>(mode);
        }
    }
}