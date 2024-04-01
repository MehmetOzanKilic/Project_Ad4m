using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CardGameManager;

public class CardController : MonoBehaviour
{
    [SerializeField] public CardData card;
    public TextMeshProUGUI card_info_txt;

    public CardGameManager cardGameManager;

    public Transform cardGridPos;

    // Start is called before the first frame update
    void Start()
    {
        if (card != null && card_info_txt != null)
        {
            this.card_info_txt.text = $"{card.name}\nHealth: {card.health}\nAttack: {card.attack}\n{card.abilitytxt}";
        }

        cardGameManager = FindObjectOfType<CardGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cardGameManager.currentPhase == CardGameManager.GamePhase.Action && gameObject.transform.parent.CompareTag("GridSlot"))
        {
            getSurroundingCardsInfo();
            switch (card.type)
            {
                case CardData.CardType.DirectDamage:
                    attackCard();
                    break;
                case CardData.CardType.Healing:
                    healCard();
                    break;
                case CardData.CardType.AOEDmg:
                    attackAOECard();
                    break;
                case CardData.CardType.BuffDebuff:
                    buffDebuffCard();
                    break;
                case CardData.CardType.Blocking:
                    blockDmg();
                    break;
                case CardData.CardType.CounterAttack:
                    counterattack();
                    break;
                case CardData.CardType.CardSacrifice:
                    sacrifice();
                    break;
            }
        }
    }


    public void getSurroundingCardsInfo()
    {

    }

    public void attackCard()
    {

    }

    public void attackAOECard()
    {

    }

    public void healCard()
    {

    }

    public void buffDebuffCard()
    {

    }
    public void blockDmg()
    {

    }

    public void counterattack()
    {

    }

    public void sacrifice()
    {

    }
}
