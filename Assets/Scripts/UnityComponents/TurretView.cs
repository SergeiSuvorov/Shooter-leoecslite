using Leopotam.EcsLite;
using UnityEngine;

public class TurretView : MonoBehaviour
{
    public int TurretWeapon;
    public EcsWorld EcsWorld;
    public int ShootingDistance;
    public void Shoot()
    {
        EcsWorld.GetPool<Shoot>().Add(TurretWeapon);
    }
    public void Reload()
    {
        EcsWorld.GetPool<ReloadingFinished>().Add(TurretWeapon);
    }
}
