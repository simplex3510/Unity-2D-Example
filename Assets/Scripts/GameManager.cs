using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMove player;
    public GameObject[] stages;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;


    public void NextStage()
    {
        if (stageIndex < stages.Length-1)
        {
            stages[stageIndex].SetActive(false);
            stageIndex++;
            stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            Time.timeScale = 0;
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
        }
        else
        {
            health--;
            // Player Die Effect
            player.OnDie();

            // Result UI

            // Retry Button UI
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

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-18.5f, -1f, 0);
        player.VelocityZero();
    }

}
