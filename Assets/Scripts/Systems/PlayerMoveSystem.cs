using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSystem : IEcsRunSystem
{
   
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var filter = ecsWorld.Filter<Player>().Inc<PlayerInputData>().End();

        var playerPool = ecsWorld.GetPool<Player>();
        var playerInputPool = ecsWorld.GetPool<PlayerInputData>();

        foreach(var i in filter)
        {
            ref var player = ref playerPool.Get(i);
            ref var input  = ref playerInputPool.Get(i);

            Vector3 direction = (Vector3.forward * input.moveInput.z + Vector3.right * input.moveInput.x).normalized;
            player.playerRigidbody.AddForce(direction * player.playerSpeed);
        }

    }
}