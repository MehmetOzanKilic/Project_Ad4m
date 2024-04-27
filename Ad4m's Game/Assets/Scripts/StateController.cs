using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public static string gamePhase = "PlayerTurn";
    
    public void setPhase(string phase)
    {
        gamePhase = phase;
    }

    
}
