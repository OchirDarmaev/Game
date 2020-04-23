using UniRx;
using UnityEngine;
using Zenject;

namespace Room
{
    public class ExplosionPresenter : MonoBehaviour
    {
        [Inject]
        public void Construct(Explosion explosion)
        {
            var particleSystem = gameObject.GetComponent<ParticleSystem>();
            explosion.BlowUp.Subscribe(_ => particleSystem.Play());
        }
    }
}