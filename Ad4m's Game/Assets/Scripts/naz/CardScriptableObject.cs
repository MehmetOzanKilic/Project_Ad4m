using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string CardName;

    public int health;
    public int attack;
    public int healingAmount;
    public int buffingAmount;


    public string abilitytxt;
    public int symboltype = 0;

    [System.Serializable]
    public enum CardType
    {
        DirectDamage,
        Healing,
        AOEDmg,
        BuffDebuff,
        Blocking,
        CounterAttack,
        CardSacrifice
    }
    [SerializeField] public CardType type;
}

