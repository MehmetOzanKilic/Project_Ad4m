using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 1f;
    private float reloadSpeed;
    private float shotAmount;
    private int shotCount =0;
    private bool canShoot;
    private PlayerAttributeController playerAttributeController;
    private GameObject playerObject;


    // Start is called before the first frame update
    private void Awake()
    {
        playerObject = GameObject.Find("Ad4m");
    }
    void Start()
    {
        canShoot = true;
        playerAttributeController = playerObject.GetComponent<PlayerAttributeController>();
        reloadSpeed = playerAttributeController.reloadSpeed;
        shotAmount = playerAttributeController.shotAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("bambam");
            if (shotCount < shotAmount && canShoot)
            {
                Shoot();
            }
            else
            {
                canShoot = false;
                StartCoroutine(WaitForTemp());
            }
        }
    }
    IEnumerator WaitForTemp()
    {
        yield return new WaitForSeconds(reloadSpeed);
        shotCount = 0; // Reset the shot count after waiting
        canShoot = true;
    }

    void Shoot()
    { 
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        shotCount++;
        Destroy(bullet, bulletLifetime);
    }

    void FixedUpdate()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            bullet.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }
}
