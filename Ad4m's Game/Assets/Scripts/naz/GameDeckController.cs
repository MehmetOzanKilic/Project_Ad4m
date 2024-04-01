using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckController : MonoBehaviour
{
    public  List<GameObject> deckCards = new List<GameObject>();
    public CardGameManager cardGameManager;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            deckCards.Add(gameObject.transform.GetChild(i).gameObject);
        }

        ShuffleDeck();
        cardGameManager.InitializePlayerHand();
        cardGameManager.InitializeOpponentHand();
        StackCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StackCards()
    {
        float yOffset = 0f;
        foreach (GameObject card in deckCards)
        {
            card.transform.localPosition = new Vector3(0f, yOffset, 0f);
            yOffset += 0.03f;
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            int randomIndex = Random.Range(i, deckCards.Count);
            GameObject temp = deckCards[randomIndex];
            deckCards[randomIndex] = deckCards[i];
            deckCards[i] = temp;
        }

        StackCards();
    }

    public void GiveCard(List<GameObject> whichPlayersDeck) //NOT BEING USED RN
    {
        whichPlayersDeck.Add(deckCards[0]);
        deckCards.Remove(deckCards[0]);
        StackCards();
    }
}
