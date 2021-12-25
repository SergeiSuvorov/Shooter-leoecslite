using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationSystem : IEcsRunSystem
{
    private SceneData sceneData;
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();

        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        sceneData = dataContainer.SceneData;

        var filter = ecsWorld.Filter<Player>().End();
        var playerPool = ecsWorld.GetPool<Player>();
        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);

            Plane playerPlane = new Plane(Vector3.up, player.playerTransform.position);
            Ray ray = sceneData.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!playerPlane.Raycast(ray, out var hitDistance)) continue;

            player.playerTransform.forward = ray.GetPoint(hitDistance) - player.playerTransform.position;
        }
    }
}
