using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicManager : MonoBehaviour
{

    public GameObject panel1;
    public int angle1 = 90;
    public GameObject panel2;
    public int angle2 = 90;
    public GameObject panel3;
    public int angle3 = 90;
    public GameObject panel4;
    public int angle4 = 90;
    public GameObject nextButton;
    public GameObject skipButton;
    int nextPanel = 0;
    float distance = 1000;
    float speed = 3000;
    float savedTime;

    GameObject[] panels;
    int[] angles;

    
    void Awake()
    {
        panels = new GameObject[] { panel1, panel2, panel3, panel4 };
        angles = new int[] { angle1, angle2, angle3, angle4 };
        foreach(GameObject p in panels)
        {
            p.SetActive(false);
        }
        nextButton.SetActive(false);
        skipButton.SetActive(false);
    }

    private void Update()
    {
        if (nextPanel > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Next();
            }
        }
    }



    public void StartSequence()
    {
        savedTime = Time.timeScale;
        Time.timeScale = 0;
        nextButton.SetActive(true);
        skipButton.SetActive(true);
        nextPanel = 1;
        Next();
    }
    public void Next()
    {
        if (nextPanel < panels.Length + 1)
        {
            StartCoroutine(AddPanel(nextPanel));
            nextPanel++;
        }
        else
        {
            exitComic();
        }
    }
    public void exitComic()
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
        nextButton.SetActive(false);
        skipButton.SetActive(false);
        nextPanel = 0;
        Time.timeScale = savedTime;

        //Exit function goes here
    }

    private IEnumerator AddPanel(int panelNum)
    {
        GameObject currentPanel = panels[panelNum - 1];
        int currentAngle = angles[panelNum - 1];
        Vector3 startingPos = currentPanel.transform.position;
        currentPanel.SetActive(true);
        currentPanel.transform.position += new Vector3(distance * Mathf.Cos(currentAngle * Mathf.Deg2Rad), distance * Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0);

        while (Vector3.Distance(currentPanel.transform.position, startingPos) > 1f)
        {
            currentPanel.transform.position = Vector3.MoveTowards(currentPanel.transform.position, startingPos, .02f * speed);

            yield return new WaitForSecondsRealtime(0.02f);
        }

        currentPanel.transform.position = startingPos;
    }
}
