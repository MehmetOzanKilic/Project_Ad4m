using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CardGameManager : MonoBehaviour
{
    public enum GamePhase
    {
        Turn,
        SlotSelection,
        Action
    }

    public GamePhase currentPhase = GamePhase.Turn;

    public Transform playerHandParent;
    public GameObject[] cards;
    public GameObject[] gridSlots;

    private int selectedCardIndex = 0;
    private int selectedGridIndex = 4;
    public float movementSpeed = 5f;
    
    

    public CinemachineVirtualCamera deckViewCam;
    public CinemachineVirtualCamera topViewCam;

    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;
    private bool isPlacingCard = false;

    void Start()
    {
        InitializePlayerHand();
        SwitchCamera();
    }

    void Update()
    {
        switch (currentPhase)
        {
            case GamePhase.Turn:
                HandleTurnPhase();
                break;
            case GamePhase.SlotSelection:
                HandleSlotSelectionPhase();
                break;
            case GamePhase.Action:
                HandleActionPhase();
                break;
        }

        Hover();
    }

    void InitializePlayerHand()
    {
        List<GameObject> cardObjects = new List<GameObject>();
        foreach (Transform cardPos in playerHandParent)
        {
            if (cardPos.childCount > 0)
            {
                cardObjects.Add(cardPos.GetChild(0).gameObject);
            }
        }
        cards = cardObjects.ToArray();
    }


    void HandleTurnPhase() //TURN PHASE
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPhase = GamePhase.SlotSelection;
            SwitchCamera();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
        }
    }

    void HandleSlotSelectionPhase() //SLOT SELECTION PHASE
    {
        Vector3 targetPosition = GetGridSlotPosition(selectedGridIndex);
        MoveCardToPosition(selectedCardIndex, targetPosition);
        cards[selectedCardIndex].transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeftOnGrid();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightOnGrid();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPhase = GamePhase.Action;
        }
    }


    void HandleActionPhase() //ACTION PHASE
    {
        isPlacingCard = false;
        Vector3 targetPosition = GetGridSlotPosition(selectedGridIndex);

        MoveCardToPosition(selectedCardIndex, targetPosition);
        cards[selectedCardIndex].transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        cards[selectedCardIndex].transform.SetParent(gridSlots[selectedGridIndex].transform);

        List<GameObject> cardList = new List<GameObject>(cards);
        cardList.RemoveAt(selectedCardIndex);
        cards = cardList.ToArray();

        currentPhase = GamePhase.Turn;
        SwitchCamera();
    }

    void Hover() //hovering
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Vector3 targetPosition = cards[i].transform.localPosition;
            if (i == selectedCardIndex && !isPlacingCard)
            {
                targetPosition.y = hoverHeight;
            }
            else
            {
                targetPosition.y = 0f;
            }
            cards[i].transform.localPosition = Vector3.Lerp(cards[i].transform.localPosition, targetPosition, hoverSpeed * Time.deltaTime);
        }
    }

    Vector3 GetGridSlotPosition(int index)
    {
        if (index >= 0 && index < 9)
        {
            return gridSlots[index].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void MoveCardToPosition(int cardIndex, Vector3 targetPosition)
    {
        cards[cardIndex].transform.position = Vector3.MoveTowards(cards[cardIndex].transform.position, targetPosition, movementSpeed * Time.deltaTime);
    }


    void MoveRight()
    {
        selectedCardIndex = (selectedCardIndex + 1) % cards.Length;
    }

    void MoveLeft()
    {
        selectedCardIndex = (selectedCardIndex - 1 + cards.Length) % cards.Length;
    }

    void MoveUp()
    {
        int newIndex = selectedGridIndex - 3;
        if (newIndex >= 0)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveDown()
    {
        int newIndex = selectedGridIndex + 3;
        if (newIndex < 9)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveLeftOnGrid()
    {
        int newIndex = selectedGridIndex - 1;
        if (newIndex >= 0 && newIndex / 3 == selectedGridIndex / 3)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveRightOnGrid()
    {
        int newIndex = selectedGridIndex + 1;
        if (newIndex < 9 && newIndex / 3 == selectedGridIndex / 3)
        {
            selectedGridIndex = newIndex;
        }
    }

    void SwitchCamera()
    {
        if (currentPhase == GamePhase.Turn)
        {
            topViewCam.Priority = 0;
            deckViewCam.Priority = 10;
        }
        else
        {
            deckViewCam.Priority = 0;
            topViewCam.Priority = 10;
        }
    }
}
