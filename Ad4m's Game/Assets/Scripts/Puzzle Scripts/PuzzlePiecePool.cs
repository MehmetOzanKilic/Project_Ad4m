using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiecePool : MonoBehaviour
{
    GameObject grid;
    GameObject vegasSphere;
    PuzzlePiece puzzlePiece;
    [SerializeField] GameObject gridPrefab;
    [SerializeField] GameObject smallPuzzlePiecePrefab;
    [SerializeField] List<GameObject> puzzlePieces;
    public List<GameObject> puzzlePiecePoolList;
    List<GameObject> smallPuzzlePiecePool = new List<GameObject>();
    public List<Vector3> smallPuzzlePiecePositions = new List<Vector3>();

    void Awake()
    {
        smallPuzzlePiecePositions.Add(new Vector3(-87f, 0f, 87f));
        smallPuzzlePiecePositions.Add(new Vector3(0f, 0f, 87));
        smallPuzzlePiecePositions.Add(new Vector3(87f, 0f, 87f));

        smallPuzzlePiecePositions.Add(new Vector3(-87f, 0f, 0f));
        smallPuzzlePiecePositions.Add(new Vector3(0f, 0f, 0f));
        smallPuzzlePiecePositions.Add(new Vector3(87f, 0f, 0f));

        smallPuzzlePiecePositions.Add(new Vector3(-87f, 0f, -87f));
        smallPuzzlePiecePositions.Add(new Vector3(0f, 0f, -87f));
        smallPuzzlePiecePositions.Add(new Vector3(87f, 0f, -87f));
    }

    void Start()
    {
        puzzlePiece = FindObjectOfType<PuzzlePiece>();
        vegasSphere = GameObject.FindWithTag("VegasSphere");
    }

    public void InitializePuzzlePiecePool()
    {
        Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        grid = GameObject.FindWithTag("Grid");

        puzzlePiecePoolList = new List<GameObject>();

        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            Vector3 puzzlePiecePosition = GetPosition();

            GameObject newPuzzlePiece = Instantiate(puzzlePieces[i], puzzlePiecePosition, Quaternion.identity, transform);

            puzzlePiecePoolList.Add(newPuzzlePiece); ;
        }
    }

    public void InitializeSmallPuzzlePiecePool()
    {
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            Vector3 smallPuzzlePiecePosition = GetSmallPosition(i);

            GameObject smallPuzzlePiece = Instantiate(smallPuzzlePiecePrefab, smallPuzzlePiecePosition, Quaternion.identity, transform);

            smallPuzzlePiecePool.Add(smallPuzzlePiece);
        }
    }

    Vector3 GetPosition()
    {
        SphereCollider sphereCollider = vegasSphere.GetComponent<SphereCollider>();
        float sphereRadius = sphereCollider.radius * vegasSphere.transform.localScale.z;

        float randomTheta = Random.Range(0f, Mathf.PI * 2f);
        float randomPhi = Random.Range(0f, Mathf.PI);

        float randomXPoint = sphereRadius * Mathf.Sin(randomPhi) * Mathf.Cos(randomTheta);
        float randomZPoint = sphereRadius * Mathf.Sin(randomPhi) * Mathf.Sin(randomTheta);

        Vector3 position = new Vector3(randomXPoint, 0, randomZPoint);

        if (grid.GetComponent<BoxCollider>().bounds.Contains(position))
        {
            return GetPosition();
        }
        else
        {
            return position;
        }
    }

    Vector3 GetSmallPosition(int i)
    {
        return smallPuzzlePiecePositions[i];
    }

    public void MatchRandomPuzzlePieces(int count)
    {
        count = Mathf.Min(count, puzzlePiecePoolList.Count);
        List<Vector3> tempPuzzlePiecePositions = new List<Vector3>(smallPuzzlePiecePositions);
        List<GameObject> tempPuzzlePieceList = new List<GameObject>(puzzlePiecePoolList);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, puzzlePiecePoolList.Count - 1);
            GameObject randomPuzzlePiece = tempPuzzlePieceList[randomIndex];

            randomPuzzlePiece.transform.position = new Vector3(tempPuzzlePiecePositions[randomIndex].x, -0.01f, tempPuzzlePiecePositions[randomIndex].z);
            randomPuzzlePiece.tag = "Picked";
            puzzlePiece.remaningPuzzlePiece--;

            tempPuzzlePieceList.RemoveAt(randomIndex);
            tempPuzzlePiecePositions.RemoveAt(randomIndex);
            smallPuzzlePiecePositions[randomIndex] = new Vector3(-1000f, 1000f, -1000f);
        }
    }
}
