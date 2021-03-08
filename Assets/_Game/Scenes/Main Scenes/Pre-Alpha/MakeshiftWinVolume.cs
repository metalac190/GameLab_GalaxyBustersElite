using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeshiftWinVolume : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        GameManager.gm.WinGame();
    }

}