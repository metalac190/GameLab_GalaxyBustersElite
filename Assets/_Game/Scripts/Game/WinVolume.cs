using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinVolume : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.gm.WinGame();
    }

}