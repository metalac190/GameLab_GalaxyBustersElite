using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailedCamShake : MonoBehaviour
{
    public List<ShakeInfo> shakeInfos = new List<ShakeInfo>();


    public void TriggerShake(int index)
    {
        StartCoroutine(CameraShaker.instance.AlternateShake(shakeInfos[index].shakeAmount, shakeInfos[index].decay, shakeInfos[index].range));
    }

    [Serializable]
    public class ShakeInfo
    {
        public float shakeAmount;
        public float decay;
        public float range;
    }
}
