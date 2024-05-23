using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(CardGameManager))]
public class CardGameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CardGameManager cardGameManager = (CardGameManager)target;

        GUILayout.Space(10);

        GUILayout.Label("Cards Played Queue", EditorStyles.boldLabel);

        foreach (GameObject card in cardGameManager.cardsPlayed)
        {
            EditorGUILayout.ObjectField(card, typeof(GameObject), true);
        }
    }
}

public class CardGameManager : MonoBehaviour
{
    public enum GamePhase
    {
        PlayerTurn,
        EnemyTurn,
        FightingStage,
        SlotSelection,
        Action
    }

    public GamePhase currentPhase ;

    public Transform playerHandParent;
    public List<GameObject> playerCards;
    public GameObject[] playerCardsPositions;

    public Transform opponentHandParent;
    public List<GameObject> opponentCards;
    public GameObject[] opponentCardsPositions;
    public EnemyController enemyController;

    public List<GameObject> gridSlots;

    private int selectedCardIndex = 0;
    private int selectedGridIndex = 4;
    public float movementSpeed = 5f;
    public GameObject selectedCard;

    int playedCardCount = 0;

    public CinemachineVirtualCamera deckViewCam;
    public CinemachineVirtualCamera topViewCam;

    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;
    private bool isPlacingCard = false;

    public GameDeckController gameDeckController;

    public Queue<GameObject> cardsPlayed = new Queue<GameObject>();
    //public GameObject currentCardExecutingAbility;

    public List<GameObject> emptySlots;

    void Start()
    {   getPhase();
        SwitchCamera();

        for (int i = 0; i < gridSlots.Count; i++)
        {
            emptySlots.Add(gridSlots[i]);
        }

        //for debugging
        SelectedSections.printAllSections();
    }

    void getPhase()
    {
        currentPhase = (GamePhase)System.Enum.Parse(typeof(GamePhase), StateController.gamePhase);
        print("current pahse is: " + currentPhase);
    }

