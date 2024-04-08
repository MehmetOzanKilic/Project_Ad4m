//this is for all upgrades, attributes of the player. even upgrades will be kept here then will be used from the scripts using getcomponent.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributeController : MonoBehaviour
{ 
    public float playerBulletDamage = 20f;
    public float playerMeleeDamage = 40f;
    public float reloadSpeed = 1f;
    public int shotAmount = 3;
}
