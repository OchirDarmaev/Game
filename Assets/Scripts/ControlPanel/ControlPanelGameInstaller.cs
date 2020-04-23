using Room;
using ServerTCP;
using System;
using System.Net;
using UniRx;
using UnityEngine;
using Zenject;

namespace ControlPanel
{
    public class ControlPanelGameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Settings>().AsCached();
            Container.Bind<Client>().ToSelf().AsCached();

            Container.Bind<DetonatorPresenter>().AsCached();
            Container.Bind<Detonator>().AsCached();

            Container.Bind<LightSwitchPresenter>().AsCached();
            Container.Bind<LightSwitch>().AsCached().WithArguments(false);

            var detonator = Container.Resolve<Detonator>();
            var lightSwitch = Container.Resolve<LightSwitch>();
            var settings = Container.Resolve<Settings>();

            var clientObs = Observer.Create<byte[]>(async data =>
            {
                using (var client = new Client(settings.Address, settings.Port, Debug.unityLogger))
                {
                    await client.ConnectAsync();
                    await client.SendAsync(data);
                }
            });

            lightSwitch.IsTurnOn
                .Select(TypeMessage.BuildLightSwitchMsg)
                .Do(msg => Debug.Log($"{nameof(lightSwitch)} send to server {BitConverter.ToString(msg)}"))
                .Subscribe(clientObs.OnNext);

            detonator.Press
                .Select(x => TypeMessage.BuildBlowUpMsg())
                .Do(msg => Debug.Log($"{nameof(detonator)}send to server {BitConverter.ToString(msg)}"))
                .Subscribe(clientObs.OnNext);
        }
    }
}