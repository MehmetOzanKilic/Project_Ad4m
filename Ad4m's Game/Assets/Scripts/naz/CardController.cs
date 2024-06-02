using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static CardGameManager;
using System.Linq;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;

public class CardController : MonoBehaviour
{
    [SerializeField] public CardData card;
    public TextMeshProUGUI card_info_txt;

    public CardGameManager cardGameManager;
    public GameObject cardOnTopofQueue;

    public GameObject cardGridPos;

    public bool hasSurroundingInfo = false;
    public GameObject rightOfThisCard;
    public GameObject leftOfThisCard;
    public GameObject frontOfThisCard;
    public GameObject behindThisCard;
    public List<GameObject> CardsOnTheSameRow;

    int thisCardAtk;
    int thisCardHlt;
    int thisCardHealAmt;
    int thisCardBuffAmt;

    public bool isExecutingAbility = false;
    public bool isFriendlyCard = true;
    public bool isTheTopCardInQueue = false;

    public bool playedThisRound = false;

    public GameObject lastCardToAttack;


    // Start is called before the first frame update
    void Start()
    {
        if (card != null && card_info_txt != null)
        {
            thisCardAtk = card.attack;
            thisCardHlt = card.health;
            thisCardHealAmt = card.healingAmount;
            thisCardBuffAmt = card.buffingAmount;
        }
        
       

        cardGameManager = FindObjectOfType<CardGameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cardGameManager.currentPhase == CardGameManager.GamePhase.Action && !hasSurroundingInfo && gameObject.transform.parent.CompareTag("GridSlot"))
        {
            cardGridPos = gameObject.transform.parent.gameObject;
            getSurroundingCardsInfo();

            //cardOnTopofQueue = cardGameManager.cardsPlayed.First();

            if (isTheTopCardInQueue && !isExecutingAbility && !playedThisRound)
            {
                Debug.Log("Starting ExecuteCardAbility coroutine for " + card.name);
                StartCoroutine(ExecuteCardAbility());
            }

        }
        if (card != null && card_info_txt != null)
        {
            this.card_info_txt.text = $"{card.name}\nHealth: {thisCardHlt}\nAttack: {thisCardAtk}\n{card.abilitytxt}";
            //this.card_info_txt.rectTransform
        }
    }



    IEnumerator ExecuteCardAbility()
    {
        //yield return new WaitForSeconds(1f);
        Debug.Log("retreiving card type and executing action...");
        switch (card.type)
        {
            case CardData.CardType.DirectDamage:
                if (card.CardName == "ArrowShot")
                {
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(rightOfThisCard);
                    }

                }
                if (card.CardName == "Fireball")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(frontOfThisCard);
                    }

                }
                break;
            case CardData.CardType.AOEDmg:
                if (card.CardName == "Explosion")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(frontOfThisCard);
                    }

                    if (behindThisCard != null && behindThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(behindThisCard);
                    }
                    if (leftOfThisCard != null && leftOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(rightOfThisCard);
                    }
                }
                if (card.CardName == "Thunderstorm")
                {
                    attackAOE();
                }
                break;
            case CardData.CardType.Healing:
                if (card.CardName == "Healing Aura")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        healCard(frontOfThisCard);
                    }
                    if (behindThisCard != null && behindThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        healCard(behindThisCard);
                    }
                    if (leftOfThisCard != null && leftOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        healCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        healCard(rightOfThisCard);
                    }
                }
                if (card.CardName == "Regeneration")
                {
                    healCard(this.gameObject);
                }
                break;
            case CardData.CardType.BuffDebuff:
                if (card.CardName == "Strength Boost")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        buffDebuffCard(frontOfThisCard);
                    }
                    if (behindThisCard != null && behindThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        buffDebuffCard(behindThisCard);
                    }
                    if (leftOfThisCard != null && leftOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        buffDebuffCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard == isFriendlyCard)
                    {
                        buffDebuffCard(rightOfThisCard);
                    }
                }
                if (card.CardName == "Weakening Curse")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        buffDebuffCard(frontOfThisCard);
                    }
                    if (behindThisCard != null && behindThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        buffDebuffCard(behindThisCard);
                    }
                    if (leftOfThisCard != null && leftOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        buffDebuffCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        buffDebuffCard(rightOfThisCard);
                    }
                }
                break;
            case CardData.CardType.CounterAttack:
                if (card.CardName == "Retaliation")
                {
                    if (lastCardToAttack != null)
                    {
                        attackCard(lastCardToAttack);

                    }
                }
                break;
            case CardData.CardType.CardSacrifice:
                if(card.name == "Explosive Sacrifice")
                {
                    if (frontOfThisCard != null && frontOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(frontOfThisCard);
                    }

                    if (behindThisCard != null && behindThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(behindThisCard);
                    }
                    if (leftOfThisCard != null && leftOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(leftOfThisCard);
                    }
                    if (rightOfThisCard != null && rightOfThisCard.GetComponent<CardController>().isFriendlyCard != isFriendlyCard)
                    {
                        attackCard(rightOfThisCard);
                    }
                    destroyCard(gameObject);
                }
                if(card.name == "Last Stand")
                {
                    attackCard(lastCardToAttack);
                    destroyCard(gameObject);
                }
                break;
            case CardData.CardType.Blocking:
                if(card.name == "Guardian Shield")
                {
                    //code for dmg blocking
                }
                break;
        }
        yield return new WaitForSeconds(1f);
        Debug.Log(name + "has executed its ability");
        playedThisRound = true;
        Debug.Log("moving card..");

        yield return new WaitForSeconds(2f);
        cardGameManager.cardsPlayed.Enqueue(cardGameManager.cardsPlayed.First());
        cardGameManager.cardsPlayed.First().GetComponent<CardController>().isTheTopCardInQueue = false;
        cardGameManager.cardsPlayed.Dequeue();
        Debug.Log("card moved");
        cardGameManager.cardsPlayed.First().GetComponent<CardController>().isTheTopCardInQueue = true;

        foreach (GameObject cardGameObject in cardGameManager.cardsPlayed)
        {
            cardGameObject.GetComponent<CardController>().cardOnTopofQueue = cardGameManager.cardsPlayed.First();
            cardGameObject.GetComponent<CardController>().hasSurroundingInfo = false;

            
        }
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
                        if (!CardsOnTheSameRow.Contains(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject))
                        {
                            CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);

                        }
                    }
                }

            }
            else if (cardGameManager.gridSlots[index].name.StartsWith("(2,"))
            {
                for (int i = 3; i <= 5; i++)
                {
                    if (cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject != null)
                    {
                        if (!CardsOnTheSameRow.Contains(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject))
                        {
                            CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);

                        }
                    }
                }
            }
            else if (cardGameManager.gridSlots[index].name.StartsWith("(3,"))
            {
                for (int i = 6; i <= 8; i++)
                {
                    if (cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject != null)
                    {
                        if (!CardsOnTheSameRow.Contains(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject))
                        {
                            CardsOnTheSameRow.Add(cardGameManager.gridSlots[i].GetComponent<SlotController>().occupiedCardObject);

                        }
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
            targetcard.GetComponent<CardController>().lastCardToAttack = gameObject;
            isExecutingAbility = true;

            Debug.Log(dmgAmount + " of damage has been dealt to " + targetcard.name + " by " + gameObject.name);
            if (targetcard.GetComponent<CardController>().thisCardHlt <= 0)
            {
            Debug.Log(targetcard + "has been destroyed");
            destroyCard(targetcard);
           
        }

 
    }

    public void attackAOE()
    {
        if(CardsOnTheSameRow.Count > 0)
        {
            for (int i = 0; i < CardsOnTheSameRow.Count; i++)
            {
                if (CardsOnTheSameRow[i] != this.gameObject && !CardsOnTheSameRow[i].GetComponent<CardController>().isFriendlyCard)
                {
                    attackCard(CardsOnTheSameRow[i]);
                }
            }
        }
 
    }

    public void healCard(GameObject targetcard)
    {
        int healAmount = thisCardHealAmt;
        targetcard.GetComponent<CardController>().thisCardHlt += healAmount;
        isExecutingAbility = true;

        Debug.Log(healAmount + " of health has been healed to " + targetcard.name + " by " + gameObject.name);

    }

    public void buffDebuffCard(GameObject targetcard)
    {
        int buffdebuffamt = thisCardBuffAmt;
        targetcard.GetComponent<CardController>().thisCardAtk += buffdebuffamt;
        isExecutingAbility = true;

        Debug.Log(buffdebuffamt + " of atk has been buffed to " + targetcard.name + " by " + gameObject.name);
    }
    public void blockDmg()
    {

    }

    public void destroyCard(GameObject targetcard)
    {
        targetcard.transform.parent = null;

        cardGameManager.cardsPlayed = new Queue<GameObject>(cardGameManager.cardsPlayed.Where(x => x.gameObject != targetcard));
        
        Destroy(targetcard);
        
    }

}
