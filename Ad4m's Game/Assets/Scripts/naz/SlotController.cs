using UnityEngine;

public class SlotController : MonoBehaviour
{
    public bool isOccupied = false;

    void Update()
    {
        isOccupied = transform.childCount > 1 && transform.GetChild(1).CompareTag("Card");
    }
}
