using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private StaticData _configuration;
    [SerializeField] private SceneData _sceneData;
    [SerializeField] private UI _ui;

    private EcsWorld _ecsWorld;
    private EcsSystems _updateSystems;
    private EcsSystems _fixedUpdateSystems; // новая группа систем

    private void Start()
    {

        _ecsWorld = new EcsWorld();
        RuntimeData runtimeData = new RuntimeData();
        //InitSystemDataContainer(configuration, sceneData) - используется для внедрения зависимостей при инициализации систем
        _updateSystems = new EcsSystems(_ecsWorld, new InitSystemDataContainer(_configuration, _sceneData, runtimeData,_ui));
        _fixedUpdateSystems = new EcsSystems(_ecsWorld);


        _updateSystems
            .Add(new PlayerInitSystem())
            .Add(new TurretInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new PlayerRotationSystem())
            .Add(new TurretRottationSystem())
            .Add(new TurretShootingSystem())
            .Add(new PlayerAnimationSystem())
            .Add(new CameraFollowSystem())
            .Add(new WeaponShootSystem())
            .Add(new SpawnProjectileSystem())
            .Add(new ProjectileHitSystem())
            .Add(new ProjectileMoveSystem())
            .Add(new ReloadingSystem())
            .Add(new PauseSystem());

        _fixedUpdateSystems
            .Add(new PlayerMoveSystem()); // добавляем систему движения

        // Инициализируем группы систем
        _updateSystems.Init();
        _fixedUpdateSystems.Init();
    }

    private void Update()
    {
        _updateSystems?.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        _updateSystems?.Destroy();
        _updateSystems = null;
        _fixedUpdateSystems?.Destroy();
        _fixedUpdateSystems = null;
        _ecsWorld?.Destroy();
        _ecsWorld = null;
    }
}


