using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Story : MonoBehaviour
{
    public GameObject btnStoryRead;
    public GameObject canvas;
    public string[] sentences;
    public float typingSpeed;
    public Text storyText;

    private void Awake()
    {
        canvas.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        bool firstSentence = true;
        foreach (string s in sentences)
        {
            char[] charArr = s.ToCharArray();

            foreach (char c in charArr)
            {
                storyText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (firstSentence)
            {
                storyText.text += "\n\n";
                firstSentence = false;
            }
        }
        btnStoryRead.SetActive(true);
    }

    public void OnStroyReadClick()
    {
        canvas.SetActive(false);
        int currenLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++currenLevel);
    }
}
