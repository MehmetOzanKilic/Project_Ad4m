using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    public GameObject weapon;
    public bool canAttack = true;
    public float attackCooldown = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (canAttack)
            {
                MeleeAttack();
            }
        }

    }
    public void MeleeAttack()
    {
        canAttack = false;
        Animator anim = weapon.GetComponent<Animator>();
        anim.SetTrigger("Attack");
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
