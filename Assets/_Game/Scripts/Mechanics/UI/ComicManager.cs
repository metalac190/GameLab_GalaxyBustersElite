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
    public int angleExit = 270;
    public float panelSpeed = 4000;
    public float panelDelay = 2;
    public GameObject background;
    int nextPanel = 0;
    float distance = 1000;
    float savedTime;
    public GameObject frame;

    GameObject[] panels;
    int[] angles;

    
    void Awake()
    {
        panels = new GameObject[] { panel1, panel2, panel3, panel4 };
        angles = new int[] { angle1, angle2, angle3, angle4 };
        frame.SetActive(true);
        foreach(GameObject p in panels)
        {
            p.SetActive(false);
        }
        background.SetActive(false);

        GameManager.gm.comicScreen = this;
    }

    private void Update()
    {
        if (nextPanel > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space)|| Input.GetButton("Primary Fire"))
            {
                Next(nextPanel);
            }
        }
    }



    public void StartSequence()
    {
        savedTime = Time.timeScale;
        Time.timeScale = 0;
        background.SetActive(true);
        nextPanel = 1;
        Next(1);
    }
    public void Next(int panel)
    {
        if (panel != nextPanel)
        {
            return;
        }
        if (nextPanel < panels.Length + 1)
        {
            StartCoroutine(AddPanel(nextPanel));
            nextPanel++;
            StartCoroutine(delayUntilPanel(nextPanel));
        }
        else
        {
            StartCoroutine(ExitPanels());
        }
    }
    public void ExitComic()
    {
        frame.SetActive(false);
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
        background.SetActive(false);
        nextPanel = 0;
        Time.timeScale = savedTime;

        GameManager.gm.EndComicSequence();
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
            currentPanel.transform.position = Vector3.MoveTowards(currentPanel.transform.position, startingPos, .02f * panelSpeed);

            yield return new WaitForSecondsRealtime(0.02f);
        }

        currentPanel.transform.position = startingPos;
    }

    private IEnumerator delayUntilPanel(int panelNum)
    {
        yield return new WaitForSecondsRealtime(panelDelay);
        if (panelNum < panels.Length+1)
        {
            Next(panelNum);
        }
    }

    private IEnumerator ExitPanels()
    {
        Vector3 endPos=frame.transform.position+new Vector3(distance * Mathf.Cos(angleExit * Mathf.Deg2Rad), distance * Mathf.Sin(angleExit * Mathf.Deg2Rad), 0);

        while (Vector3.Distance(frame.transform.position, endPos) > 1f)
        {
            frame.transform.position = Vector3.MoveTowards(frame.transform.position, endPos, .02f * panelSpeed);

            yield return new WaitForSecondsRealtime(0.02f);
        }

        ExitComic();
    }
}
