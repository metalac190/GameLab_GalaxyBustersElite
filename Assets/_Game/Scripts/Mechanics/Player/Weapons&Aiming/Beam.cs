using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
	[SerializeField] private GameObject target;
	private LineRenderer line;

	private void OnEnable()
	{
		line = GetComponent<LineRenderer>();
		line.positionCount = 1;
		line.SetPosition(0, transform.position);
	}

	private void Update()
	{
		line.SetPosition(0, transform.position);
		
		if(target == null)
		{
			line.positionCount = 1;
		}

		if(line.positionCount == 2)
		{
			line.SetPosition(1, target.transform.position);
		}
	}

	public void SetTarget(GameObject newTarget)
	{
		target = newTarget;
		line.positionCount = 2;
		line.SetPosition(1, target.transform.position);
	}

	
}
