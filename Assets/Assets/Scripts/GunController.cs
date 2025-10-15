using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
    [SerializeField]
    protected GameObject bulletPrefab;
    protected Transform bulletHole;

    protected float fireCooldown;

    [SerializeField]
    protected float fireRate;

    [SerializeField]
    protected bool isAutomatic;
    protected bool automatic;

    protected int magazineSize;

    [SerializeField]
    protected int ammoCount;

    [SerializeField]
    protected int magazineAmount;

    protected int maxMagazineAmount;


    protected bool isSelected = false;

    protected float currentCooldown;

    [SerializeField]
    private GameObject gunHolder;
    protected virtual void Start()
    {
        fireCooldown = 1f / fireRate;
        automatic = isAutomatic;
        magazineSize = ammoCount;
        maxMagazineAmount = magazineAmount;
        bulletHole = transform.Find("BulletHole");
    }

    // Update is called once per frame
    protected virtual void Update()
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
            if (Input.GetButtonDown("SwitchMode") && automatic)
            {
                SwitchMode();
            }
            currentCooldown -= Time.deltaTime;
        }
    }

    protected virtual void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletHole.position, bulletHole.rotation);
        currentCooldown = fireCooldown;
        ammoCount--;
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
    }

    protected void Reload()
    {
        if (magazineAmount <= 0)
        {
            Debug.Log("No magazines left");
            return;
        }
        Debug.Log("Reloading");
        currentCooldown = 5f;
        ammoCount = magazineSize;
        magazineAmount--;
        GameController.Instance.StartTimer(currentCooldown);
        Invoke("FinishReload", currentCooldown);
    }

    public void SelectGun()
    {
        isSelected = true;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Collider>() != null)
                child.gameObject.GetComponent<Collider>().enabled = false;
        }
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
    }
    public void SwitchMode()
    {
        isAutomatic = !isAutomatic;
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
    }

    public void CollectAmmo(int amount, GameObject magazine)
    {
        if(ammoCount < magazineSize)
        {
            ammoCount = magazineSize;
            GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
            amount--;            
        }
        if(magazineAmount+amount > maxMagazineAmount)
        {
            Debug.Log("Max magazines reached");
            return;
        }
        magazineAmount = Mathf.Min(magazineAmount + amount, maxMagazineAmount);
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
        Destroy(magazine);
    }

    private void FinishReload()
    {
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
    }

    public void TeleportToGunHolder()
    {
        transform.position = gunHolder.transform.position;
        isSelected = false;
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Collider>() != null)
                child.gameObject.GetComponent<Collider>().enabled = true;
        }
    }
    
    public bool GetIsSelected()
    {
        return isSelected;
    }
    
}
