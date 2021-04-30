using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHost : MonoBehaviour
{
	private static CoroutineHost instance;
	public static CoroutineHost Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public Coroutine StartCoroutineFromHost(IEnumerator routine)
	{
		return StartCoroutine(routine);
	}
}
