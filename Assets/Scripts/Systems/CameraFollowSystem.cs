using Leopotam.EcsLite;
using UnityEngine;

public class CameraFollowSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _ecsWorld;
    private SceneData _sceneData;
    private StaticData _staticData;

    private Vector3 currentVelocity;

    public void Init(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        _sceneData = dataContainer.SceneData;
        _staticData = dataContainer.Configuration;
    }

    public void Run(EcsSystems systems)
    {
        var playerPool = _ecsWorld.GetPool<Player>();
        var filter = _ecsWorld.Filter<Player>().End();

        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);
            var currentPos = _sceneData.MainCamera.transform.position;

            currentPos = Vector3.SmoothDamp(currentPos, player.PlayerTransform.position + _staticData.FollowOffset, ref currentVelocity, _staticData.SmoothTime);
            _sceneData.MainCamera.transform.position = currentPos;
        }
    }
}
