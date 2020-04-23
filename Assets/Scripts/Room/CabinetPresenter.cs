using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CabinetPresenter : MonoBehaviour
{
    public float Magnitude;

    private void Start()
    {
        GameObject lastValue = null;
        gameObject.OnParticleCollisionAsObservable()
            .TakeUntilDestroy(this)
         .Subscribe(obj => lastValue = obj);

        Observable.Interval(TimeSpan.FromMilliseconds(100))
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                if (lastValue == null)
                    return;

                var t = lastValue;
                lastValue = null;
                var force = transform.position - t.transform.position;
                force.Normalize();
                gameObject.GetComponent<Rigidbody>().AddForce(force * Magnitude);
            });
    }
}