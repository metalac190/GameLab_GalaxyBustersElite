using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHUD : MonoBehaviour
{
	static ScoreHUD instance;
	public static ScoreHUD Instance
	{
		get { return instance; }
	}

	[SerializeField] private Transform overlayContainer;

	[SerializeField] private TextMeshProUGUI hudScoreText;
	[SerializeField] private TextMeshProUGUI hudMultiplierText;

	[SerializeField] private GameObject scoreEventContainer;
	[SerializeField] private GameObject scoreEventPrefab;

	[SerializeField] private float secondsToIncrementPerEvent = 0.1f;

	[SerializeField] float timeToMoveEvents = 0.25f;
	[SerializeField] float distanceToMoveEvents = 50f;

	[SerializeField] int maxEventsOnScreen = 5;
	[SerializeField] float eventFadeInTime = 0.5f;
	[SerializeField] float eventMidTime = 2f;
	[SerializeField] float eventFadeOutTime = 0.5f;

	Dictionary<string, string> eventTextDictionary = new Dictionary<string, string>()
	{
		{ "Drone", "Nice shot!" },
		{ "Rammer", "Nice shot!" },
		{ "Minion", "Nice shot!" },
		{ "Bandit", "Nice shot!" },
		{ "Spearhead", "Nice shot!" },
		{ "NearMiss", "Close Call!" },
		{ "Challenge", "Objective Completed!" }
	};

	Queue<ScoreEvent> scoreEventObjects = new Queue<ScoreEvent>();

	Queue<ScoreEvent> scoreEventQueue = new Queue<ScoreEvent>();
	Coroutine processingQueueCoroutine = null;

	private float _hudScore;
	private float hudScore {
		get { return _hudScore; }
		set 
		{
			_hudScore = value;
			hudScoreText.text = _hudScore.ToString("000000");
		}
	}
	private int comboMultiplier;
	[SerializeField] private float colorPulseTime;
	[SerializeField] private float colorPulseAmount;
	[SerializeField] private Color[] multiplierColors = new Color[4];
	private Color multiplierColor;
	private Coroutine pulseCoroutine;

	private Queue<int> incrementQueue = new Queue<int>();
	private Coroutine incrementCoroutine;

	private List<PickupBase> pickupsInLevel;

	[SerializeField] private GameObject scoreBillboardPrefab;
	[SerializeField] private GameObject powerupBillboardPrefab;

	private static GameObject s_scoreBillboardPrefab;

	private Camera cam;
	private static Camera s_cam;
	private static Transform s_overlayContainer;
	private void Awake()
	{
		if (instance == null) instance = this;
		s_scoreBillboardPrefab = scoreBillboardPrefab;
		cam = Camera.main;
		s_cam = cam;
		s_overlayContainer = overlayContainer;
	}

	private void OnDestroy()
	{
		if (instance == this) instance = null;
	}

	private void Start()
	{
		pickupsInLevel = new List<PickupBase>(FindObjectsOfType<PickupBase>()); // TODO change if i want more than weps
		StartCoroutine(CheckPowerups());
	}

	public void PickupEnabled(PickupBase pickup)
	{
		if (pickupsInLevel == null) return;
		if (!pickupsInLevel.Contains(pickup))
		{
			pickupsInLevel.Add(pickup);
		}
	}

	private void OnEnable()
	{
		ScoreSystem.onScoreIncreased += OnScoreAdded;
		ScoreSystem.onMultiplierChanged += OnMultiplierChanged;
		ScoreSystem.onScoreReset += ResetScore;
		//processingQueueCoroutine = StartCoroutine(ProcessScoreEventQueue());
	}

	private void OnDisable()
	{
		ScoreSystem.onScoreIncreased -= OnScoreAdded;
		ScoreSystem.onMultiplierChanged -= OnMultiplierChanged;
		ScoreSystem.onScoreReset -= ResetScore;
		//StopCoroutine(processingQueueCoroutine);
	}

	void OnScoreAdded(string source, int amount)
	{
		incrementQueue.Enqueue(amount);
		if (incrementCoroutine == null) incrementCoroutine = StartCoroutine(IncrementScore(incrementQueue.Peek()));
		//scoreEventQueue.Enqueue(new ScoreEvent(this, eventTextDictionary[source] + " +" + amount.ToString()));
	}

	void OnMultiplierChanged()
	{
		hudMultiplierText.text = _hudScore.ToString("(x" + ScoreSystem.GetComboMultiplier() + ")");
		comboMultiplier = ScoreSystem.GetComboMultiplier();

		multiplierColor = multiplierColors[comboMultiplier-1];
		if (pulseCoroutine != null) StopCoroutine(pulseCoroutine);
		pulseCoroutine = StartCoroutine(MultiplierColorPulse());
	}

	void ResetScore()
	{
		hudScore = 0;
	}

	private IEnumerator MultiplierColorPulse()
	{
		while (true)
		{
			Color currentCol = multiplierColor;
			hudMultiplierText.color = currentCol;
			for (int i = 0; i < (colorPulseTime / 2) / Time.fixedDeltaTime; i++)
			{
				currentCol.r += colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				currentCol.g += colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				currentCol.b += colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				hudMultiplierText.color = currentCol;
				yield return new WaitForFixedUpdate();
			}
			for (int i = 0; i < (colorPulseTime / 2) / Time.fixedDeltaTime; i++)
			{
				if (currentCol.r > multiplierColor.r) currentCol.r -= colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				if (currentCol.g > multiplierColor.g) currentCol.g -= colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				if (currentCol.b > multiplierColor.b) currentCol.b -= colorPulseAmount / ((colorPulseTime / 2) / Time.fixedDeltaTime);
				hudMultiplierText.color = currentCol;
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForFixedUpdate();
		}
	}

	// TODO breaks when disabled bc it cancels coroutines
	private IEnumerator IncrementScore(int amountToIncrement)
	{
		incrementQueue.Dequeue();
		int i = 0;
		while (i < amountToIncrement)
		{
			yield return new WaitForEndOfFrame();
			int newInc = Mathf.CeilToInt(amountToIncrement * (Time.deltaTime / secondsToIncrementPerEvent));
			i += newInc;
			if (i > amountToIncrement) newInc -= (i - amountToIncrement);
			hudScore += newInc;	
		}
		/*
		for (int i = 0; i < amountToIncrement; i++)
		{
			yield return new WaitForSeconds(secondsToIncrementPerEvent / amountToIncrement);
			hudScore++;
		}*/
		if (incrementQueue.Count > 0) incrementCoroutine = StartCoroutine(IncrementScore(incrementQueue.Peek()));
		else incrementCoroutine = null;
	}

	// event functions

	private IEnumerator ProcessScoreEventQueue()
	{

		while (true)
		{
			if (scoreEventQueue.Count != 0)
			{
				ScoreEvent scoreEvent = scoreEventQueue.Peek();

				// create obj
				scoreEvent.eventObject = Instantiate(scoreEventPrefab.gameObject, Vector3.zero, Quaternion.identity, scoreEventContainer.transform);
				RectTransform eventRect = scoreEvent.eventObject.GetComponent<RectTransform>();
				TMP_Text eventText = scoreEvent.eventObject.GetComponent<TMP_Text>();
				eventRect.anchoredPosition = Vector3.zero;
				eventText.text = scoreEvent.text;
				
				scoreEventObjects.Enqueue(scoreEvent);
				scoreEvent.lifeCoroutine = StartCoroutine(scoreEvent.LifeCoroutine());


				// delete an excess if cap
				if (scoreEventObjects.Count > maxEventsOnScreen)
				{
					scoreEventObjects.Peek().DestroyEarly();
					scoreEventObjects.Dequeue();
				}

				// move all objs
				// TODO tween/something smoother
				HashSet<RectTransform> objsToMove = new HashSet<RectTransform>(scoreEventContainer.GetComponentsInChildren<RectTransform>());
				objsToMove.Remove(scoreEventContainer.GetComponent<RectTransform>());
				for (int i = 0; i < timeToMoveEvents / Time.fixedDeltaTime; i++)
				{
					foreach (RectTransform rect in objsToMove)
					{
						if (rect == null) continue;
						Vector3 rectPos = rect.anchoredPosition;
						rectPos.y -= distanceToMoveEvents * (Time.fixedDeltaTime / timeToMoveEvents);
						rect.anchoredPosition = rectPos;
					}
					yield return new WaitForFixedUpdate();
				}

				// dequeue
				scoreEventQueue.Dequeue();
			}
			yield return new WaitForFixedUpdate();
		}

	}
	// TODO cap amount

	private class ScoreEvent
	{
		public ScoreEvent(ScoreHUD hud, string text)
		{
			this.hud = hud;
			this.text = text;
		}
		private ScoreHUD hud;
		public string text;
		public GameObject eventObject;
		public Coroutine lifeCoroutine;
		private Coroutine fadeOutCoroutine;

		public void DestroyEarly()
		{
			if (fadeOutCoroutine == null)
			{
				hud.StopCoroutine(lifeCoroutine);
				fadeOutCoroutine = hud.StartCoroutine(FadeOut());
			}
		}

		public IEnumerator LifeCoroutine()
		{
			TMP_Text eventText = eventObject.GetComponent<TMP_Text>();

			// fade in
			for (float i = 0; i < hud.eventFadeInTime; i += Time.fixedDeltaTime)
			{
				eventText.alpha = i / hud.eventFadeInTime;
				yield return new WaitForFixedUpdate();
			}
			yield return new WaitForSeconds(hud.eventMidTime);
			fadeOutCoroutine = hud.StartCoroutine(FadeOut());
		}

		public IEnumerator FadeOut()
		{
			TMP_Text eventText = eventObject.GetComponent<TMP_Text>();
			// fade out
			for (float i = 0; i < hud.eventFadeOutTime; i += Time.fixedDeltaTime)
			{
				eventText.alpha = 1 - (i / hud.eventFadeOutTime);
				yield return new WaitForFixedUpdate();
			}

			if (hud.scoreEventObjects.Peek() == this) hud.scoreEventObjects.Dequeue();
			else if (hud.scoreEventObjects.Contains(this)) Debug.LogError("Score queue got messed up somehow");
			//else Debug.LogError("Score queue got messed up somehow");

			Destroy(eventObject);
		}
	}

	public static void CreateScoreBillboard(Transform obj, int score)
	{
		if (s_cam == null || s_overlayContainer == null || s_scoreBillboardPrefab == null) return;

		GameObject go = Instantiate(s_scoreBillboardPrefab, Vector3.zero, Quaternion.identity, s_overlayContainer);
		ScoreBillboard bb = go.GetComponent<ScoreBillboard>();
		bb.cam = s_cam;
		bb.objTransform = obj;
		bb.DisplayedText = "+" + score.ToString();
	}


	IEnumerator CheckPowerups()
	{
		float distance = 100;

		float distSq = distance * distance;
		while (pickupsInLevel.Count > 0) {
			yield return new WaitForSeconds(0.5f);
			// duplicate list bc it might be modified during the foreach loop since this is running in a coroutine
			foreach (PickupBase pickup in new List<PickupBase>(pickupsInLevel))
			{
				if ((pickup.transform.position - cam.transform.position).sqrMagnitude < distSq && pickup.enabled)
				{
					//MeshRenderer rend = GetComponentInChildren<MeshRenderer>();
					//if (rend != null) { if (!rend.enabled) continue; }

					if (pickup is WeaponPickup)
					{
						GameObject go = Instantiate(powerupBillboardPrefab, Vector3.zero, Quaternion.identity, s_overlayContainer);
						ScoreBillboard bb = go.GetComponent<ScoreBillboard>();
						bb.cam = cam;
						bb.objTransform = pickup.transform;

						pickup.onDestroy += () => Destroy(go);

						WeaponPickup pick = (WeaponPickup)pickup;
						Vector2 size = bb.optionalLine.sizeDelta;

						if (pick.WeaponReference.GetComponentInChildren<Blaster>() != null)
						{
							bb.DisplayedText = "BLASTER";
							size.y = 255;
							bb.optionalLine.sizeDelta = size;
						}
						else if (pick.WeaponReference.GetComponentInChildren<EnergyBurst>() != null)
						{
							bb.DisplayedText = "ENERGY BURST";
							size.y = 440;
							bb.optionalLine.sizeDelta = size;
						}
						else if (pick.WeaponReference.GetComponentInChildren<LaserBeam>() != null)
						{
							bb.DisplayedText = "LASER";
							size.y = 185;
							bb.optionalLine.sizeDelta = size;
						}
					}
					pickupsInLevel.Remove(pickup);
					continue;
				}
			}
		}
	}
}
