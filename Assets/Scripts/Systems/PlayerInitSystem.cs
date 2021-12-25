using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;
    private StaticData staticData; 
    private SceneData sceneData;


    public void Init(EcsSystems systems)
    {
        ecsWorld = systems.GetWorld();

        var dataContainer = systems.GetShared<InitSystemDataContainer>();
        staticData = dataContainer.Configuration;
        sceneData = dataContainer.SceneData;

        int playerEntity = ecsWorld.NewEntity();

        var inputPool = ecsWorld.GetPool<PlayerInputData>();
        inputPool.Add(playerEntity);
        ref var inputData = ref inputPool.Get(playerEntity);

        var playerPool =  ecsWorld.GetPool<Player>();
        playerPool.Add(playerEntity);
        ref var player = ref playerPool.Get(playerEntity);
        player.playerSpeed = staticData.playerSpeed;

        //// Спавним GameObject игрока
        GameObject playerGO = Object.Instantiate(staticData.playerPrefab, sceneData.playerSpawnPoint.position, Quaternion.identity);
        player.playerTransform = playerGO.transform;
        player.playerRigidbody = playerGO.GetComponent<Rigidbody>();
        player.playerAnimator = playerGO.GetComponent<Animator>();

    }
}
