using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    PuzzlePiecePool puzzlePiecePool;
    public int matchCount;

    void Start()
    {
        puzzlePiecePool = FindObjectOfType<PuzzlePiecePool>();

        puzzlePiecePool.InitializePuzzlePiecePool();
        puzzlePiecePool.InitializeSmallPuzzlePiecePool();

        puzzlePiecePool.MatchRandomPuzzlePieces(matchCount);
    }
}