    void Update()
    {
        switch (currentPhase)
        {
            case GamePhase.PlayerTurn:
                HandleTurnPhase();
                break;
            case GamePhase.EnemyTurn:
                enemyController.EnemyTurnHandler();
                break;
            case GamePhase.FightingStage:
                Invoke("FightingStagePhase", 2);
                print("switching to fighting stage");
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

    public void InitializePlayerHand()
    {
        for (int i = 0; i < 3; i++)
        {
            playerCards.Add(gameDeckController.deckCards[i]);
            gameDeckController.deckCards.Remove(gameDeckController.deckCards[i]);
            playerCards[i].transform.parent = null;

            playerCards[i].transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            playerCards[i].transform.position = playerCardsPositions[i].transform.position;

            playerCards[i].transform.parent = playerCardsPositions[i].transform;

            // Get the target position and rotation
            //Vector3 targetPosition = cardsPositions[i].transform.position;
            //Quaternion targetRotation = Quaternion.Euler(0f, 0f, 45f);

            // Smoothly move and rotate

            /*float smoothMovementSpeed = 10f;
            float smoothRotationSpeed = 10f;
            cards.Add(gameDeckController.deckCards[i]);
            gameDeckController.deckCards.Remove(gameDeckController.deckCards[i]);
            cards[i].transform.parent = null;

            cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, targetPosition, smoothMovementSpeed * Time.deltaTime);
            cards[i].transform.rotation = Quaternion.Lerp(cards[i].transform.rotation, targetRotation, smoothRotationSpeed * Time.deltaTime);

            cards[i].transform.parent = cardsPositions[i].transform;*/
        }
    }

    public void InitializeOpponentHand()
    {
        for (int i = 0; i < 3; i++)
        {
            opponentCards.Add(gameDeckController.deckCards[i]);
            gameDeckController.deckCards.Remove(gameDeckController.deckCards[i]);
            opponentCards[i].transform.parent = null;

            opponentCards[i].transform.rotation = Quaternion.Euler(0f, 180f, 45f);
            opponentCards[i].transform.position = opponentCardsPositions[i].transform.position;

            opponentCards[i].transform.parent = opponentCardsPositions[i].transform;
            opponentCards[i].GetComponent<CardController>().isFriendlyCard = false;
        }
    }

    void HandleTurnPhase() //TURN PHASE
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentPhase = GamePhase.SlotSelection;
            SwitchCamera();
            selectedCard = playerCards[selectedCardIndex];// FOR DEBUG
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
    [SerializeField] private GameObject turnOnOff;
    void FightingStagePhase()
    {
        print("fightingStage");
        string levelString = "Level" + SelectedSections.returnCount().ToString();
        SceneManager.LoadScene(levelString);
    }

    void HandleSlotSelectionPhase()
    {
        playerCards[selectedCardIndex].transform.rotation = Quaternion.Euler(0f, 0f, 90f);

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
            if (playerCards.Count > 0 && selectedCardIndex < playerCards.Count)
            {
                playedCardCount++;
                isPlacingCard = false;
                emptySlots.Remove(gridSlots[selectedGridIndex]);

                Vector3 targetPositionGrid = GetGridSlotPosition(selectedGridIndex);
                MoveCardToPosition(selectedCardIndex, targetPositionGrid);

                cardsPlayed.Enqueue(playerCards[selectedCardIndex]); //ENQUEUE


                playerCards[selectedCardIndex].transform.SetParent(gridSlots[selectedGridIndex].transform);
                playerCards[selectedCardIndex].transform.rotation = Quaternion.Euler(0f, 0f, 90f);

                playerCards.RemoveAt(selectedCardIndex);

                RearrangePlayerHand();
                selectedCardIndex = 0;

                //AddCardToDeck();

                if (playedCardCount == 2)
                {
                    currentPhase = GamePhase.EnemyTurn;
                    playedCardCount = 0;
                    SwitchCamera();
                }
                else
                {
                    currentPhase = GamePhase.PlayerTurn;
                    SwitchCamera();
                }
            }
            else
            {
                Debug.LogWarning("invalid card index.");
            }
        }
        else
        {
            Vector3 targetPosition = GetInitialSlotPosition();
            MoveCardToPosition(selectedCardIndex, targetPosition);
        }
    }

    Vector3 GetInitialSlotPosition()
    {
        //If the middle slot is unoccupied, put the card in the middle slot.
        if (!gridSlots[selectedGridIndex].GetComponent<SlotController>().isOccupied)
        {
            return GetGridSlotPosition(selectedGridIndex) + new Vector3(0f, 1f, 0f);
        }
        else if (gridSlots[selectedGridIndex].GetComponent<SlotController>().isOccupied) //If the middle slot is occupied, put it on an unoccupied adjacent slot
        {
            int[] adjacentSlots = { selectedGridIndex - 1, selectedGridIndex + 1, selectedGridIndex - 3, selectedGridIndex + 3 };
            foreach (int index in adjacentSlots)
            {
                if (index >= 0 && index < 9 && !gridSlots[index].GetComponent<SlotController>().isOccupied)
                {
                    selectedGridIndex = index;
                    return GetGridSlotPosition(index) + new Vector3(0f, 1f, 0f);
                }
            }
        }
        else
        {
            // Check corner slots
            int[] cornerSlots = { 0, 2, 6, 8 };
            foreach (int index in cornerSlots)
            {
                if (!gridSlots[index].GetComponent<SlotController>().isOccupied)
                {
                    selectedGridIndex = index;
                    return GetGridSlotPosition(index) + new Vector3(0f, 1f, 0f);
                }
            }
        }


        return GetGridSlotPosition(selectedGridIndex) + new Vector3(0f, 1f, 0f);
    }

    void HandleActionPhase()
    {
        SwitchCamera();
        cardsPlayed.First().GetComponent<CardController>().isTheTopCardInQueue = true;

        if (checkIfAllCardsPlayed())
        {
            
            //
        }
    }

    bool checkIfAllCardsPlayed()
    {
        foreach (GameObject cardGameObject in cardsPlayed)
        {
            if (!cardGameObject.GetComponent<CardController>().playedThisRound)
            {
                return false;
            }
        }
        return true;
    }

    void RearrangePlayerHand()
    {
        //float smoothMovementSpeed = 5f;
        for (int i = 0; i < playerCards.Count; i++)
        {
            playerCards[i].transform.parent = null;
            playerCards[i].transform.position = playerCardsPositions[i].transform.position;
            playerCards[i].transform.parent = playerCardsPositions[i].transform;
            /*cards[i].transform.parent = null;
            cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, cardsPositions[i].transform.position, smoothMovementSpeed * Time.deltaTime);
            cards[i].transform.parent = cardsPositions[i].transform;*/
        }
    }

    void Hover() //hovering
    {
        for (int i = 0; i < playerCards.Count; i++)
        {
            Vector3 targetPosition = playerCards[i].transform.localPosition;
            if (i == selectedCardIndex && !isPlacingCard)
            {
                targetPosition.y = hoverHeight;
            }
            else
            {
                targetPosition.y = 0f;
            }
            playerCards[i].transform.localPosition = Vector3.Lerp(playerCards[i].transform.localPosition, targetPosition, hoverSpeed * Time.deltaTime);
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
        if (cardIndex >= 0 && cardIndex < playerCards.Count) // Check if index is within range
        {
            playerCards[cardIndex].transform.position = Vector3.MoveTowards(playerCards[cardIndex].transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Invalid card index: " + cardIndex);
        }
    }


    void MoveRight()
    {
        selectedCardIndex = (selectedCardIndex + 1) % playerCards.Count;
    }

    void MoveLeft()
    {
        selectedCardIndex = (selectedCardIndex - 1 + playerCards.Count) % playerCards.Count;
    }


    void MoveUp()
    {
        int newIndex = selectedGridIndex - 3;
        if (newIndex >= 0 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveDown()
    {
        int newIndex = selectedGridIndex + 3;
        if (newIndex < 9 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveLeftOnGrid()
    {
        int newIndex = selectedGridIndex - 1;
        if (newIndex >= 0 && newIndex / 3 == selectedGridIndex / 3 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            selectedGridIndex = newIndex;
        }
    }

    void MoveRightOnGrid()
    {
        int newIndex = selectedGridIndex + 1;
        if (newIndex < 9 && newIndex / 3 == selectedGridIndex / 3 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            selectedGridIndex = newIndex;
        }
    }

    public void SwitchCamera()
    {
        if (currentPhase == GamePhase.SlotSelection || currentPhase == GamePhase.Action)
        {
            deckViewCam.Priority = 0;
            topViewCam.Priority = 10;
        }
        else if(currentPhase == GamePhase.PlayerTurn || currentPhase == GamePhase.EnemyTurn) 
        {
            deckViewCam.Priority = 10;
            topViewCam.Priority = 0;
        }
    }

}


