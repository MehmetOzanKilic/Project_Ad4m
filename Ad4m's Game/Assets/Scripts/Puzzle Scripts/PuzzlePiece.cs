using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    GameObject pickedPiece;
    PuzzleController gameController;
    Vector3 negativeFlagVector = new Vector3(-1, -1, -1);
    [SerializeField]private PuzzlePiecePool puzzlePiecePool;
    public int remaningPuzzlePiece = 9;

    public List<GameObject> correctPuzzlePieces=new List<GameObject>();

    private GameObject[] puzzlePieces;

    void Start()
    {
        Invoke("findPuzzlePieces",0.3f);
    }

    private void findPuzzlePieces()
    {
        puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");
        gameController = GameObject.Find("GameController").GetComponent<PuzzleController>();
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        GameObject closest = null;
        float closestDistance = 100;
        if (pickedPiece == null)
        {
            if (puzzlePieces != null)
            {
                //if every puzzle piece is spawned at the same time
                /*for (int i = 0; i < puzzlePieces.Length; i++)
                {
                    print("index: " + i);
                    while (correctPuzzlePieces.Contains(puzzlePieces[i]))
                    {
                        print("correct puzzle piece: " + puzzlePieces[i]);
                        i++;
                        if (i >= puzzlePieces.Length) // Ensure not to go out of bounds
                            break;
                    }
                    if (i >= puzzlePieces.Length) // Ensure not to go out of bounds
                        break;

                    print("false puzzle piece: " + puzzlePieces[i]);
                    float distance = Vector3.Distance(puzzlePieces[i].transform.position, transform.position);
                    if (distance < closestDistance) { closestDistance = distance; closest = puzzlePieces[i]; }
                    print("closest: " + closest);

                }

                if (closestDistance < 2)
                {
                    PickUpPiece(closest);
                }*/
                
                GameObject activePuzzlePiece = GameObject.FindGameObjectWithTag("PuzzlePiece");
                if(Vector3.Distance(activePuzzlePiece.transform.position, transform.position)<3)
                {
                    pickedPiece = activePuzzlePiece;
                }
            }
        }
        else
        {
            DropPiece();
        }

        if(correctPuzzlePieces.Count != 9 )
        {
            if(GameObject.FindGameObjectWithTag("PuzzlePiece")==null)
            {
                puzzlePiecePool.activateRandomPiece();
            }
        }
    }

    if (pickedPiece != null)
    {
        Vector3 newPosition = transform.position + new Vector3(0, 130, 0);
        pickedPiece.transform.position = newPosition;
    }
}

    void PickUpPiece(GameObject piece)
    {
        pickedPiece = piece;
        //pickedPiece.transform.localScale = reducedSize;
    }


    void DropPiece()
    {
        Vector3 closestPosition = GetClosestPosition(transform.position);
        float distance = Vector3.Distance(closestPosition, transform.position);

        if (closestPosition != negativeFlagVector /*&& distance < 20*/)
        {
            if (puzzlePiecePool.puzzlePiecesList.IndexOf(pickedPiece) == puzzlePiecePool.smallPuzzlePiecePositions.IndexOf(closestPosition))
            {
                remaningPuzzlePiece--;
                pickedPiece.tag = "Picked";
                puzzlePiecePool.activateRandomPiece();
                correctPuzzlePieces.Add(pickedPiece);
                if(correctPuzzlePieces.Count == 9) gameController.puzzleFinished();
                puzzlePiecePool.smallPuzzlePiecePositions[puzzlePiecePool.smallPuzzlePiecePositions.IndexOf(closestPosition)] = new Vector3(-1000f, 1000f, -1000f);
            }

            pickedPiece.transform.position = new Vector3(closestPosition.x, -0.01f, closestPosition.z);
        }
        else
        {
            pickedPiece.transform.position = new Vector3(pickedPiece.transform.position.x, 0f, pickedPiece.transform.position.z);
        }

        //pickedPiece.transform.localScale = originalSize;
        pickedPiece = null;
    }
    
    Vector3 GetClosestPosition(Vector3 currentPosition)
    {
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = float.MaxValue;
        int counter=0;
        foreach (Vector3 position in puzzlePiecePool.smallPuzzlePiecePositions)
        {
            counter++;
            float distance = Vector3.Distance(currentPosition, position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
            print(counter);
            print(position);
        }

        print(closestDistance);
        print(closestPosition);

        if (closestDistance < 2f) { return closestPosition; }
        else { return negativeFlagVector; }
    }
}

