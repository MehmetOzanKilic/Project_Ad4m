using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Vector3 startingPos = new Vector3();
    float deactivateDelay = 3.5f;

    public void SetStartingPos(Vector3 pos)
    {
        startingPos = pos;
    }

    public IEnumerator DeactivateProjectile()
    {
        yield return new WaitForSeconds(deactivateDelay);

        gameObject.transform.position = startingPos;
        gameObject.SetActive(false);
    }
}
