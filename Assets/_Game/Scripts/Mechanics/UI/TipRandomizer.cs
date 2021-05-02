using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TipRandomizer : MonoBehaviour
{
    public string[] textOptions;
    private void OnEnable()
    {
        int random = Random.Range(0, textOptions.Length);
        gameObject.GetComponent<TextMeshProUGUI>().text = textOptions[random];
    }
}
