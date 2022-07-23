using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_combat : MonoBehaviour
{
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Player_attack")) {
            StartCoroutine(attack());
        }   
    }

    private IEnumerator attack (){
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("animationlock", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("animationlock", false);
        animator.SetBool("IsAttacking", false);
    }
}
