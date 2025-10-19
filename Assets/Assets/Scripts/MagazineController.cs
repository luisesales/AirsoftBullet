using UnityEngine;

public class MagazineController : MonoBehaviour
{
    [SerializeField]
    private float bulletMass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float GetBulletMass()
    {
        return bulletMass;
    }
    
    public void SetBulletMass(float bulletMass)
    {
        this.bulletMass = bulletMass;
    }
}
