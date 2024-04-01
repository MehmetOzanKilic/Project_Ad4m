using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CardGameManager;
using System.Linq;

public class CardController : MonoBehaviour
{
    [SerializeField] public CardData card;
    public TextMeshProUGUI card_info_txt;

    public CardGameManager cardGameManager;

    public GameObject cardGridPos;

    private bool hasSurroundingInfo = false;
    public GameObject rightOfThisCard;
    public GameObject leftOfThisCard;
    public GameObject frontOfThisCard;
    public GameObject behindThisCard;

    int thisCardAtk;
    int thisCardHlt;

    private bool isExecutingAbility = false;

    // Start is called before the first frame update
    void Start()
    {
        if (card != null && card_info_txt != null)
        {
            this.card_info_txt.text = $"{card.name}\nHealth: {card.health}\nAttack: {card.attack}\n{card.abilitytxt}";
            thisCardAtk = card.attack;
            thisCardHlt = card.health;
        }

        cardGameManager = FindObjectOfType<CardGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cardGameManager.currentPhase == CardGameManager.GamePhase.Action && !hasSurroundingInfo && gameObject.transform.parent.CompareTag("GridSlot"))
        {
            cardGridPos = gameObject.transform.parent.gameObject;
            getSurroundingCardsInfo();
            Debug.Log("card have recieved surrounding info");

            if (!isExecutingAbility && cardGameManager.currentPhase == CardGameManager.GamePhase.Action)
            {
                StartCoroutine(ExecuteCardAbility());
            }
        }

        
    }

    IEnumerator ExecuteCardAbility()
    {
        
        yield return new WaitForSeconds(1f);

        switch (card.type)
        {
            case CardData.CardType.DirectDamage:
                if (card.CardName == "ArrowShot")
                {
                    attackCard(rightOfThisCard);
                    isExecutingAbility = true;
                }
                if (card.CardName == "Fireball")
                {
                    attackCard(frontOfThisCard);
                    isExecutingAbility = true;
                }
                break;
                /*......*/
        }

        isExecutingAbility = false;
    }

    public void getSurroundingCardsInfo()
    {
        int[] slotsWithAvailableRightSlots = { 0, 1, 3, 4, 6, 7 };
        int[] slotsWithAvailableLeftSlots = { 1, 2, 4, 5, 7, 8 };
        int[] slotsWithAvailableForwardSlots = { 3, 4, 5, 6, 7, 8 };
        int[] slotsWithAvailableBehindSlots = { 0, 1, 2, 3, 4, 5 };

        int index = cardGameManager.gridSlots.IndexOf(cardGridPos);
        if (index != -1)
        {
            if (slotsWithAvailableRightSlots.Contains(index) && index + 1 < cardGameManager.gridSlots.Count)
            {
                if (cardGameManager.gridSlots[index + 1].GetComponent<SlotController>().isOccupied)
                {
                    rightOfThisCard = cardGameManager.gridSlots[index + 1].GetComponent<SlotController>().occupiedCardObject;
                }
            }
            if (slotsWithAvailableLeftSlots.Contains(index) && index - 1 >= 0)
            {
                if (cardGameManager.gridSlots[index - 1].GetComponent<SlotController>().isOccupied)
                {
                    leftOfThisCard = cardGameManager.gridSlots[index - 1].GetComponent<SlotController>().occupiedCardObject;
                }
            }
            if (slotsWithAvailableForwardSlots.Contains(index) && index - 3 < cardGameManager.gridSlots.Count)
            {
                if (cardGameManager.gridSlots[index - 3].GetComponent<SlotController>().isOccupied)
                {
                    frontOfThisCard = cardGameManager.gridSlots[index - 3].GetComponent<SlotController>().occupiedCardObject;
                }
            }
            if (slotsWithAvailableBehindSlots.Contains(index) && index + 3 >= 0)
            {
                if (cardGameManager.gridSlots[index + 3].GetComponent<SlotController>().isOccupied)
                {
                    behindThisCard = cardGameManager.gridSlots[index + 3].GetComponent<SlotController>().occupiedCardObject;
                }
            }
        }

        hasSurroundingInfo = true;
    }


    public void attackCard(GameObject targetcard)
    {
        if (frontOfThisCard != null)
        {
            int dmgAmount = thisCardAtk;
            targetcard.GetComponent<CardController>().thisCardHlt -= dmgAmount;
            Debug.Log(dmgAmount + "of damage has been dealt to" + targetcard.name + "by" + gameObject.name);
            if (targetcard.GetComponent<CardController>().thisCardHlt <= 0)
            {
                Debug.Log(targetcard + "has been destroyed");
            }
        }
        else
        {
            Debug.LogError("frontOfThisCard has not been assigned");
        }
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
