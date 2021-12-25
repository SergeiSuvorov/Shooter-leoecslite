using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
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
            .Add(new PlayerAnimationSystem());
       

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

public sealed class InitSystemDataContainer
{
    public StaticData Configuration { get; private set; }
    public SceneData SceneData { get; private set; }

    public InitSystemDataContainer(StaticData configuration, SceneData sceneData)
    {
        Configuration = configuration;
        SceneData = sceneData;
    }

}


