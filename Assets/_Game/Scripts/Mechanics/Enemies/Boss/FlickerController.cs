using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerController : MonoBehaviour
{
    [Header("Flash Settings")]
    [Tooltip("Scriptable Object Reference for this FlickerType")]
    [SerializeField] private FlickerSettings _flickerSettings = null;
    [Tooltip("The gameobject with the corresponding Mesh")]
    [SerializeField] private Renderer[] _meshSegments = null;
    
    private Material[] _startMats = null;
    //private Renderer _meshRender = null;
    private Coroutine _flickerRoutine = null;

    private int _flashCount = 0;

    private void Awake()
    {
        //_meshRender = _meshSegment.GetComponent<Renderer>();
        _startMats = new Material[_meshSegments.Length];
        for (int m = 0; m < _meshSegments.Length; m++)
            _startMats[m] = _meshSegments[m].material;
    }

    public void CallFlicker()
    {
        _flashCount = _flickerSettings.FlickerNumber;

        if(_flickerRoutine == null)
            _flickerRoutine = StartCoroutine(RepeatFlash());
    }

    private IEnumerator RepeatFlash()
    {
        //referencing instance variable flashCount, which is reset at each instance of damage
        //multiple damage instnaces will reset the timer but not extend it
        while (_flashCount > 0)
        {
            for (int m = 0; m < _meshSegments.Length; m++)
                _meshSegments[m].material = _flickerSettings.FlickerMaterial;

            yield return new WaitForSeconds(_flickerSettings.FlickerTime / 2);

            for (int m = 0; m < _meshSegments.Length; m++)
                _meshSegments[m].material = _startMats[m];

            yield return new WaitForSeconds(_flickerSettings.FlickerTime / 2);

            _flashCount--;
        }
        
        _flickerRoutine = null;
    }
}
