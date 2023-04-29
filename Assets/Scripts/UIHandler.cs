using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public GameObject objPanelGameOver;
    public GameObject objPanelTutorial;
    public Button btnRestart;
    public Text txtLevel;
    public Text txtScore;
    public Image imgFinishTrans;
    private int totalScore;
    private int score;
    private bool tutorialDestroyed;

    private void Awake()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel == 1)
        {
            PlayerPrefs.SetInt(Constants.PREF_TOTAL_SCORE, 0);// reset score when starting from level1
        }
        totalScore = PlayerPrefs.GetInt(Constants.PREF_TOTAL_SCORE, 0);
        txtLevel.text = "Level - " + currentLevel;
        // CalculateScoreForDevelopment();
    }

    private void Start()
    {
        objPanelTutorial.SetActive(true);
        StartCoroutine(HideTutorial());
    }

    private void OnEnable()
    {
        Player.PlayerPickup += OnPlayerPickup;
        Player.PlayerPickupFinish += OnPlayerPickupFinish;
        Player.PlayerDead += OnPlayerDead;
        Enemy.GivePlayerScore += OnGivePlayerScore;
    }

    private void OnDisable()
    {
        Player.PlayerPickup -= OnPlayerPickup;
        Player.PlayerPickupFinish -= OnPlayerPickupFinish;
        Player.PlayerDead -= OnPlayerDead;
        Enemy.GivePlayerScore -= OnGivePlayerScore;
    }

    private void OnGivePlayerScore(int s)
    {
        score += s;
        txtScore.text = score.ToString();
    }

    private void OnPlayerPickup(int val)
    {
        score += val;
        txtScore.text = score.ToString();
    }

    private void OnPlayerPickupFinish()
    {
        StartCoroutine(LevelTransit());
    }

    private void OnPlayerDead()
    {
        if (!tutorialDestroyed) objPanelTutorial.SetActive(false);
        objPanelGameOver.SetActive(true);
    }

    public void OnRestartClick()
    {
        btnRestart.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator HideTutorial()
    {
        yield return new WaitForSeconds(5f);
        Destroy(objPanelTutorial);
        tutorialDestroyed = true;
    }

    private IEnumerator LevelTransit()
    {
        // Show transit panel
        imgFinishTrans.gameObject.SetActive(true);
        Color color = imgFinishTrans.color;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += 0.1f;
            color.a = alpha;
            imgFinishTrans.color = color;
            yield return new WaitForSeconds(0.1f);
        }
        // Set score
        totalScore += score;
        PlayerPrefs.SetInt(Constants.PREF_TOTAL_SCORE, totalScore);
        // Goto Next Level
        int currenLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++currenLevel);
    }

    private void CalculateScoreForDevelopment()
    {
        int chicken = 0;
        int piglet = 0;
        int wildboar = 0;
        GameObject[] obj = GameObject.FindGameObjectsWithTag(Constants.TAG_ENEMY);
        foreach (GameObject g in obj)
        {
            if (g.name.Contains("Chicken"))
            {
                ++chicken;
            }
            else if (g.name.Contains("Piglet"))
            {
                ++piglet;
            }
            else if (g.name.Contains("Wildboar"))
            {
                ++wildboar;
            }
        }
        int coin = GameObject.FindGameObjectsWithTag(Constants.TAG_PICKUP).Length - 1;// mius 1 to remove finish pickup
        print($"Piglet : {piglet} x 10 = {piglet * 10}");
        print($"Chicken : {chicken} x 30 = {chicken * 30}");
        print($"Wildboar : {wildboar} x 50 = {wildboar * 50}");
        print($"Coin : {coin} x 20 = {coin * 20}");
    }
}
