using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInitSystem : IEcsInitSystem
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

        int playerEntity = _ecsWorld.NewEntity();

        var inputPool = _ecsWorld.GetPool<PlayerInputData>();
        inputPool.Add(playerEntity);
        ref var inputData = ref inputPool.Get(playerEntity);

        var playerPool =  _ecsWorld.GetPool<Player>();
        playerPool.Add(playerEntity);
        ref var player = ref playerPool.Get(playerEntity);
        player.PlayerSpeed = _staticData.PlayerSpeed;

        //// Спавним GameObject игрока
        GameObject playerGO = Object.Instantiate(_staticData.PlayerPrefab, _sceneData.PlayerSpawnPoint.position, Quaternion.identity);
        player.PlayerTransform = playerGO.transform;
        player.PlayerRigidbody = playerGO.GetComponent<Rigidbody>();
        player.PlayerAnimator = playerGO.GetComponent<Animator>();

        playerGO.GetComponent<PlayerView>().EcsWorld = _ecsWorld;

        var weaponEntity = _ecsWorld.NewEntity();
        var weaponPool = _ecsWorld.GetPool<Weapon>();
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

        var hasWeaponPool = _ecsWorld.GetPool<HasWeapon>();
        hasWeaponPool.Add(playerEntity);
        ref var hasWeapon = ref hasWeaponPool.Get(playerEntity);
        hasWeapon.Weapon = weaponEntity;

        playerGO.GetComponent<PlayerView>().PlayerWeapon = weaponEntity;

        var animatorRefPool = _ecsWorld.GetPool<AnimatorRef>();
        animatorRefPool.Add(playerEntity);
        ref var animatorRef = ref animatorRefPool.Get(playerEntity);
        animatorRef.Animator = player.PlayerAnimator;

        _ui.GameScreen.SetAmmoInfo(weapon.CurrentInMagazine, weapon.TotalAmmo);
    }
}
