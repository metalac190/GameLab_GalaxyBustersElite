using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDespawnSequence : MonoBehaviour
{
    public GameObject[] spawnSections;
    public bool isImmediate;
    public float wait_between_time;

    private void OnTriggerEnter(Collider other)
    {
        //validate, other is Player, probably a better method somewhere
        if (other.CompareTag("Player"))
            StartCoroutine(SpawnSequence(isImmediate, wait_between_time));

    }

    IEnumerator SpawnSequence(bool immediate, float time)
    {
        for (int i = 0; i < spawnSections.Length; i++)
        {
            if (isImmediate == false)
            {
                spawnSections[i].SetActive(false);
                yield return new WaitForSeconds(time);
            }
            else
            {
                spawnSections[i].SetActive(false);
            }


        }




    }
}
