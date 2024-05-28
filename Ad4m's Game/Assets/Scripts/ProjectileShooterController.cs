using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooterController : MonoBehaviour
{
    [SerializeField]private GameObject eyeProjectile;
    [SerializeField]private float shootsAfter=2;
    private GameObject adam;
    
    // Start is called before the first frame update
    void Start()
    {
        adam = GameObject.Find("Ad4m");
        Invoke("death", shootsAfter);
    }

    // Update is called once per frame
    void Update()
    {   Vector3 target=new Vector3(adam.transform.position.x, transform.position.y,adam.transform.position.z);

        transform.LookAt(target);
    }

    private void death()
    {
        Debug.Log("death");
        GameObject temp=(GameObject)Instantiate(eyeProjectile, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
