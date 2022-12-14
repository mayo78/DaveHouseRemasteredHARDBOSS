using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public Collider daveSpeakTrigger, daveAngry, winLine, stopwatch;
    public bool beenJumpscared;
    public GameObject daveJumpscare, angryDave;

    Move move;
    Look look;

    Dave dave;

    public Sprite stopwatchSprite;

    private void Start()
    {
        move = GetComponent<Move>();
        look = GetComponentInChildren<Look>();
        dave = angryDave.GetComponent<Dave>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == daveSpeakTrigger)
        {
            GameManager.Instance.OnEnteredHouse();
        }
        if (other == daveAngry && !dave.pied && !dave.spinning)
        {
            if (!beenJumpscared)
            {
                GameManager.Instance.UnlockTrophy(162193);
                Jumpscared();
            }
        }
        if(other == stopwatch)
        {
            if (!beenJumpscared)
            {
                daveJumpscare.GetComponent<SpriteRenderer>().sprite = stopwatchSprite;
                Jumpscared();
            }
        }
        if(other == winLine && GameManager.Instance.finalMode)
        {
            if(GameManager.Instance.gamemode == "Normal")
            {
                PlayerPrefs.SetInt("HasWon", 1);
                GameManager.Instance.UnlockTrophy(162161);
                SceneManager.LoadScene("Win");
            }
            else if(GameManager.Instance.gamemode == "Timed")
            {
                int score = (int)Mathf.Round(GameManager.Instance.timePassed);
                GameManager.Instance.SubmitScore(score, score.ToString(), 722152, $"Submitted on version {Application.version}");
                GameManager.Instance.UnlockTrophy(162513);
                PlayerPrefs.SetInt("HasWon", 1);
                PlayerPrefs.SetInt("FinalTimeForSession", score);
                SceneManager.LoadScene("WinTimed");
            }
        }
    }
    public void Jumpscared()
    {
        beenJumpscared = true;
        daveJumpscare.SetActive(true);
        stopwatch.gameObject.SetActive(false);
        angryDave.SetActive(false);
        StartCoroutine(Jumpscare());
        move.lockPos = true;
        look.lockRot = true;
    }
    IEnumerator Jumpscare()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }

  
}
