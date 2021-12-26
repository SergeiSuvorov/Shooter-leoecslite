using Leopotam.EcsLite;

public class ReloadingSystem : IEcsRunSystem, IEcsInitSystem
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
        EcsWorld ecsWorld = systems.GetWorld();

        var tryReloadFilter = ecsWorld.Filter<TryReload>().Inc<AnimatorRef>().End();
        var reloadingFinishedFilter = ecsWorld.Filter<Weapon>().Inc<ReloadingFinished>().End();

        var tryReloadPool = ecsWorld.GetPool<TryReload>();
        var animatorRefPool = ecsWorld.GetPool<AnimatorRef>();
        var weaponPool = ecsWorld.GetPool<Weapon>();
        var reloadingFinishedPool = ecsWorld.GetPool<ReloadingFinished>();
        var playerPool = ecsWorld.GetPool<Player>();

        foreach (var tryReloadEntity in tryReloadFilter)
        {
            ref var tryReload = ref tryReloadPool.Get(tryReloadEntity);
            bool wasTrying = tryReload.WasTrying;

            if (!wasTrying)
            {
                ref var animatorRef = ref animatorRefPool.Get(tryReloadEntity);

                animatorRef.Animator.SetTrigger("Reload");

                var reloadingPool = ecsWorld.GetPool<Reloading>();
                reloadingPool.Add(tryReloadEntity);
                tryReload.WasTrying = true;
            }
        }


        foreach (var reloadingFinished in reloadingFinishedFilter)
        {
            ref var weapon = ref weaponPool.Get(reloadingFinished);

            var needAmmo = weapon.MaxInMagazine - weapon.CurrentInMagazine;
            weapon.CurrentInMagazine = (weapon.TotalAmmo >= needAmmo)
                ? weapon.MaxInMagazine
                : weapon.CurrentInMagazine + weapon.TotalAmmo;
            weapon.TotalAmmo -= needAmmo;
            weapon.TotalAmmo = weapon.TotalAmmo < 0
                ? 0
                : weapon.TotalAmmo;


            var reloadingPool = ecsWorld.GetPool<Reloading>();
            reloadingPool.Del(weapon.Owner);
            reloadingFinishedPool.Del(reloadingFinished);
            tryReloadPool.Del(weapon.Owner);

            if (playerPool.Has(weapon.Owner))
            {
                _ui.GameScreen.SetAmmoInfo(weapon.CurrentInMagazine, weapon.TotalAmmo);
            }
        }

    }
}
