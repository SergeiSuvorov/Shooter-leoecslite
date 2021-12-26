using Leopotam.EcsLite;
using UnityEngine;

public class PlayerRotationSystem : IEcsRunSystem, IEcsInitSystem
{
    private SceneData _sceneData;
    private EcsWorld _ecsWorld;
    private RuntimeData _runtimeData;

    public void Init(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        _sceneData = dataContainer.SceneData;
        _runtimeData = systems.GetShared<InitSystemDataContainer>().RuntimeData;
    }

    public void Run(EcsSystems systems)
    {
        if (_runtimeData.IsPaused) return;

        var filter = _ecsWorld.Filter<Player>().End();
        var playerPool = _ecsWorld.GetPool<Player>();
        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);

            Plane playerPlane = new Plane(Vector3.up, player.PlayerTransform.position);
            Ray ray = _sceneData.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (!playerPlane.Raycast(ray, out var hitDistance)) continue;

            player.PlayerTransform.forward = ray.GetPoint(hitDistance) - player.PlayerTransform.position;
        }
    }
}
