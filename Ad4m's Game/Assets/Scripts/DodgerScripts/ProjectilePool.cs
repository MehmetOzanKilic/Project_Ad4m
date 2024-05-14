using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    GameObject[] projectilePrefabs;
    int poolSize = 30;
    List<GameObject> projectilePool;
    float waitDelay = 1f;

    float force = 50f;
    Transform spawningPlane;
    Transform arena;
    GameObject vegasSphere;

    private GameObject adam;

    void Awake()
    {
        projectilePrefabs = GameObject.FindGameObjectsWithTag("Projectile");
        spawningPlane = GameObject.FindWithTag("Spawner").transform;
        arena = GameObject.FindWithTag("Ground").transform;
        vegasSphere = GameObject.FindWithTag("VegasSphere");
        adam= GameObject.Find("Ad4m");
    }

    bool IsInPlaneSpawnArea(Vector3 projectilePosition)
    {
        Vector3 positionWithoutY = new Vector3(projectilePosition.x, spawningPlane.position.y, projectilePosition.z);

        return spawningPlane.GetComponent<BoxCollider>().bounds.Contains(positionWithoutY);
    }

    bool IsInArena(Vector3 projectilePosition)
    {
        Vector3 sphereCenter = vegasSphere.GetComponent<SphereCollider>().bounds.center;
        float sphereRadius = vegasSphere.GetComponent<SphereCollider>().radius * vegasSphere.transform.localScale.z;

        float distanceToSphereCenter = Vector3.Distance(projectilePosition, sphereCenter);

        return distanceToSphereCenter <= sphereRadius;
    }

    Vector3 GetRandomPointInArena()
    {
        Vector2 randomPoint = Random.insideUnitCircle * arena.GetComponent<MeshCollider>().bounds.extents.x;
        return new Vector3(randomPoint.x, spawningPlane.position.y, randomPoint.y);
    }

    public Vector3 GetProjectilePosition()
    {
        Vector3 bounds = spawningPlane.GetComponent<MeshRenderer>().bounds.extents;

        float yCoordinate = spawningPlane.position.y + 1.2f;
        float xCoordinate = Random.Range(-bounds.x, bounds.x);
        float zCoordinate = Random.Range(-bounds.z, bounds.z);

        return new Vector3(xCoordinate, yCoordinate, zCoordinate);
    }

    public bool IsValidPosition(Vector3 projectilePosition)
    {
        return IsInPlaneSpawnArea(projectilePosition) && !IsInArena(projectilePosition);
    }

    public void InitializePool()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            bool isValidPosition = false;
            Vector3 projectilePosition = Vector3.zero;

            while (!isValidPosition)
            {
                projectilePosition = GetProjectilePosition();
                isValidPosition = IsValidPosition(projectilePosition);
            }

            GameObject selectedProjectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
            GameObject projectile = Instantiate(selectedProjectilePrefab, projectilePosition, Quaternion.identity, transform);
            projectile.GetComponent<Projectiles>().SetStartingPos(projectilePosition);

            projectile.SetActive(false);
            projectilePool.Add(projectile);
        }
    }

    public IEnumerator ActivateProjectilesPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitDelay);

            ActivateRandomProjectile();
        }
    }

    void ActivateRandomProjectile()
    {
        int randomIndex = Random.Range(0, projectilePool.Count);
        GameObject randomProjectile = projectilePool[randomIndex];

        if (!randomProjectile.activeInHierarchy)
        {
            randomProjectile.SetActive(true);
            ApplyForce(randomProjectile);
        }
        Debug.Log("boş activate");
    }

    public void ApplyForce(GameObject projectile)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 randomPointInArena = GetRandomPointInArena();
        Vector3 directionToArena = (randomPointInArena - projectile.transform.position).normalized;

        rb.AddForce(directionToArena * force, ForceMode.Impulse);
        StartCoroutine(projectile.GetComponent<Projectiles>().DeactivateProjectile());
    }

    public void ProjectileEyeApplyForce(GameObject projectile, Vector3 playerPos, Vector3 eyePos)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 direction = (playerPos - eyePos).normalized;

        rb.AddForce(direction * force, ForceMode.Impulse);
        StartCoroutine(projectile.GetComponent<Projectiles>().DeactivateProjectile());
    }

    public void ProjectileEye()
    {
        int randomIndex = Random.Range(0, projectilePool.Count);
        GameObject randomProjectile = projectilePool[randomIndex];

        if (!randomProjectile.activeInHierarchy)
        {
            randomProjectile.SetActive(true);

            // values are placeholders, replace it with actual player position and eye position.
            ProjectileEyeApplyForce(randomProjectile, new Vector3(0, 0, 0), new Vector3(50, 0, 50));
        }
        else
        {
            Debug.Log("call ProjectileEye() again.");
        }
    }
}
