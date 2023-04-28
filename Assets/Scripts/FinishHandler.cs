using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishHandler : MonoBehaviour
{
    public GameObject objPanel;
    public Image imgCongrats;
    public Image imgFailed;
    public Button btnPlayAgain;
    public Text txtRansomVal;
    public Text txtGatheredVal;
    public Text txtResult;
    public int ransomAmount;
    private int totalScore;

    private void Start()
    {
        totalScore = PlayerPrefs.GetInt(Constants.PREF_TOTAL_SCORE, 0);
        txtGatheredVal.text = totalScore.ToString();
        StartCoroutine(CountRansomVal());
    }

    private IEnumerator CountRansomVal()
    {
        for (int i = 1; i <= ransomAmount; i++)
        {
            txtRansomVal.text = i.ToString();
            yield return new WaitForSeconds(0.0005f);
        }
        if (totalScore >= ransomAmount)
        {
            txtResult.text = "CONGRATS!";
            imgCongrats.gameObject.SetActive(true);
        }
        else
        {
            txtResult.text = "FAILED!";
            imgFailed.gameObject.SetActive(true);
        }
        btnPlayAgain.gameObject.SetActive(true);
    }

    public void OnBtnPlayAgainClick()
    {
        objPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
