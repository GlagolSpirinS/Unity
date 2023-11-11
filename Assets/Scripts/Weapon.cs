using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponEnum weaponType;
    public AudioSource shotSound;
    public AudioSource reloadSound;

    public int maxMagazineBulletCount;
    public int currentMagazineBulletCount;
    public int maxAmmoSupply;
    public float timeBetweenShots;
    public float timeForReloading;

    bool canFire = true;
    bool isReloading = false;

    private IEnumerator LockFire(float Time)
    {
        yield return new WaitForSeconds(Time);
        canFire = true;
    }

    private IEnumerator LockFireForReloading(float Time)
    {
        reloadSound.Play();
        yield return new WaitForSeconds(Time);
        currentMagazineBulletCount = maxMagazineBulletCount;
        maxAmmoSupply -= maxMagazineBulletCount - currentMagazineBulletCount;
        canFire = true;
        isReloading = false;
        Debug.Log("Перезарядка завершена!");
    }

    public bool IsMagazineEmpty()
    {
        if (currentMagazineBulletCount == 0) return true;
        else return false;
    }

    public void Reload()
    {
        if (!isReloading)
        {
            Debug.Log("Перезарядка!");
            isReloading = true;
            canFire = false;
            StartCoroutine(LockFireForReloading(timeForReloading));
        }
    }

    public void Fire()
    {
        if (canFire && !IsMagazineEmpty() && !isReloading)
        {
            shotSound.Play();

            currentMagazineBulletCount--;
            Debug.Log(currentMagazineBulletCount);

            RaycastHit HitInfo = new RaycastHit();

            if (Physics.Raycast(Camera.main.transform.position,
                Camera.main.transform.forward, out HitInfo))
            {
                Debug.Log(HitInfo.transform.name);
            }

            canFire = false;
            StartCoroutine(LockFire(timeForReloading));
        }
    }
}