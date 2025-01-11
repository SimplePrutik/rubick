using System;
using TMPro;
using UniRx;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private TMP_Text content;

    private IDisposable lifeCycleDisposable;

    public void Init(string value, Transform rootObject)
    {
        content.text = value;
        content.fontSize = Vector3.Distance(transform.position, rootObject.position) * 0.5f;

        lifeCycleDisposable = Observable
            .EveryUpdate()
            .Subscribe(_ =>
            {
                transform.LookAt(rootObject);
                content.fontSize = Vector3.Distance(transform.position, rootObject.position) * 0.5f;
                transform.position += Vector3.one * Time.deltaTime;
            });

        Observable
            .Timer(TimeSpan.FromSeconds(2f))
            .Subscribe(_ => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        lifeCycleDisposable?.Dispose();
    }
}