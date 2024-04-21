using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackController : MonoBehaviour
{
    public GameObject weapon;
    private GameObject playerObject;
    private PlayerAttributeController playerAttributeController;
    public bool canAttack = true;
    public float attackCooldown;
    Animator anim;

    private void Awake()
    {
        playerObject = GameObject.FindWithTag("Player"); //sets the game object
        anim = weapon.GetComponent<Animator>();
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
    }

    void Update()
    {
        if (attackCooldown != playerAttributeController.meleeAttackCooldown)
        {
            attackCooldown = playerAttributeController.meleeAttackCooldown;
        }
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
        
        anim.SetTrigger("Attack");
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
