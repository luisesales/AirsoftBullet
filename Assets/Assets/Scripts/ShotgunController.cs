using UnityEngine;

public class ShotgunController : GunController
{
    [SerializeField]
    private int numberOfPellets = 6;

    [Range(0, 30)] private float spreadAngle = 10f;
    protected override void Shoot()
    {
        for (int i = 0; i < numberOfPellets; i++)
        {
            // Calculate a random spread rotation
            float randomSpreadX = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            float randomSpreadY = Random.Range(-spreadAngle / 2, spreadAngle / 2);

            // Apply spread to the bulletHole's forward direction
            Quaternion spreadRotation = Quaternion.Euler(bulletHole.eulerAngles.x + randomSpreadX, bulletHole.eulerAngles.y + randomSpreadY, bulletHole.eulerAngles.z);
            Vector3 bulletDirection = spreadRotation * Vector3.forward;

            GameObject bullet = Instantiate(bulletPrefab, bulletHole.position, spreadRotation);
            currentCooldown = fireCooldown;
        }
        ammoCount--;
        GameController.Instance.UpdateAmmoCount(ammoCount, magazineSize, magazineAmount, isAutomatic);
    }
}
