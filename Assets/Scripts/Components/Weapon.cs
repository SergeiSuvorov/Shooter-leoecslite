using UnityEngine;

public struct Weapon 
{
    public int Owner;
    public GameObject ProjectilePrefab;
    public Transform ProjectileSocket;
    public float ProjectileSpeed;
    public float ProjectileRadius;
    public int WeaponDamage;
    public int CurrentInMagazine;
    public int MaxInMagazine;
    public int TotalAmmo;
}
