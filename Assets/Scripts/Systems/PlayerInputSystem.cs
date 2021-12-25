using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{

    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var filter = ecsWorld.Filter<PlayerInputData>().End();
        var playerInput = ecsWorld.GetPool<PlayerInputData>();

        foreach (var i in filter)
        {
            // Получаем значение компонента. 
            ref var input = ref playerInput.Get(i);
            input.moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); // заполняем данные
        }
    }
}