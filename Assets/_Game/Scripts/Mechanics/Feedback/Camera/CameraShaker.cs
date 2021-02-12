using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    float shakeAmt;
    const float SHAKE_ENDING_SMOOTHNESS = 20;
    const float NOT_SHAKING_THRESHOLD = 0.01f;


    private void Update() => Shake();
    private void Shake()
    {
        if (shakeAmt > 0)
            transform.localPosition = Vector2.up * Random.Range(-shakeAmt, shakeAmt) + Vector2.right * Random.Range(-shakeAmt, shakeAmt);
        else
            transform.localPosition = Vector2.zero;
    }

    private void FixedUpdate() => DecrementShakeIfStoppedShaking();
    private void DecrementShakeIfStoppedShaking()
    {
        if (shakeAmt > NOT_SHAKING_THRESHOLD)
        {
            shakeAmt = Mathf.Lerp(shakeAmt, 0, 1 / SHAKE_ENDING_SMOOTHNESS);
            if (shakeAmt <= NOT_SHAKING_THRESHOLD)
                shakeAmt = 0;
        }
    }

    public void Shake(float setShakeAmt) { shakeAmt = setShakeAmt; }
}
