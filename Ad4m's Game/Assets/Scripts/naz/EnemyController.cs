using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public CardGameManager cardGameManager;
    private int playedCardCount = 0;
    private bool isPlacingCard = false;
    public float cardPlacementDelay = 1.5f; 

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
            GameObject cardToPlace = ChooseCardFromHand();
            GameObject slotToPlaceCard = ChooseSlotToPlaceCard();

            if (cardToPlace != null && slotToPlaceCard != null)
            {
                yield return new WaitForSeconds(cardPlacementDelay);
                yield return StartCoroutine(PlaceCard(slotToPlaceCard, cardToPlace));

                cardGameManager.cardsPlayed.Enqueue(cardToPlace); // ENQUEUE

                playedCardCount++;
            }
        }
        else
        {
            // End the enemy's turn
            yield return new WaitForSeconds(cardPlacementDelay);
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
        List<GameObject> emptySlots = new List<GameObject>();

        foreach (GameObject slot in cardGameManager.gridSlots)
        {
            if (!slot.GetComponent<SlotController>().isOccupied)
            {
                emptySlots.Add(slot);
            }
        }

        // If there are no empty slots, return null
        if (emptySlots.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, emptySlots.Count);
        GameObject selectedSlot = emptySlots[randomIndex];
        if (selectedSlot.GetComponent<SlotController>().isOccupied)
        {
            return ChooseSlotToPlaceCard();
        }
        else
        {
            return selectedSlot;
        }
    }


    IEnumerator PlaceCard(GameObject slot, GameObject card)
    {
        if (slot.GetComponent<SlotController>().isOccupied)
        {
            GameObject newSlot = ChooseSlotToPlaceCard();
            if (newSlot != null)
            {
                slot = newSlot;
            }
            else
            {
                // No empty slots available, return
                yield break;
            }
        }

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

        card.transform.position = targetPosition;
        card.transform.SetParent(slot.transform);
        slot.GetComponent<SlotController>().isOccupied = true;
    }
}
