using Leopotam.EcsLite;
using UnityEngine;

public class WeaponShootSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        EcsFilter weaponFilter = ecsWorld.Filter<Weapon>().Inc<Shoot>().End();
        var shootPool = ecsWorld.GetPool<Shoot>();
        var weaponPool = ecsWorld.GetPool<Weapon>();

        foreach (var shootedWeapon in weaponFilter)
        {
            shootPool.Del(shootedWeapon);
            ref var weapon = ref weaponPool.Get(shootedWeapon);   
            if (weapon.CurrentInMagazine > 0)
            {
                weapon.CurrentInMagazine--;
                Debug.Log("ammo "+ weapon.CurrentInMagazine);
                var spawnProjectilePool = ecsWorld.GetPool<SpawnProjectile>();
                spawnProjectilePool.Add(shootedWeapon);
            }
            else // если патронов нет, начать перезарядку
            {
                Debug.Log("no ammo");
                var reloadPool = ecsWorld.GetPool<TryReload>();
                reloadPool.Add(weapon.Owner);
            }
        }
    }
}
