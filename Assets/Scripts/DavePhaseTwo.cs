using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DavePhaseTwo : MonoBehaviour
{
    float health = 500;

    public Slider healthBar;
    public bool bossDefeated;
    bool fadeMusic;
    bool triggerBossDefeatCutscene;

    public AudioSource music, dave;

    public AudioClip riser, daveScare;

    public GameObject exitTwo;

    public SpriteRenderer daveTwoSprite;
    public Sprite defeated;
    void Update()
    {
        if(!bossDefeated)
        {
            Vector3 position = transform.position;
            position.x += 3.5f * Time.deltaTime;
            transform.position = position;
        }

        healthBar.value = health;

        if(health <= 0)
        {
            bossDefeated = true;
        }
        if(bossDefeated && !triggerBossDefeatCutscene)
        {
            StartCoroutine(Defeated());
        }
        if(fadeMusic)
        {
            music.volume -= 0.25f * Time.deltaTime;
        }
        if(!bossDefeated && Input.GetKeyDown(KeyCode.U))
        {
          health = 0; //for debugging :)
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LaserShot"))
        {
            health -= Random.Range(1f, 3f);
            Destroy(other);
        }
    }
    IEnumerator Defeated()
    {
        exitTwo.SetActive(false);
        triggerBossDefeatCutscene = true;
        fadeMusic = true;
        daveTwoSprite.sprite = defeated;
        dave.PlayOneShot(riser);
        yield return new WaitForSeconds(3);
        dave.PlayOneShot(daveScare);
        yield return new WaitForSeconds(10);
        transform.DOMoveY(-400, 5);
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
    }
}
