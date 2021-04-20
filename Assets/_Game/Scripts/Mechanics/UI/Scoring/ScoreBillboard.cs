using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBillboard : MonoBehaviour
{
    public AnimationCurve alphaCurve;
    public float fadeDuration;
    [HideInInspector] public Camera cam;
    [HideInInspector] public Transform objTransform;

    public CanvasGroup canvGroup;
    public Text text;
    public RectTransform optionalLine;
    public Vector2 offsetPerSecond;
    public bool fade;
    public bool scale;

    public float fadeOutDistance;
    public AnimationCurve fadeOutCurve;
    public float fadeOutDuration;
    Coroutine fadeOutCoroutine;

    public string DisplayedText {
        get { return text.text; }
        set { text.text = value; }
    }

    float scaleF;
    Vector2 offset = Vector2.zero;
    RectTransform rectTransform;
	private void Start()
	{
        rectTransform = GetComponent<RectTransform>();
        if (fade) StartCoroutine(TweenAlpha(alphaCurve, fadeDuration));
	}

	void LateUpdate()
    {
        if (Vector3.Dot(objTransform.position - cam.transform.position, cam.transform.forward) < 0)
		{
            Destroy(gameObject);
		}
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, objTransform.position) + offset;
        offset += offsetPerSecond * Time.deltaTime;
        if (scale)
		{
            scaleF = 1 / ((objTransform.position - cam.transform.position).magnitude / 15);
            transform.localScale = new Vector3(scaleF, scaleF, scaleF);
        }

        if ((objTransform.position - cam.transform.position).sqrMagnitude < fadeOutDistance * fadeOutDistance) {
            if (fadeOutCoroutine == null) fadeOutCoroutine = StartCoroutine(TweenAlpha(fadeOutCurve, fadeOutDuration));
		}
    }

    IEnumerator TweenAlpha(AnimationCurve curve, float duration)
	{
        float timer = 0;
        while (timer < duration)
		{
            float value = curve.Evaluate(timer/duration);

            canvGroup.alpha = value;

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
		}
        canvGroup.alpha = curve.Evaluate(1);
	}
}
