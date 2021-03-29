using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWaypoint : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] UnityEvent OnEnter;
    
    [SerializeField] Renderer renderer;

    private void Start()
    {
        if (renderer != null)
        {
            renderer.enabled = false; //Disable the renderer, so it's visible in editor but not in-game
        }
    }
    public void OnReach()
    {
        OnEnter.Invoke();
    }
}
