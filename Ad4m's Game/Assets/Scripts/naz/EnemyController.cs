using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public CardGameManager cardGameManager;
    private int playedCardCount = 0;
    private bool isPlacingCard = false;
    public float cardPlacementDelay = 1f; 

    void Update()
    {
        if (cardGameManager.currentPhase == CardGameManager.GamePhase.EnemyTurn && !isPlacingCard)
        {
            StartCoroutine(EnemyTurnHandler());
        }
    }

    public IEnumerator EnemyTurnHandler()
    {
        isPlacingCard = true;

        if (playedCardCount < 2)
        {
            if (ChooseCardFromHand() != null && ChooseSlotToPlaceCard() != null)
            {
                yield return new WaitForSeconds(cardPlacementDelay);
                yield return StartCoroutine(PlaceCard(ChooseSlotToPlaceCard(), ChooseCardFromHand()));
                playedCardCount++;
            }
        }
        else
        {
            cardGameManager.currentPhase = CardGameManager.GamePhase.Action;
            playedCardCount = 0;
        }

        isPlacingCard = false;
    }

    GameObject ChooseCardFromHand()
    {
        if (cardGameManager.opponentCards.Count > 0)
        {
            int randomIndex = Random.Range(0, cardGameManager.opponentCards.Count);
            return cardGameManager.opponentCards[randomIndex];
        }
        else
        {
            return null;
        }
    }

    GameObject ChooseSlotToPlaceCard()
    {
        // If there are no empty slots, return null
        if (cardGameManager.emptySlots.Count == 0)
        {
            return null;
        }
        else
        {
            int randomIndex = Random.Range(0, cardGameManager.emptySlots.Count);
            GameObject selectedSlot = cardGameManager.emptySlots[randomIndex];
            cardGameManager.emptySlots.Remove(selectedSlot);
            return selectedSlot;
        }
    }


    IEnumerator PlaceCard(GameObject slot, GameObject card)
    {
        cardGameManager.opponentCards.Remove(card);
        
        card.transform.parent = null;
        card.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        Vector3 targetPosition = slot.transform.position + Vector3.up * 1f;
        Vector3 initialPosition = card.transform.position;
        float elapsedTime = 0f;
        float smoothTime = 1f;

        while (elapsedTime < smoothTime)
        {
            card.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardGameManager.cardsPlayed.Enqueue(card); // ENQUEUE

        card.transform.position = targetPosition;
       
        card.transform.SetParent(slot.transform);
        slot.GetComponent<SlotController>().isOccupied = true;
        
    }
}
