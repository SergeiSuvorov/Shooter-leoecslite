using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public StaticData configuration;
    public SceneData sceneData;

    private EcsWorld ecsWorld;
    private EcsSystems updateSystems;
    private EcsSystems fixedUpdateSystems; // новая группа систем

    private void Start()
    {

        ecsWorld = new EcsWorld();
        //InitSystemDataContainer(configuration, sceneData) - используется для внедрения зависимостей при инициализации систем
        updateSystems = new EcsSystems(ecsWorld, new InitSystemDataContainer(configuration, sceneData));
        fixedUpdateSystems = new EcsSystems(ecsWorld);
        RuntimeData runtimeData = new RuntimeData();

        updateSystems
            .Add(new PlayerInitSystem())
            .Add(new PlayerInputSystem())
            .Add(new PlayerRotationSystem())
            .Add(new PlayerAnimationSystem())
            .Add(new CameraFollowSystem())
            .Add(new WeaponShootSystem())
            .Add(new SpawnProjectileSystem())
            .Add(new ProjectileHitSystem())
            .Add(new ProjectileMoveSystem())
            .Add(new ReloadingSystem());





        fixedUpdateSystems
            .Add(new PlayerMoveSystem()); // добавляем систему движения

        // Инициализируем группы систем
        updateSystems.Init();
        fixedUpdateSystems.Init();
    }

    private void Update()
    {
        updateSystems?.Run();
    }

    private void FixedUpdate()
    {
        fixedUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        updateSystems?.Destroy();
        updateSystems = null;
        fixedUpdateSystems?.Destroy();
        fixedUpdateSystems = null;
        ecsWorld?.Destroy();
        ecsWorld = null;
    }
}


