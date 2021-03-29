using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBriefing : MonoBehaviour
{
    [SerializeField] private GameObject debrief1, debrief2, debrief3;
    void OnEnable()
    {       
        GameManager.gm.SetMissionBriefing(gameObject, debrief1, debrief2, debrief3);
        if(GameManager.gm.currentState == GameState.Briefing)
        {
            Time.timeScale = 0;
            switch (GameManager.gm.currentLevel)
            {
                case 1:
                    debrief1.SetActive(true);
                    break;
                case 2:
                    debrief2.SetActive(true);
                    break;
                case 3:
                    debrief3.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void EndBriefing() {
        GameManager.gm.EndMissionBriefing();
    }

}
