using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenAnim : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 10.0f;
    void Update()
    {
        float index = Time.time * framesPerSecond;
        index = index % frames.Length;
        gameObject.GetComponent<Image>().sprite = frames[(int)index];
    }
}
