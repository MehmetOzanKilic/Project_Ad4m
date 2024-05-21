using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    GameObject pickedPiece;
    LayerMask layerMask;
    Vector3 originalSize = new Vector3(85, 2.1f, 85);
    Vector3 reducedSize = new Vector3(30, 2.1f, 30);
    Vector3 negativeFlagVector = new Vector3(-1, -1, -1);
    PuzzlePiecePool puzzlePiecePool;
    public int remaningPuzzlePiece = 9;

    void Awake()
    {
        layerMask = LayerMask.GetMask("PuzzlePiece");
        puzzlePiecePool = FindObjectOfType<PuzzlePiecePool>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pickedPiece == null)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, layerMask);

                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("PuzzlePiece"))
                    {
                        PickUpPiece(col.gameObject);
                        break;
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
        pickedPiece.transform.localScale = reducedSize;
    }

    void DropPiece()
    {
        Vector3 closestPosition = GetClosestPosition(transform.position);

        if (closestPosition != negativeFlagVector)
        {
            if (puzzlePiecePool.puzzlePiecePoolList.IndexOf(pickedPiece) == puzzlePiecePool.smallPuzzlePiecePositions.IndexOf(closestPosition))
            {
                remaningPuzzlePiece--;
                pickedPiece.tag = "Picked";
                puzzlePiecePool.smallPuzzlePiecePositions[puzzlePiecePool.smallPuzzlePiecePositions.IndexOf(closestPosition)] = new Vector3(-1000f, 1000f, -1000f);
            }

            pickedPiece.transform.position = new Vector3(closestPosition.x, -0.01f, closestPosition.z);
        }
        else
        {
            pickedPiece.transform.position = new Vector3(pickedPiece.transform.position.x, 0f, pickedPiece.transform.position.z);
        }

        pickedPiece.transform.localScale = originalSize;
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

        if (closestDistance < 70f) { return closestPosition; }
        else { return negativeFlagVector; }
    }
}