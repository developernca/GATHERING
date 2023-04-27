using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishHandler : MonoBehaviour
{
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
            yield return new WaitForSeconds(0.0025f);
        }
        if (totalScore >= ransomAmount)
            txtResult.text = "CONGRATS!";
        else
            txtResult.text = "FAILED!";
    }
}
