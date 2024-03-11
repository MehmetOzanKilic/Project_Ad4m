using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerDeckController : MonoBehaviour
{
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode playCard = KeyCode.Space;

    public Transform playerHandParent;
    public GameObject[] cards;
    private int selectedCardIndex = 0;

    public GameObject[] gridSlots;
    private bool usingGridInput = false;

    public CinemachineVirtualCamera deckViewCam;
    public CinemachineVirtualCamera topViewCam;
    private bool usingtopViewCam = false;

    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;

    public Transform playCardTransform;
    public float movementSpeed = 5f;
    private bool isMovingToPlayPosition = false;


    // Start is called before the first frame update
    void Start()
    {
        InitializePlayerHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(moveLeftKey))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(moveRightKey))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(playCard))
        {
            MoveCardToPlayPosition();
            SwitchCamera();
        }
        else if (usingGridInput)
        {
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
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
        }

        if (isMovingToPlayPosition)
        {
            MoveToPlayPosition();
        }

        Hover();
    }

    void InitializePlayerHand()
    {
        cards = new GameObject[playerHandParent.childCount];
        for (int i = 0; i < playerHandParent.childCount; i++)
        {
            cards[i] = playerHandParent.GetChild(i).gameObject;
        }
    }

    void MoveRight()
    {
        if (!usingGridInput)
            selectedCardIndex = (selectedCardIndex + 1) % cards.Length;
        else
        {
            int newIndex = selectedCardIndex + 1;

 
            if (newIndex >= 0 && newIndex < gridSlots.Length && newIndex / 3 == selectedCardIndex / 3)
            {
                selectedCardIndex = newIndex;
                UpdateCardPosition();
            }
        }
    }

    void MoveLeft()
    {
        if (!usingGridInput)
            selectedCardIndex = (selectedCardIndex - 1 + cards.Length) % cards.Length;
        else
        {
            int newIndex = selectedCardIndex - 1;

            if (newIndex >= 0 && newIndex < gridSlots.Length && newIndex / 3 == selectedCardIndex / 3)
            {
                selectedCardIndex = newIndex;
                UpdateCardPosition();
            }
        }
    }

    void MoveUp()
    {
        int newIndex = selectedCardIndex - 3;

        if (newIndex >= 0 && newIndex < gridSlots.Length)
        {
            selectedCardIndex = newIndex;
            UpdateCardPosition();
        }
    }

    void MoveDown()
    {
        int newIndex = selectedCardIndex + 3;

        if (newIndex >= 0 && newIndex < gridSlots.Length)
        {
            selectedCardIndex = newIndex;
            UpdateCardPosition();
        }
    }

    void Hover()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Vector3 targetPosition = cards[i].transform.localPosition;
            if (i == selectedCardIndex)
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

    void SwitchCamera()
    {
        if (usingtopViewCam)
        {
            topViewCam.Priority = 0;
            deckViewCam.Priority = 10;
            usingGridInput = false;
            usingtopViewCam = false;
     
        }
        else
        {
            deckViewCam.Priority = 0;
            topViewCam.Priority = 10;
            usingGridInput = true;
            usingtopViewCam = true;
        
        }
    }

    /*void MoveToPlayPosition()
    {
        Vector3 targetPosition = playCardTransform.position;
        Quaternion targetRotation = playCardTransform.rotation;

        cards[selectedCardIndex].transform.position = Vector3.MoveTowards(cards[selectedCardIndex].transform.position, targetPosition, movementSpeed * Time.deltaTime);
        cards[selectedCardIndex].transform.rotation = Quaternion.RotateTowards(cards[selectedCardIndex].transform.rotation, targetRotation, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(cards[selectedCardIndex].transform.position, targetPosition) < 0.01f)
        {
            isMovingToPlayPosition = false;
        }

        if (usingGridInput)
        {
            float minDistance = float.MaxValue;
            Vector3 nearestSlotPosition = Vector3.zero;

            foreach (GameObject slot in gridSlots)
            {
                float distance = Vector3.Distance(cards[selectedCardIndex].transform.position, slot.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSlotPosition = slot.transform.position;
                }
            }

            targetPosition = nearestSlotPosition;
        }

    }*/

    void MoveToPlayPosition()
    {
        Quaternion targetRotation = playCardTransform.rotation;

        if (usingGridInput)
        {
            float minDistance = float.MaxValue;
            Vector3 nearestSlotPosition = Vector3.zero;

            foreach (GameObject slot in gridSlots)
            {
                float distance = Vector3.Distance(cards[selectedCardIndex].transform.position, slot.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSlotPosition = slot.transform.position;
                }
            }


            cards[selectedCardIndex].transform.position = Vector3.MoveTowards(cards[selectedCardIndex].transform.position, nearestSlotPosition, movementSpeed * Time.deltaTime);
        }
        else
        {

            cards[selectedCardIndex].transform.position = Vector3.MoveTowards(cards[selectedCardIndex].transform.position, playCardTransform.position, movementSpeed * Time.deltaTime);
        }

        cards[selectedCardIndex].transform.rotation = Quaternion.RotateTowards(cards[selectedCardIndex].transform.rotation, targetRotation, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(cards[selectedCardIndex].transform.position, playCardTransform.position) < 0.01f)
        {
            isMovingToPlayPosition = false; 
        }
    }

    public void MoveCardToPlayPosition()
    {
        isMovingToPlayPosition = true;
    }

    void UpdateCardPosition()
    {
        Vector3 targetPosition = gridSlots[selectedCardIndex].transform.position;
        targetPosition.y += hoverHeight;

        cards[selectedCardIndex].transform.position = Vector3.Lerp(cards[selectedCardIndex].transform.position, targetPosition, hoverSpeed * Time.deltaTime);
    }
}
