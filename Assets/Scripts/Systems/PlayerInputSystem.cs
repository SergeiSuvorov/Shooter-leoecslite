﻿using Leopotam.EcsLite;
using UnityEngine;

public class PlayerInputSystem : IEcsRunSystem
{

    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var filter = ecsWorld.Filter<PlayerInputData>().Inc<HasWeapon>().End();
        var playerInputPool = ecsWorld.GetPool<PlayerInputData>();
        
       

        foreach (var i in filter)
        {
            // Получаем значение компонента. 
            ref var input = ref playerInputPool.Get(i);
            input.MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")); // заполняем данные

            //New
            input.ShootInput = Input.GetMouseButton(0);

           
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("reload push");
                var hasWeaponPool = ecsWorld.GetPool<HasWeapon>();
                ref var hasWeapon = ref hasWeaponPool.Get(i);
                var weaponPool = ecsWorld.GetPool<Weapon>();
                ref var weapon = ref weaponPool.Get(hasWeapon.Weapon);

                if (weapon.CurrentInMagazine < weapon.MaxInMagazine) // если патронов недостаточно, то начать перезарядку
                {
                    var playerFilter = ecsWorld.Filter<Player>().End();
                    var tryReloadPool = ecsWorld.GetPool<TryReload>();
                  
                        Debug.Log("reload");
                        tryReloadPool.Add(i);
                }

            }
        }
    }
}