/*using UnityEngine;
using System.Collections.Generic;

public class PuzzlePiece : MonoBehaviour
{
    GameObject pickedPiece;
    GameController gameController;
    Vector3 negativeFlagVector = new Vector3(-1, -1, -1);
    PuzzlePiecePool puzzlePiecePool;
    public int remainingPuzzlePieces = 9;

    public List<GameObject> correctPuzzlePieces = new List<GameObject>();

    private GameObject[] puzzlePieces;

    void Start()
    {
        Invoke("FindPuzzlePieces", 0.3f);
    }

    private void FindPuzzlePieces()
    {
        Debug.Log("FindPuzzlePieces start");
        puzzlePiecePool = GameObject.Find("PuzzlePiecePool").GetComponent<PuzzlePiecePool>();
        puzzlePieces = GameObject.FindGameObjectsWithTag("PuzzlePiece");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Debug.Log("FindPuzzlePieces finish");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject closest = null;
            float closestDistance = 100;
            if (pickedPiece == null)
            {
                if (puzzlePieces != null)
                {
                    for (int i = 0; i < puzzlePieces.Length; i++)
                    {
                        Debug.Log("index: " + i);
                        while (correctPuzzlePieces.Contains(puzzlePieces[i]))
                        {
                            Debug.Log("correct puzzle piece: " + puzzlePieces[i]);
                            i++;
                            if (i >= puzzlePieces.Length) // Ensure not to go out of bounds
                                break;
                        }
                        if (i >= puzzlePieces.Length) // Ensure not to go out of bounds
                            break;

                        Debug.Log("false puzzle piece: " + puzzlePieces[i]);
                        float distance = Vector3.Distance(puzzlePieces[i].transform.position, transform.position);
                        if (distance < closestDistance) { closestDistance = distance; closest = puzzlePieces[i]; }
                        Debug.Log("closest: " + closest);
                    }

                    if (closestDistance < 2)
                    {
                        PickUpPiece(closest);
                    }
                }
            }
            else
            {
                DropPiece();
            }
        }

        if (pickedPiece != null)
        {
            Vector3 newPosition = transform.position + new Vector3(0, 130, 0);
            pickedPiece.transform.position = newPosition;
        }
    }

    void PickUpPiece(GameObject piece)
    {
        pickedPiece = piece;
    }

    void DropPiece()
    {
        Vector3 closestPosition = GetClosestPosition(transform.position);
        float distance = Vector3.Distance(closestPosition, transform.position);

        if (closestPosition != negativeFlagVector && distance < 2)
        {
            int index = puzzlePiecePool.puzzlePiecePoolList.IndexOf(pickedPiece);
            if (index != -1 && index == puzzlePiecePool.smallPuzzlePiecePositions.IndexOf(closestPosition))
            {
                remainingPuzzlePieces--;
                pickedPiece.tag = "Picked";
                correctPuzzlePieces.Add(pickedPiece);
                if (correctPuzzlePieces.Count == 9) gameController.puzzleFinished();
                puzzlePiecePool.smallPuzzlePiecePositions[index] = new Vector3(-1000f, 1000f, -1000f);
            }

            pickedPiece.transform.position = new Vector3(closestPosition.x, -0.01f, closestPosition.z);
        }
        else
        {
            pickedPiece.transform.position = new Vector3(pickedPiece.transform.position.x, 0f, pickedPiece.transform.position.z);
        }

        pickedPiece = null;
    }

    Vector3 GetClosestPosition(Vector3 currentPosition)
    {
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (Vector3 position in puzzlePiecePool.smallPuzzlePiecePositions)
        {
            float distance = Vector3.Distance(currentPosition, position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
        }

        if (closestDistance < 2f) { return closestPosition; }
        else { return negativeFlagVector; }
    }
}*/