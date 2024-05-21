//this is for all upgrades, attributes of the player. even upgrades will be kept here then will be used from the scripts using getcomponent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeController : MonoBehaviour
{ 
    public float playerBulletDamage = 20f;
    public float reloadSpeed = 1f;
    public float bulletSpeed = 10f;
    public int shotAmount = 3;

    public float playerMeleeDamage = 40f;

    public float timeSlowMultiplier = 1f;

    public float seeAngle = 30f;

    public float meleeAttackCooldown = 1f;

    public float moveSpeed = 5f;

    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public float dashCoolDown = 1f;
}
