using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckController : MonoBehaviour
{
    public Transform gameDeckParent;
    public  List<Transform> deckCards = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        gameDeckParent = transform;

        foreach (Transform cardTransform in gameDeckParent)
        {
            deckCards.Add(cardTransform);
        }

        StackCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StackCards()
    {
        float yOffset = 0f;
        foreach (Transform cardTransform in deckCards)
        {
            cardTransform.localPosition = new Vector3(0f, yOffset, 0f);
            yOffset += 0.035f;
        }
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            int randomIndex = Random.Range(i, deckCards.Count);
            Transform temp = deckCards[randomIndex];
            deckCards[randomIndex] = deckCards[i];
            deckCards[i] = temp;
        }

        StackCards();
    }
}
