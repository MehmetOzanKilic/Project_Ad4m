using UnityEngine;

public class SlotController : MonoBehaviour
{
    public bool isOccupied = false;
    public GameObject occupiedCardObject;
    public CardData occupiedCardData;
    public int occupiedCardHealth;
    public int occupiedCardAttack;

    void Update()
    {
        isOccupied = transform.childCount > 1 && transform.GetChild(1).CompareTag("Card");

        if (isOccupied)
        {
            occupiedCardObject = transform.GetChild(1).gameObject;
            getCardInfo(occupiedCardObject);
        }
    }

    void getCardInfo(GameObject occupiedCardObject)
    {
        CardController cardController = occupiedCardObject.GetComponent<CardController>();
        if (cardController != null)
        {
            occupiedCardData = cardController.card;
            if (occupiedCardData != null)
            {
                occupiedCardHealth = occupiedCardData.health;
                occupiedCardAttack = occupiedCardData.attack;
            }
        }
    }
}
