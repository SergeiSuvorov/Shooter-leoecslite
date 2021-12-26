using Leopotam.EcsLite;
using UnityEngine;

public class SpawnProjectileSystem : IEcsRunSystem
{

    private EcsWorld _ecsWorld;

    public void Run(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        var bulletSpawnFilter = _ecsWorld.Filter<Weapon>().Inc<SpawnProjectile>().End();
        var weaponPool = _ecsWorld.GetPool<Weapon>();
        var spawnProjectilePool = _ecsWorld.GetPool<SpawnProjectile>();
        var projectilePool = _ecsWorld.GetPool<Projectile>();

        foreach (var bulletSpawn in bulletSpawnFilter)
        {
            ref var weapon = ref weaponPool.Get(bulletSpawn);
            
            // Создаем GameObject пули и ее сущность
            var projectileGO = Object.Instantiate(weapon.ProjectilePrefab, weapon.ProjectileSocket.position, Quaternion.identity);
            var projectileEntity = _ecsWorld.NewEntity();

            ref var projectile = ref projectilePool.Add(projectileEntity);

            projectile.Damage = weapon.WeaponDamage;
            projectile.Direction = weapon.ProjectileSocket.forward;
            projectile.Radius = weapon.ProjectileRadius;
            projectile.Speed = weapon.ProjectileSpeed;
            projectile.ProjectileGO = projectileGO;
            projectile.PreviousPos = projectileGO.transform.position;
          
            spawnProjectilePool.Del(bulletSpawn);
        }

    }
}
