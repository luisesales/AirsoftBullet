using UnityEngine;

public class MotorRifleController : GunController
{
    [SerializeField]
    private float heatTimer = 1.5f;
    private float currentHeatTimer = 0f;
    private float fireRateMutiplier = 1f;

    [SerializeField]
    private float maxFireRateMutiplier = 3f;

    protected override void Start()
    {
        base.Start();
        currentHeatTimer = heatTimer;
    }

    protected override void Update()
    {
        if (isSelected)
        {
            if (isAutomatic)
            {
                if (Input.GetButton("Fire1") && currentCooldown <= 0f && ammoCount > 0)
                {
                    if (currentHeatTimer > 0f)
                    {
                        currentHeatTimer -= Time.deltaTime;
                        return;
                    }
                    Shoot();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    fireRateMutiplier = 1f;
                    fireCooldown = 1f / fireRate;
                    currentHeatTimer = heatTimer;
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

    protected override void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletHole.position, bulletHole.rotation);        
        currentCooldown = fireCooldown / fireRateMutiplier;
        if (fireRateMutiplier < maxFireRateMutiplier)
            fireRateMutiplier += 0.5f;
        fireCooldown = 1f / (fireRate * fireRateMutiplier);        
        currentCooldown = fireCooldown;
        ammoCount--;
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount);
    }
}