using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public int health;
    public int attack;
    public string abilitytxt;
}
