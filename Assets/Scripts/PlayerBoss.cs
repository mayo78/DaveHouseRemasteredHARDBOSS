using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using GameJolt.API;
using GameJolt.API.Objects;

public class PlayerBoss : MonoBehaviour
{
    int health = 100, ammo = 30;

    public Slider healthBar;

    public GameObject blasterBullet;
    public Transform cam;

    public DaveBoss boss;

    public TMP_Text ammoText;

    bool reloading;

    public AudioSource audioSource;
    public AudioClip fire, reload;

    public Collider exitThree;

    public GameObject postProcessingHandler;
    
    bool isShooting; // making it so the blaster acts like a machine gun :)

    private void Start()
    {
        if (PlayerPrefs.GetInt("PostProcessing", 1) == 1)
        {
            postProcessingHandler.SetActive(true);
        }
        else
        {
            postProcessingHandler.SetActive(false);
        }
    }
    private void Update()
    {
        healthBar.value = health;
        ammoText.text = $"Ammo: {ammo}";
        
        isShooting = Input.GetMouseButton(0);
        if(Input.GetKeyDown(KeyCode.R) && !reloading && boss.bossStarted)
        {
            StartCoroutine(Reload());
        }
        if(health <= 0)
        {
            SceneManager.LoadScene("BossGameOver");
        }
        if(boss.bossStarted && isShooting && ammo > 0 && !reloading)
        {
          Instantiate(blasterBullet, cam.position, cam.rotation);
          audioSource.PlayOneShot(fire);
          ammo--;
        }
        if(ammo > 0)
        {
          isShooting = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pyramid"))
        {
            health -= 5;
            Destroy(other);
        }
        if(other.CompareTag("Spike"))
        {
            health -= 30;
        }
        if(other.CompareTag("DaveTwo") && !other.GetComponent<DavePhaseTwo>().bossDefeated)
        {
            health = 0;
        }
        if(other.CompareTag("Lava") || other.CompareTag("LavaAttack"))
        {
            health = 0;
        }
        if(other == exitThree)
        {
            SceneManager.LoadScene("Menu");

            PlayerPrefs.SetInt("BossBattleUnlocked", 1);
            if(health == 100)
            {
                UnlockTrophy(167459);
            }
            else
            {
                UnlockTrophy(167460);
            }
        }
    }
    IEnumerator Reload()
    {
        reloading = true;
        audioSource.PlayOneShot(reload);
        yield return new WaitForSeconds(reload.length);
        ammo = 30;
        reloading = false;
    }
    public void UnlockTrophy(int id)
    {
        if (GameJoltAPI.Instance.CurrentUser != null)
        {
            Trophies.Get(id, (Trophy lol) =>
            {
                if (!lol.Unlocked)
                {
                    Trophies.Unlock(id, (bool success) =>
                    {
                        if (success)
                        {
                            Debug.Log("Trophy unlocked");
                        }
                    });
                }
            });
        }
    }
}
