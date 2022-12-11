using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))//On click
        {
            PerformAttack();
        }
        if (Input.GetMouseButtonDown(1))//On click
        {
            PerformStun();
        }
    }

    public void PerformAttack()
    {
        animator.SetTrigger("Base_Attack");//Playes attack the animation
    }

    public void PerformStun()
    {
        animator.SetTrigger("Stun");//Playes attack the animation
    }

    void OnCollisionEnter(Collision col)
    {

    }
}
