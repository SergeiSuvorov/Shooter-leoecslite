
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSystem : IEcsRunSystem
{
   
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var filter = ecsWorld.Filter<Player>().Inc<PlayerInputData>().End();

        var playerPool = ecsWorld.GetPool<Player>();
        var InputPool = ecsWorld.GetPool<PlayerInputData>();

        foreach (var i in filter)
        {
            ref var player = ref playerPool.Get(i);
            ref var input = ref InputPool.Get(i);

            float vertical = Vector3.Dot(input.moveInput.normalized, player.playerTransform.forward);
            float horizontal = Vector3.Dot(input.moveInput.normalized, player.playerTransform.right);
            player.playerAnimator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
            player.playerAnimator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        }
    }
}