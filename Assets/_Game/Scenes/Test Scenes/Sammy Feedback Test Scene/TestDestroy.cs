using UnityEngine;
using UnityEngine.Events;

public class TestDestroy : MonoBehaviour
{
    public UnityEvent OnDestroyed;

    [ContextMenu("Destroy Self")]
    public void DestroySelf()
    {
        OnDestroyed.Invoke();
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
    }
}
