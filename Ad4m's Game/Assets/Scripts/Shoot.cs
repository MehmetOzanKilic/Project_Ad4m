using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;

    Vector3 mousePos;
    float nextFireTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >=nextFireTime)
        {
            if(Input.GetMouseButton(0))
            {
                ShootBullet();
            }
        }

        Ray ray = camera.gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
        }
        
    }

    void ShootBullet()
    {
        GameObject instBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        instBullet.GetComponent<Bullet>().move = mousePos;

        nextFireTime = Time.time + 1 / fireRate;
    }
}
