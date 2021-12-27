using Leopotam.EcsLite;
using UnityEngine;

public class TurretInitSystem : IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private StaticData _staticData;
    private SceneData _sceneData;
    private UI _ui;
    public void Init(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();

        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        _staticData = dataContainer.Configuration;
        _sceneData = dataContainer.SceneData;
        _ui = dataContainer.UI;

        var turretPool = _ecsWorld.GetPool<Turret>();

        for (int i = 0; i < _sceneData.TurretSpawnPoints.Length; i++)
        {
            int turretEntity = _ecsWorld.NewEntity();
            ref var turret = ref turretPool.Add(turretEntity);

            GameObject turretGO = Object.Instantiate(_staticData.TurretPrefab, _sceneData.TurretSpawnPoints[i].position, Quaternion.identity);

            turret.TurretTransform = turretGO.transform;
            var turretView = turretGO.GetComponent<TurretView>();
            turretView.EcsWorld = _ecsWorld;
            turret.ShootingDistance = turretView.ShootingDistance;
            turret.TurretAnimator = turretGO.GetComponent<Animator>();
            var weaponEntity = _ecsWorld.NewEntity();
            var weaponPool = _ecsWorld.GetPool<Weapon>();
            weaponPool.Add(weaponEntity);
            ref var weapon = ref weaponPool.Get(weaponEntity);
            var weaponView = turretGO.GetComponentInChildren<WeaponSettings>();

            weapon.Owner = turretEntity;
            weapon.ProjectilePrefab = weaponView.ProjectilePrefab;
            weapon.ProjectileRadius = weaponView.ProjectileRadius;
            weapon.ProjectileSocket = weaponView.ProjectileSocket;
            weapon.ProjectileSpeed = weaponView.ProjectileSpeed;
            weapon.TotalAmmo = weaponView.TotalAmmo;
            weapon.WeaponDamage = weaponView.WeaponDamage;
            weapon.CurrentInMagazine = weaponView.CurrentInMagazine;
            weapon.MaxInMagazine = weaponView.MaxInMagazine;

            turretView.TurretWeapon = weaponEntity;

            var animatorRefPool = _ecsWorld.GetPool<AnimatorRef>();
            animatorRefPool.Add(turretEntity);
            ref var animatorRef = ref animatorRefPool.Get(turretEntity);

            animatorRef.Animator = turret.TurretAnimator;
        }
    }
}
