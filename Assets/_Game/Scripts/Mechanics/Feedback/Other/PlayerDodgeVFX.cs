using System.Collections;
using UnityEngine;

public class PlayerDodgeVFX : MonoBehaviour
{
    [SerializeField] Transform dodgeVFXHolder;

    [Header("Adjustments")]
    [Range(1, 3)]
    [SerializeField] float appearStartScale = 1.5f;
    [Range(1, 50)]
    [SerializeField] float appearSpeed = 5;

    [Space(5)]
    [Range(0, 0.99f)]
    [SerializeField] float disappearEndScale = 0.1f;
    [Range(1, 35)]
    [SerializeField] float disappearSpeed = 5;

    private void Awake()
    {
        if (!dodgeVFXHolder)
        {
            Debug.LogWarning("No dodge VFX hooked up to " + name);
            this.enabled = false;
            return;
        }

        dodgeVFXHolder.gameObject.SetActive(false);
    }

    public void ShowDodgeVFX()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateShowingDodgeVFX());
    }
    IEnumerator AnimateShowingDodgeVFX()
    {
        dodgeVFXHolder.gameObject.SetActive(true);
        float curScale = appearStartScale;
        while (curScale > 1.01f)
        {
            dodgeVFXHolder.localScale = Vector3.one * curScale;
            curScale = Mathf.Lerp(curScale, 1, 0.01f * appearSpeed);
            yield return null;
        }
        dodgeVFXHolder.localScale = Vector3.one;
    }

    public void HideDodgeVFX()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateHidingDodgeVFX());
    }
    IEnumerator AnimateHidingDodgeVFX()
    {
        dodgeVFXHolder.gameObject.SetActive(true);
        float curScale = 1;
        while (curScale > disappearEndScale + 0.01f)
        {
            dodgeVFXHolder.localScale = Vector3.one * curScale;
            curScale = Mathf.SmoothStep(curScale, disappearEndScale, 0.01f * disappearSpeed);
            //curScale -= Time.deltaTime * 3 * disappearSpeed;
            yield return null;
        }
        dodgeVFXHolder.gameObject.SetActive(false);
    }
}
