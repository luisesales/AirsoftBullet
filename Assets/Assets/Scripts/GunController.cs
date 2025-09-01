using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private Transform bulletHole;

    public float fireCooldown { get; private set; }

    public float fireRate { get; private set; }

    public bool isAutomatic { get; private set; }

    private bool isSelected = false;

    private float currentCooldown;
    void Start()
    {
        currentCooldown = fireCooldown;
        bulletHole = transform.Find("BulletHole");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            if (isAutomatic)
            {
                if (Input.GetButton("Fire1") && currentCooldown <= 0f)
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && currentCooldown <= 0f)
                {
                    Shoot();
                }
            }
            currentCooldown -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletHole.position, bulletHole.rotation);
        currentCooldown = fireCooldown;
    }

    public void SelectGun()
    {
        isSelected = true;  
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Collider>() != null)
            child.gameObject.GetComponent<Collider>().enabled = false;
        }      
    }
}
