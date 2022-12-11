using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDam : MonoBehaviour
{
    public Slider playerHPSlider;
    public int playerHP = 100;
    bool canTakeDamage = true;

    void Update()
    {
        //if (BossAI.takenPlayerColDam == true) playerHP -= 5;
        if (!ChargeAttackNode.GoingtoHitPlayer) { canTakeDamage = true; }
        if (ZAttack.TakenDamage) playerHP -= 1;
        playerHPSlider.value = playerHP;

        if (playerHP == 0)//Turns the player off and restarts the game
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            gameObject.GetComponent<PlayerMove>().enabled = false;
            StartCoroutine(LoadLevelAfterDelay(2));
        }
    }

    void OnParticleCollision(GameObject other)//Checks if the player has been hit by the boss attack
    {
        Debug.Log(other);
        if (other.gameObject.tag == "BossAttack")
        {
            playerHP -= 1;
            Debug.Log("Player Taking Damage");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Boss") && ChargeAttackNode.GoingtoHitPlayer && canTakeDamage)
        {
            Debug.Log("Hit Player");
            playerHP -= 5;
            canTakeDamage = false;
        }

        //DeathBomb
        if (col.gameObject.tag == "DeathBomb")
        {
            playerHP -= 10;
            Debug.Log("Player Taking Damage");
        }

    }


    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("BossArena");
    }
}
