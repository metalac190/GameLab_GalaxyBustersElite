using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerController : MonoBehaviour
{
    [Header("Damage Flash Settings")]
    [Tooltip("The Material to alternate between")]
    public Material FlashMaterial = null;
    [Tooltip("The gameobject with the corresponding Mesh")]
    [SerializeField] private GameObject _meshSegment = null;
    [Tooltip("Full cycle length of a single Flash\n(On and Off)")]
    [SerializeField] private float _flashLength = 0.1f;
    [Tooltip("Number of Flashes to take place per one damage")]
    [SerializeField] private int _flashNumber = 5;
    
    private Material _flashMat = null;
    private Material _startMat = null;
    private Renderer _meshRender = null;
    private Coroutine _flickerRoutine = null;

    private int _flashCount = 0;

    private void Awake()
    {
        _meshRender = _meshSegment.GetComponent<Renderer>();
        _startMat = _meshRender.material;
    }

    public void CallFlicker()
    {
        _flashCount = _flashNumber;

        if(_flickerRoutine == null)
            _flickerRoutine = StartCoroutine(RepeatFlash());
    }

    private IEnumerator RepeatFlash()
    {
        //referencing instance variable flashCount, which is reset at each instance of damage
        //multiple damage instnaces will reset the timer but not extend it
        while (_flashCount > 0)
        {
            Debug.Log("Set to Red");
            _meshRender.material = _flashMat;

            yield return new WaitForSeconds(_flashLength / 2);

            Debug.Log("Set to Normal");
            _meshRender.material = _startMat;

            yield return new WaitForSeconds(_flashLength / 2);

            _flashCount--;
        }

        Debug.Log("Done Flashing");
        _flickerRoutine = null;
    }
}
