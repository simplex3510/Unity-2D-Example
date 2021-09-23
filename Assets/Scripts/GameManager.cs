using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;
    public GameObject[] stages;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartButton;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        if (stageIndex < stages.Length-1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = ("Stage " + (stageIndex+1)).ToString();
        }
        else
        {
            Time.timeScale = 0;
            UIRestartButton.SetActive(true);
            Text btnText = UIRestartButton.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            UIRestartButton.SetActive(true);
        }

        // Calculator Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (1 < health)
        {
            health--;
            UIhealth[health].color = new Color (1, 1, 1, 0.4f);
        }
        else
        {
            health--;
            UIhealth[health].color = new Color (1, 1, 1, 0.4f);
            // Player Die Effect
            player.OnDie();

            // Result UI

            // Retry Button UI
            UIRestartButton.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Player Reposition
            if (1 < health)
            {
                PlayerReposition();
            }
            // Health Down
            HealthDown();
        }
    }

    private void PlayerReposition()
    {
        player.transform.position = new Vector3(-18.5f, -1f, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
