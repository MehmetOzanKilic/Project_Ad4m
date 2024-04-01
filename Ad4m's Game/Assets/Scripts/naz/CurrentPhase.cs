using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentPhase : MonoBehaviour
{
    public CardGameManager cardGameManager;
    public TextMeshProUGUI phasetextinfo;
    // Start is called before the first frame update
    void Start()
    {
        phasetextinfo = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cardGameManager != null)
        {
            phasetextinfo.text = $"Current Phase: {cardGameManager.currentPhase}";
        }
    }
}
