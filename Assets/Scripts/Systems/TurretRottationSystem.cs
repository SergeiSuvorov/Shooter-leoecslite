using Leopotam.EcsLite;
using UnityEngine;

public class TurretRottationSystem :  IEcsRunSystem
{
    private EcsWorld _ecsWorld;

    public void Run(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        var playerFilter = _ecsWorld.Filter<Player>().End();
        var playerPool = _ecsWorld.GetPool<Player>();
        var turretFilter = _ecsWorld.Filter<Turret>().End();
        var turretPool = _ecsWorld.GetPool<Turret>();

        foreach(var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);

            foreach( var turretEntity in turretFilter)
            {
                ref var turret = ref turretPool.Get(turretEntity);

                Vector3 lookPos = player.PlayerTransform.position - turret.TurretTransform.position;
                Vector3 rotation = Quaternion.Lerp(turret.TurretTransform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime).eulerAngles;
                rotation.z = 0;
                rotation.x = 0;

                turret.TurretTransform.eulerAngles = rotation;
            }
        }
    }
}
