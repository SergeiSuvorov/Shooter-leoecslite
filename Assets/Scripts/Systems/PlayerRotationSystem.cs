using Leopotam.EcsLite;
using UnityEngine;

public class PlayerRotationSystem : IEcsRunSystem, IEcsInitSystem
{
    private SceneData sceneData;
    private EcsWorld ecsWorld;

    public void Init(EcsSystems systems)
    {
        ecsWorld = systems.GetWorld();
        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        sceneData = dataContainer.SceneData;
    }

    public void Run(EcsSystems systems)
    {
        var filter = ecsWorld.Filter<Player>().End();
        var playerPool = ecsWorld.GetPool<Player>();
        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);

            Plane playerPlane = new Plane(Vector3.up, player.PlayerTransform.position);
            Ray ray = sceneData.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (!playerPlane.Raycast(ray, out var hitDistance)) continue;

            player.PlayerTransform.forward = ray.GetPoint(hitDistance) - player.PlayerTransform.position;
        }
    }
}
