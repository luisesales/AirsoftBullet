using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private Transform bulletHole;
    
    private float fireCooldown;

    [SerializeField]
    private float fireRate;

    [SerializeField]
    private bool isAutomatic;
    private bool automatic;

    private int magazineSize;

    [SerializeField]
    private int ammoCount;

    private bool isSelected = false;

    private float currentCooldown;
    void Start()
    {
        fireCooldown = 1f / fireRate;
        automatic = isAutomatic;
        magazineSize = ammoCount;        
        bulletHole = transform.Find("BulletHole");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            if (isAutomatic)
            {
                if (Input.GetButton("Fire1") && currentCooldown <= 0f && ammoCount > 0)
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1") && currentCooldown <= 0f && ammoCount > 0)
                {
                    Shoot();
                }
            }
            if (Input.GetButton("Reload") && ammoCount < magazineSize && currentCooldown <= 0f)
            {
                Reload();
            }
            if(Input.GetButtonDown("F") && automatic)
            {
                SwitchMode();
            }
            currentCooldown -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletHole.position, bulletHole.rotation);
        currentCooldown = isAutomatic? 0f : fireCooldown;
        ammoCount--;
    }

    private void Reload()
    {
        currentCooldown = 5f;
        ammoCount = magazineSize;
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
    public void SwitchMode()
    {
        isAutomatic = !isAutomatic;        
    }
}
