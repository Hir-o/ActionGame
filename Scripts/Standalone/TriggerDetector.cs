
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour, IUngrabbable
{
    public UnityEvent<Collider> onTriggerEnter;
    public UnityEvent<Collider> onTriggerStay;
    public UnityEvent<Collider> onTriggerExit;

    private void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke(other);
    private void OnTriggerStay(Collider other) => onTriggerStay?.Invoke(other);
    private void OnTriggerExit(Collider other) => onTriggerExit?.Invoke(other);
}
