using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{
    [Header("^ Right click on component while playing to load scenes ^")]
    public bool fadeOutMusic = false;

    [ContextMenu("Load FeedbackTestScene1")]
    void LoadScene1() => StartCoroutine(LoadScene("FeedbackTestScene1"));
    [ContextMenu("Load FeedbackTestScene2")]
    void LoadScene2() => StartCoroutine(LoadScene("FeedbackTestScene2"));

    IEnumerator LoadScene(string nameOfScene)
    {
        if (fadeOutMusic)
            MusicPlayer.instance.FadeOut();

        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene(nameOfScene);
    }
}
