using ServerTCP;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Room
{
    public class RoomGameInstaller : MonoInstaller
    {
        public async override void InstallBindings()
        {
            Container.Bind<MySettings>().AsCached();

            var settings = Container.Resolve<MySettings>();

            Container.Bind<LampPresenter>().AsCached();
            Container.Bind<Lamp>().AsCached();

            Container.Bind<Explosion>().AsCached();
            Container.Bind<ExplosionPresenter>().AsCached();

            Container.Bind<Server>().AsCached().WithArguments(settings.Port, Debug.unityLogger);

            var lamp = Container.Resolve<Lamp>();
            var explosion = Container.Resolve<Explosion>();
            var server = Container.Resolve<Server>();

            server.ReplySubject
                .Where(TypeMessage.IsLightSwitchMsg)
                .Select(TypeMessage.ConvertLightSwitchMsg)
                .Subscribe(state => lamp.IsTurnOn.SetValueAndForceNotify(state));

            server.ReplySubject
                .Where(TypeMessage.IsBlowUpMessage)
                .Subscribe(_ => explosion.BlowUp.Execute());

            var cts = new CancellationTokenSource();
            await server.ListenForIncommingRequests(cts.Token);
        }
    }
}