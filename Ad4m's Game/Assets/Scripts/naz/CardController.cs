using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardController : MonoBehaviour
{
    [SerializeField] public CardData card;
    public TextMeshProUGUI card_info_txt;

    // Start is called before the first frame update
    void Start()
    {
        if (card != null && card_info_txt != null)
        {
            this.card_info_txt.text = $"{card.name}\nHealth: {card.health}\nAttack: {card.attack}\n{card.abilitytxt}";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
