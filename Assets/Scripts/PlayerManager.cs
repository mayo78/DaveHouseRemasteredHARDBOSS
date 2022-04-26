using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public Collider daveSpeakTrigger, daveAngry, winLine;
    public bool beenJumpscared;
    public GameObject daveJumpscare, angryDave;

    Move move;
    Look look;

    Dave dave;

    public NavMeshAgent gottaSleep;

    bool sleeping, alarmClockActive;
    float sleepingFailsave; // sorry mystman

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
            if(!beenJumpscared)
            {
                Jumpscared();
            }
        }
        if(other == winLine && GameManager.Instance.finalMode)
        {
            PlayerPrefs.SetInt("HasWon", 1);
            SceneManager.LoadScene("Win");
        }
    }
    public void Jumpscared()
    {
        daveJumpscare.SetActive(true);
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
