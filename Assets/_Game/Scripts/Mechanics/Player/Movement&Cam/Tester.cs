using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Tester : MonoBehaviour
{
    public int waypointIndex;
    public GameObject[] waypointTriggers;

    public PlayerMovement playerMovement;
    public CinemachineDollyCart cineDollyCart;
    public CinemachineSmoothPath cineSmoothPath;

    public TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        SetWaypointTriggers();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            cineDollyCart.m_Speed--;
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            cineDollyCart.m_Speed++;
        }
    }

    void SetWaypointTriggers()
    {
        for (int i = 0; i < cineSmoothPath.m_Waypoints.Length; i++)
        {
            waypointTriggers[i].transform.position = cineSmoothPath.m_Waypoints[i].position;
        }
    }

    public void SetRailSpeed(float speed)
    {
        cineDollyCart.m_Speed = speed;
        waypointIndex++;
    }

    public void InvincibleText()
    {
        StartCoroutine(InvincibleTextCoroutine());
    }

    IEnumerator InvincibleTextCoroutine()
    {
        if (uiText.text == "")
        {
            uiText.text = "Invincible";

            yield return new WaitForSeconds(1);

            uiText.text = "";
        }
    }
}
