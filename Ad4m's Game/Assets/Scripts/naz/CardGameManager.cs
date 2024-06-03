using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
/*[CustomEditor(typeof(CardGameManager))]
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
}*/

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
    [SerializeField]private GameObject escCanvas;
    private bool escFlag = false;
    public Transform playerHandParent;
    public List<GameObject> playerCards;
    public GameObject[] playerCardsPositions;

    public Transform opponentHandParent;
    public List<GameObject> opponentCards;
    public GameObject[] opponentCardsPositions;
    public EnemyController enemyController;

    public List<GameObject> gridSlots;

    private int selectedCardIndex = 0;
    //public int selectedGridIndex = 4;
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
    {   
        SelectedUpgrades.reset();
        escCanvas.SetActive(false);
        getPhase();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(escFlag==false)
            {
                escCanvas.SetActive(true);
                escFlag=true;
                Time.timeScale=0;
                Cursor.lockState = CursorLockMode.None;
            }

            else if(escFlag==true)
            {
                escCanvas.SetActive(false);
                escFlag=false;
                Time.timeScale=1;
                if(SelectedSections.returnCount()>3)Cursor.lockState = CursorLockMode.Locked;
            }
            
        }

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

    public void drawOpponentHand()
    {
        if(opponentCards.Count() ==1)
        {
            for (int i = 1; i < 3; i++)
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
        
    }

    void HandleTurnPhase() //TURN PHASE
    {   
        if(playerCards.Count==1)EnsurePlayerHasThreeCards();

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

    public void DrawCard()
    {
        if (gameDeckController.deckCards.Count > 0)
        {
            GameObject card = gameDeckController.deckCards[0];
            gameDeckController.deckCards.RemoveAt(0);
            playerCards.Add(card);

            // Set the card's parent to the hand and adjust its position
            card.transform.parent = playerHandParent;
            card.transform.position = playerCardsPositions[playerCards.Count - 1].transform.position;
            card.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            card.transform.SetParent(playerCardsPositions[playerCards.Count - 1].transform);
        }
        else
        {
            Debug.LogWarning("Deck is empty. Cannot draw more cards.");
        }
    }
    public void EnsurePlayerHasThreeCards()
    {
        while (playerCards.Count < 3)
        {
            DrawCard();
        }
    }

    public void EnemyDrawCard()
    {
        if (opponentCards.Count() == 1)
        {
            GameObject card = gameDeckController.deckCards[0];
            gameDeckController.deckCards.RemoveAt(0);
            opponentCards.Add(card);

            // Set the card's parent to the hand and adjust its position
            card.transform.parent = opponentHandParent;
            card.transform.position = opponentCardsPositions[opponentCards.Count - 1].transform.position;
            card.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
            card.transform.SetParent(opponentCardsPositions[playerCards.Count - 1].transform);
        }
        else
        {
            Debug.LogWarning("Deck is empty. Cannot draw more cards.");
        }

    }

    public void EnsureOpponentHasThreeCards()
    {
        while (opponentCards.Count < 3)
        {
            DrawCard();
        }
    }

    [SerializeField] private GameObject turnOnOff;
    void FightingStagePhase()
    {
        print("fightingStage");
        string levelString = "Level" + SelectedSections.returnCount().ToString();
        SceneManager.LoadScene(levelString);
    }

    private int upgradeCounter=0;
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
                emptySlots.Remove(gridSlots[playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex]);

                Vector3 targetPositionGrid = GetGridSlotPosition(playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex);
                MoveCardToPosition(selectedCardIndex, targetPositionGrid);

                cardsPlayed.Enqueue(playerCards[selectedCardIndex]); // ENQUEUE
                SelectedUpgrades.selectedCards[upgradeCounter] = playerCards[selectedCardIndex];
                print(SelectedUpgrades.selectedCards[upgradeCounter]);
                upgradeCounter+=1;
                if(upgradeCounter==2)
                {
                    upgradeCounter=0;
                    SelectedUpgrades.GiveUpgrades();
                }
                playerCards[selectedCardIndex].transform.SetParent(gridSlots[playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex].transform);
                playerCards[selectedCardIndex].transform.rotation = Quaternion.Euler(0f, 0f, 90f);

                playerCards.RemoveAt(selectedCardIndex);

                RearrangePlayerHand();
                selectedCardIndex = 0;

                if (playedCardCount == 2)
                {
                    playedCardCount = 0;
                    currentPhase = GamePhase.EnemyTurn;
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
                Debug.LogWarning("Invalid card index.");
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
        if (!gridSlots[playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex].GetComponent<SlotController>().isOccupied)
        {
            return GetGridSlotPosition(playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex) + new Vector3(0f, 1f, 0f);
        }
        else if (gridSlots[playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex].GetComponent<SlotController>().isOccupied) //If the middle slot is occupied, put it on an unoccupied adjacent slot
        {
            int[] adjacentSlots = { playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex - 1, playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex + 1, playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex - 3, playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex + 3 };
            foreach (int index in adjacentSlots)
            {
                if (index >= 0 && index < 9 && !gridSlots[index].GetComponent<SlotController>().isOccupied)
                {
                    playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = index;
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
                    playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = index;
                    return GetGridSlotPosition(index) + new Vector3(0f, 1f, 0f);
                }
            }
        }


        return GetGridSlotPosition(playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex) + new Vector3(0f, 1f, 0f);
    }

    void HandleActionPhase()
    {
        SwitchCamera();
        cardsPlayed.First().GetComponent<CardController>().isTheTopCardInQueue = true;

        if (checkIfAllCardsPlayed())
        {
            currentPhase = GamePhase.PlayerTurn;
            foreach (GameObject card in cardsPlayed)
            {
                card.GetComponent<CardController>().playedThisRound = false;
            }

            SwitchCamera();
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

    public void RearrangEnemyHand()
    {
        for (int i = 0; i < opponentCards.Count; i++)
        {
            opponentCards[i].transform.parent = null;
            opponentCards[i].transform.position = opponentCardsPositions[i].transform.position;
            opponentCards[i].transform.parent = opponentCardsPositions[i].transform;

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
        int newIndex = playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex - 3;
        if (newIndex >= 0 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = newIndex;
        }
    }

    void MoveDown()
    {
        int newIndex = playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex + 3;
        if (newIndex < 9 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = newIndex;
        }
    }

    void MoveLeftOnGrid()
    {
        int newIndex = playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex - 1;
        if (newIndex >= 0 && newIndex / 3 == playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex / 3 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = newIndex;
        }
    }

    void MoveRightOnGrid()
    {
        int newIndex = playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex + 1;
        if (newIndex < 9 && newIndex / 3 == playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex / 3 && !gridSlots[newIndex].GetComponent<SlotController>().isOccupied)
        {
            playerCards[selectedCardIndex].GetComponent<CardController>().selectedGridIndex = newIndex;
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

    private void opponentBeaten()
    {
        SelectedSections.gameWon=true;
        SceneManager.LoadScene("GameEndScreen");
    }

    private void playerBeaten()
    {
        SelectedSections.gameWon=false;
        SceneManager.LoadScene("GameEndScreen");
    }

    public void retryLevel()
    {
        Time.timeScale=1;
        print("clickclick");
        SelectedSections.gameWon=false;
        gameObject.GetComponent<SectionController>().openGameEnd(); 
    }

    public void returnToComputer()
    {
        Time.timeScale=1;
        SelectedSections.gameWon=false;
        SceneManager.LoadScene("The Computer");
    }
}
