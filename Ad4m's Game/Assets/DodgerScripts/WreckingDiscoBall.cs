using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingDiscoBall : MonoBehaviour
{
    GameObject vegasSphere;
    Vector3 position;
    float yPosition = 20f;
    float discoBallCooldownDelay = 10f;

    void Awake()
    {
        vegasSphere = GameObject.FindWithTag("VegasSphere");
    }

    Vector3 GetPosition()
    {
        float randomFloat;

        SphereCollider sphereCollider = vegasSphere.GetComponent<SphereCollider>();
        float sphereRadius = sphereCollider.radius * vegasSphere.transform.localScale.z;

        randomFloat = Random.Range(-1f, 1f);
        float randomXPoint = randomFloat * sphereRadius;

        randomFloat = Random.Range(-1f, 1f);
        float randomZPoint = randomFloat * sphereRadius;

        return position = new Vector3(randomXPoint, yPosition, randomZPoint);
    }

    Quaternion GetRotationAngle()
    {
        Quaternion randomRotation;
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                randomRotation = Quaternion.Euler(30f, 0f, 0f);
                break;
            case 1:
                randomRotation = Quaternion.Euler(30f, 0f, 30f);
                break;
            case 2:
                randomRotation = Quaternion.Euler(0f, 0f, 30f);
                break;
            default:
                randomRotation = Quaternion.identity;
                break;
        }

        return randomRotation;
    }

    public void PrepareDiscoBall()
    {
        transform.SetPositionAndRotation(GetPosition(), GetRotationAngle());
        StartCoroutine(DiscoBallCooldown());
    }

    public void DeactivateDiscoBall()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DiscoBallCooldown()
    {
        yield return new WaitForSeconds(discoBallCooldownDelay);
        DeactivateDiscoBall();
    }
}
