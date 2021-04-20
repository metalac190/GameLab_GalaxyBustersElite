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

    public Text text;
    public Vector2 offsetPerSecond;

    public string DisplayedText {
        get { return text.text; }
        set { text.text = value; }
    }

    Vector2 offset = Vector2.zero;
    RectTransform rectTransform;
	private void Start()
	{
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(TweenAlpha(alphaCurve, fadeDuration));
	}

	void LateUpdate()
    {
        if (Vector3.Dot(objTransform.position - cam.transform.position, cam.transform.forward) < 0)
		{
            print("destroy");
            Destroy(gameObject);
		}
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, objTransform.position) + offset;
        offset += offsetPerSecond * Time.deltaTime;
    }

    IEnumerator TweenAlpha(AnimationCurve curve, float duration)
	{
        float timer = 0;
        Color newColor = text.color;
        while (timer < duration)
		{
            float value = curve.Evaluate(Mathf.Lerp(0, duration, timer));

            newColor.a = value;
            text.color = newColor;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
		}
	}
}
