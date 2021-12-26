using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;
    private StaticData staticData; 
    private SceneData sceneData;


    public void Init(EcsSystems systems)
    {
        ecsWorld = systems.GetWorld();

        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        staticData = dataContainer.Configuration;
        sceneData = dataContainer.SceneData;

        int playerEntity = ecsWorld.NewEntity();

        var inputPool = ecsWorld.GetPool<PlayerInputData>();
        inputPool.Add(playerEntity);
        ref var inputData = ref inputPool.Get(playerEntity);

        var playerPool =  ecsWorld.GetPool<Player>();
        playerPool.Add(playerEntity);
        ref var player = ref playerPool.Get(playerEntity);
        player.PlayerSpeed = staticData.PlayerSpeed;

        //// Спавним GameObject игрока
        GameObject playerGO = Object.Instantiate(staticData.PlayerPrefab, sceneData.PlayerSpawnPoint.position, Quaternion.identity);
        player.PlayerTransform = playerGO.transform;
        player.PlayerRigidbody = playerGO.GetComponent<Rigidbody>();
        player.PlayerAnimator = playerGO.GetComponent<Animator>();

        playerGO.GetComponent<PlayerView>().EcsWorld = ecsWorld;

        var weaponEntity = ecsWorld.NewEntity();
        var weaponPool = ecsWorld.GetPool<Weapon>();
        weaponPool.Add(weaponEntity);
        ref var weapon = ref weaponPool.Get(weaponEntity);
        var weaponView = playerGO.GetComponentInChildren<WeaponSettings>();

        weapon.Owner = playerEntity;
        weapon.ProjectilePrefab = weaponView.ProjectilePrefab;
        weapon.ProjectileRadius = weaponView.ProjectileRadius;
        weapon.ProjectileSocket = weaponView.ProjectileSocket;
        weapon.ProjectileSpeed = weaponView.ProjectileSpeed;
        weapon.TotalAmmo = weaponView.TotalAmmo;
        weapon.WeaponDamage = weaponView.WeaponDamage;
        weapon.CurrentInMagazine = weaponView.CurrentInMagazine;
        weapon.MaxInMagazine = weaponView.MaxInMagazine;

        var hasWeaponPool = ecsWorld.GetPool<HasWeapon>();
        hasWeaponPool.Add(playerEntity);
        ref var hasWeapon = ref hasWeaponPool.Get(playerEntity);
        hasWeapon.Weapon = weaponEntity;

        playerGO.GetComponent<PlayerView>().PlayerWeapon = weaponEntity;

        var animatorRefPool = ecsWorld.GetPool<AnimatorRef>();
        animatorRefPool.Add(playerEntity);
        ref var animatorRef = ref animatorRefPool.Get(playerEntity);
        animatorRef.Animator = player.PlayerAnimator;
    }
}
