using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private enum MovementSetting
    {
        PingPong = 0,
        Loop,
        DoOnce
    }

    [Header("Object Reference")]
    [Tooltip("The object that needs to be moved")]
    [SerializeField] private GameObject _object = null;

    [Header("Rotation")]
    [Tooltip("Each angle is controlled independently\nRotations can be negative.")]
    [SerializeField] private Vector3 _rotationAngles = Vector3.zero;
    [Tooltip("Multiplies across all rotation angles")]
    [SerializeField] private float _rotationSpeed = 1f;

    [Header("Waypoints")]
    [SerializeField] private Transform[] _waypoints;
    [Range(0, Mathf.Infinity)] [Tooltip("Must be Positive")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private MovementSetting Setting;
    
    private Coroutine _movementRoutine = null;
    private bool loopDireciton = true;

    private void Start()
    {
        _movementRoutine = StartCoroutine(MoveToWaypoint(0));
    }

    private void Update()
    {
        _object.transform.Rotate(_rotationAngles * _rotationSpeed * Time.deltaTime);
    }

    private IEnumerator MoveToWaypoint(int target)
    {
        while (_object.transform.position != _waypoints[target].position)
        {
            Vector3 moveTo = Vector3.MoveTowards(_object.transform.position, _waypoints[target].position, _moveSpeed * Time.deltaTime);
            _object.transform.position = moveTo;

            yield return new WaitForEndOfFrame();
        }

        MovementLoop(target, Setting);
    }

    private void MovementLoop(int index, MovementSetting setting)
    {
        switch(setting)
        {
            case MovementSetting.PingPong:
                PingPong(index);
                break;
            case MovementSetting.Loop:
                Loop(index);
                break;
            case MovementSetting.DoOnce:
                DoOnce(index);
                break;
        }
    }

    #region Movement Types
    private void PingPong(int index)
    {
        if (loopDireciton)
        {
            index++;
            if (index >= _waypoints.Length)
            {
                index = _waypoints.Length - 1;
                loopDireciton = false;
            }
        }
        else
        {
            index--;
            if (index < 0)
            {
                index = 0;
                loopDireciton = true;
            }
        }

        _movementRoutine = StartCoroutine(MoveToWaypoint(index));
    }

    private void Loop(int index)
    {
        index++;
        if (index >= _waypoints.Length)
            index = 0;

        _movementRoutine = StartCoroutine(MoveToWaypoint(index));
    }

    private void DoOnce(int index)
    {
        index++;

        if (index < _waypoints.Length)
            _movementRoutine = StartCoroutine(MoveToWaypoint(index));
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        switch (Setting)
        {
            case MovementSetting.PingPong:
                Gizmos.color = Color.white;
                break;
            case MovementSetting.Loop:
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_waypoints[_waypoints.Length - 1].position, _waypoints[0].position);
                break;
            case MovementSetting.DoOnce:
                Gizmos.color = Color.red;
                break;
        }
        
        for(int i=0; i<_waypoints.Length; i++)
        {
            if (i == _waypoints.Length - 1)
                continue;

            Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
        }
    }
}
