using Leopotam.EcsLite;
using UnityEngine;

public class CameraFollowSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld ecsWorld;
    private SceneData sceneData;
    private StaticData staticData;

    private Vector3 currentVelocity;

    public void Init(EcsSystems systems)
    {
        ecsWorld = systems.GetWorld();
        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        sceneData = dataContainer.SceneData;
        staticData = dataContainer.Configuration;
    }

    public void Run(EcsSystems systems)
    {
        var playerPool = ecsWorld.GetPool<Player>();
        var filter = ecsWorld.Filter<Player>().End();

        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);
            var currentPos = sceneData.MainCamera.transform.position;

            currentPos = Vector3.SmoothDamp(currentPos, player.PlayerTransform.position + staticData.FollowOffset, ref currentVelocity, staticData.SmoothTime);
            sceneData.MainCamera.transform.position = currentPos;
        }
    }
}
