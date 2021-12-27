using Leopotam.EcsLite;
using UnityEngine;

public class WeaponShootSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private UI _ui;
    public void Init(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        _ui = systems.GetShared<InitSystemDataContainer>().UI;
    }

    public void Run(EcsSystems systems)
    {
        
        EcsFilter weaponFilter = _ecsWorld.Filter<Weapon>().Inc<Shoot>().End();

        var shootPool = _ecsWorld.GetPool<Shoot>();
        var weaponPool = _ecsWorld.GetPool<Weapon>();
        var playerPool = _ecsWorld.GetPool<Player>();

        foreach (var shootedWeapon in weaponFilter)
        {
            shootPool.Del(shootedWeapon);
            ref var weapon = ref weaponPool.Get(shootedWeapon);

            if (weapon.CurrentInMagazine > 0)
            {
                weapon.CurrentInMagazine--;
                var spawnProjectilePool = _ecsWorld.GetPool<SpawnProjectile>();
                spawnProjectilePool.Add(shootedWeapon);
            }
            else // если патронов нет, начать перезарядку
            {
                var reloadPool = _ecsWorld.GetPool<TryReload>();
                reloadPool.Add(weapon.Owner);
            }
            if (playerPool.Has(weapon.Owner))
            {
                _ui.GameScreen.SetAmmoInfo(weapon.CurrentInMagazine, weapon.TotalAmmo);
            }
        }
    }
}
