using UnityEngine;

public class pistolGUN : MonoBehaviour
{

    [SerializeField] int maxAmmo = 100;
    [SerializeField] int loadedAmmo = 25;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnBullet;
    [SerializeField] float bulletForce = 1000f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(loadedAmmo > 0)
            {
                // Fires gun
                fireGun();

            } else
            {
                // auto reloads gun
                reloadGun();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            reloadGun();
        }
    }

    public void reloadGun()
    {
        if (maxAmmo > 0 && loadedAmmo != 25)
        {
            if (maxAmmo >= 25)
            {
                int tmpAmmo = loadedAmmo - 25;
                loadedAmmo -= tmpAmmo;
                maxAmmo += tmpAmmo;
            } else if (maxAmmo > 0) {
                int tmpAmmo = loadedAmmo + maxAmmo;
                
                if (tmpAmmo <= 25)
                {
                    loadedAmmo = tmpAmmo;
                    maxAmmo = 0;
                } else 
                    loadedAmmo = 25;
                    maxAmmo = tmpAmmo - loadedAmmo;
            }
        }
    }

    public void fireGun()
    {
        //fires the gun
        loadedAmmo -= 1;
        GameObject bullet = Instantiate(bulletPrefab, spawnBullet.position, transform.rotation.normalized);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(spawnBullet.forward * bulletForce);
        Destroy(bullet, 3f);
    }
}
