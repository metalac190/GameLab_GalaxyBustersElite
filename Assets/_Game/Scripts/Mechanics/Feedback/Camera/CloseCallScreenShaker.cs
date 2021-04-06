using UnityEngine;
using UnityEngine.Events;

public class CloseCallScreenShaker : MonoBehaviour
{
    [SerializeField] UnityEvent OnCloseCall;

    private void OnTriggerEnter(Collider other)
    {
        if (CameraShaker.instance.curShakeAmt == 0)
            OnCloseCall.Invoke();
    }
}
