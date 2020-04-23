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
        public async override void InstallBindings()
        {
            var ip = IPAddress.Parse("127.0.0.1");
            var port = 8080;
            var lightIsOn = false;
            Container.Bind<Settings>().AsCached();
            Container.Bind<Client>().ToSelf().AsCached().WithArguments(ip, port, Debug.unityLogger);

            Container.Bind<DetonatorPresenter>().AsCached();
            Container.Bind<Detonator>().AsCached();

            Container.Bind<LightSwitchPresenter>().AsCached();
            Container.Bind<LightSwitch>().AsCached().WithArguments(lightIsOn);

            var detonator = Container.Resolve<Detonator>();
            var lightSwitch = Container.Resolve<LightSwitch>();
            var client = Container.Resolve<Client>();

            var clientObs = Observer.Create<byte[]>(async data =>
            {
                using (await client.ConnectAsync())
                {
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