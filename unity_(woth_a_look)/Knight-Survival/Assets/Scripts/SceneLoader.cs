using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float rate;

    [SerializeField]
    private Button btn;
    private AudioSource src;
    private 
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();
    }

    public void SwithBackToMenu() {
        PlayBtnTransition();
        StartCoroutine(LoadScene(0));
    }
    public void startGame() {
        PlayBtnTransition();
        StartCoroutine(LoadScene(1));
    }
    IEnumerator LoadScene(int sceneNumber) {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(sceneNumber);
    }
    public void fadeText() {
        TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
        Color tmp = txt.color;
        tmp.a = rate;
        txt.color = tmp;
    }
    void PlayBtnTransition() {
        src.Play();
        btn.enabled = false;
        fadeText();
    }
}
