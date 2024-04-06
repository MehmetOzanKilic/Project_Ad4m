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
    public List<GameObject> CardsOnTheSameRow;

    int thisCardAtk;
    int thisCardHlt;

    private bool isExecutingAbility = false;
    public bool isFriendlyCard = true;

    // Start is called before the first frame update
    void Start()
    {
        if (card != null && card_info_txt != null)
        {
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

        if (card != null && card_info_txt != null)
        {
            this.card_info_txt.text = $"{card.name}\nHealth: {thisCardHlt}\nAttack: {thisCardAtk}\n{card.abilitytxt}";
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
                    if(rightOfThisCard != null)
                    {
                        attackCard(rightOfThisCard);
                    }
                    isExecutingAbility = true;
                }
                if (card.CardName == "Fireball")
                {
                    if(frontOfThisCard != null)
                    {
                        attackCard(frontOfThisCard);
                    }
                    isExecutingAbility = true;
                }
                break;
            case CardData.CardType.AOEDmg:
                if(card.CardName == "Explosion")
                {
                    if(frontOfThisCard != null)
                    {
                        attackCard(frontOfThisCard);
                    }
                    if (behindThisCard != null)
                    {
                        attackCard(behindThisCard);
                    }
                    if (leftOfThisCard != null)
                    {
                        attackCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null)
                    {
                        attackCard(rightOfThisCard);
                    }
                }
                if(card.CardName == "Thunderstorm")
                {
                    attackAOE();
                }
                break;

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


            if (cardGameManager.gridSlots[index].name.StartsWith("(1,"))
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject != null)
                    {
                        CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);
                    }
                }

            }
            else if (cardGameManager.gridSlots[index].name.StartsWith("(2,"))
            {
                for (int i = 3; i <= 5; i++)
                {
                    if (cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject != null)
                    {
                        CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);
                    }
                }
            }
            else if (cardGameManager.gridSlots[index].name.StartsWith("(3,"))
            {
                for (int i = 6; i <= 8; i++)
                {
                    if (cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject != null)
                    {
                        CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);
                    }
                }
            }
        }

        hasSurroundingInfo = true;
    }


    public void attackCard(GameObject targetcard)
    {
            int dmgAmount = thisCardAtk;
            targetcard.GetComponent<CardController>().thisCardHlt -= dmgAmount;
            Debug.Log(dmgAmount + " of damage has been dealt to " + targetcard.name + " by " + gameObject.name);
            if (targetcard.GetComponent<CardController>().thisCardHlt <= 0)
            {
                Debug.Log(targetcard + "has been destroyed");
            }
    }

    public void attackAOE()
    {
        if(CardsOnTheSameRow.Count > 0)
        {
            for (int i = 0; i < CardsOnTheSameRow.Count; i++)
            {
                if (CardsOnTheSameRow[i] != this.gameObject)
                {
                    attackCard(CardsOnTheSameRow[i]);
                }
            }
        }
 
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
