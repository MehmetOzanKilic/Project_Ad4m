using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlePiecePool : MonoBehaviour
{
    [SerializeField] private int matchCount = 3;
    private PuzzlePiece puzzlePieceScript;
    [SerializeField] private List<GameObject> puzzlePieces;
    public List<GameObject> puzzlePiecesList = new List<GameObject>();
    public List<Vector3> smallPuzzlePiecePositions = new List<Vector3>();
    [SerializeField] private List<Transform> inspectorSmallPuzzlePositions = new List<Transform>();

    void Start()
    {
        puzzlePieceScript = GameObject.Find("Ad4m").GetComponent<PuzzlePiece>();
        for (int i = 0; i < inspectorSmallPuzzlePositions.Count; i++)
        {
            smallPuzzlePiecePositions.Add(inspectorSmallPuzzlePositions[i].position);
        }

        /*for (int i = 0; i < puzzlePieces.Count; i++)
        {
            puzzlePiecesList.Add(puzzlePieces[i]);
        }*/

        InitializePuzzlePiecePool();
        MatchRandomPuzzlePieces(matchCount);
    }

    void Update()
    {
        
    }

    public void InitializePuzzlePiecePool()
    {
        List<Vector3> generatedPositions = new List<Vector3>();
        float minDistance = 5f;

        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            Vector3 newPosition = GetPosition(i, generatedPositions, minDistance);
            GameObject newPuzzlePiece = Instantiate(puzzlePieces[i], newPosition, Quaternion.identity);
            newPuzzlePiece.SetActive(false);
            puzzlePiecesList.Add(newPuzzlePiece);
            generatedPositions.Add(newPosition);
        }

        Invoke("activateRandomPiece",1f);
    }

    public void activateRandomPiece()
    {
        int randomIndex = Random.Range(0,9);

        while(puzzlePieceScript.correctPuzzlePieces.Contains(puzzlePiecesList[randomIndex]))
        {
            randomIndex = Random.Range(0,9);
        }

        //puzzlePiecePoolList[randomIndex].SetActive(true);
        puzzlePiecesList[randomIndex].SetActive(true);
    }

    Vector3 GetPosition(int index, List<Vector3> generatedPositions, float minDistance)
    {
        int maxAttempts = 100;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float randomAngle = Random.Range(0f, Mathf.PI * 2f);
            float randomDistance = Random.Range(10f, 20f);
            Vector3 newPosition = new Vector3(Mathf.Cos(randomAngle) * randomDistance, 0f, Mathf.Sin(randomAngle) * randomDistance);

            bool isValidPosition = true;
            foreach (Vector3 existingPosition in generatedPositions)
            {
                if (Vector3.Distance(newPosition, existingPosition) < minDistance)
                {
                    isValidPosition = false;
                    break;
                }
            }

            if (isValidPosition)
            {
                return newPosition;
            }

            attempts++;
        }

        Debug.LogWarning("Failed to find a valid position after " + maxAttempts + " attempts.");
        return Vector3.zero;
    }

    public void MatchRandomPuzzlePieces(int count)
    {
        List<Vector3> tempPuzzlePiecePositions = new List<Vector3>(smallPuzzlePiecePositions);
        List<GameObject> tempPuzzlePieceList = new List<GameObject>(puzzlePiecesList);

        for (int i = 0; i < matchCount && tempPuzzlePieceList.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempPuzzlePieceList.Count-1);
            GameObject randomPuzzlePiece = tempPuzzlePieceList[randomIndex];

            randomPuzzlePiece.transform.position = new Vector3(tempPuzzlePiecePositions[randomIndex].x, -0.01f, tempPuzzlePiecePositions[randomIndex].z);
            randomPuzzlePiece.tag = "Picked";
            randomPuzzlePiece.SetActive(true);
            puzzlePieceScript.remaningPuzzlePiece--;
            puzzlePieceScript.correctPuzzlePieces.Add(randomPuzzlePiece);
            Debug.Log("Piece added: " + randomPuzzlePiece.name);
            print("randomIndex: " + randomIndex);
            tempPuzzlePieceList.RemoveAt(randomIndex);
            tempPuzzlePiecePositions.RemoveAt(randomIndex);
        }
    }

    /*public void MatchRandomPuzzlePieces(int count)
    {
        //List<Vector3> tempPuzzlePiecePositions = new List<Vector3>(smallPuzzlePiecePositions);
        //List<GameObject> tempPuzzlePieceList = new List<GameObject>(puzzlePiecePoolList);

        for (int i = 0; i < matchCount && puzzlePieces.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, puzzlePieces.Count);

            puzzlePiecesList[randomIndex].transform.position = new Vector3(smallPuzzlePiecePositions[randomIndex].x, -0.01f, smallPuzzlePiecePositions[randomIndex].z);
            puzzlePiecesList[randomIndex].tag = "Picked";
            //puzzlePiecesList[randomIndex].SetActive(true);
            puzzlePieceScript.remaningPuzzlePiece--;
            puzzlePieceScript.correctPuzzlePieces.Add(puzzlePiecesList[randomIndex]);

            puzzlePiecesList.RemoveAt(randomIndex);
            smallPuzzlePiecePositions.RemoveAt(randomIndex);
        }
    }*/
}

    /*Vector3 GetPosition(int index)
    {
        print("inside getposition");
        Debug.Log(index); // Use Debug.Log instead of print for Unity

        float randomXPoint = UnityEngine.Random.Range(-20f, 20f);
        float randomZPoint = UnityEngine.Random.Range(-20f, 20f);

        // Ensure the generated point is at least 10 units away from the origin
        while (Mathf.Abs(randomXPoint) < 10f)
        {
            randomXPoint *= UnityEngine.Random.Range(1f, 2f);
        }
        while (Mathf.Abs(randomZPoint) < 10f)
        {
            randomZPoint *= UnityEngine.Random.Range(1f, 2f);
        }

        return new Vector3(randomXPoint, 0f, randomZPoint);

    }*/

    /*Vector3 GetPosition(int index)
    {
        Debug.Log("inside GetPosition");
        Debug.Log(index); // Use Debug.Log instead of print for Unity

        // Generate a random angle in radians
        float randomAngle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        // Generate a random distance from the origin (within the range -20 to 20)
        float randomDistance = UnityEngine.Random.Range(10f, 20f);

        // Convert polar coordinates to Cartesian coordinates
        float xPos = Mathf.Cos(randomAngle) * randomDistance;
        float zPos = Mathf.Sin(randomAngle) * randomDistance;

        return new Vector3(xPos, 0f, zPos);
    }*/
   
