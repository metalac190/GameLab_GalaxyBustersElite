using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;

    public float curShakeAmt;
    const float SHAKE_ENDING_SMOOTHNESS = 20;
    const float NOT_SHAKING_THRESHOLD = 0.01f;

    private void Start()
    {
        instance = this;
    }

    private void Update() => PerformShakingEachFrame();
    private void PerformShakingEachFrame()
    {
        if (GameManager.gm.Paused) return;

        if (curShakeAmt > 0)
            transform.localPosition = Vector2.up * Random.Range(-curShakeAmt, curShakeAmt) + Vector2.right * Random.Range(-curShakeAmt, curShakeAmt);
        else
            transform.localPosition = Vector2.zero;
    }

    private void FixedUpdate() => DecrementShakeIfStoppedShaking();
    private void DecrementShakeIfStoppedShaking()
    {
        if (curShakeAmt > NOT_SHAKING_THRESHOLD)
        {
            curShakeAmt = Mathf.Lerp(curShakeAmt, 0, 1 / SHAKE_ENDING_SMOOTHNESS);
            if (curShakeAmt <= NOT_SHAKING_THRESHOLD)
                curShakeAmt = 0;
        }
    }

    public void Shake(float setShakeAmt) { curShakeAmt = setShakeAmt; }
}
