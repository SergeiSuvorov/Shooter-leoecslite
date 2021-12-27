using Leopotam.EcsLite;
using UnityEngine;

public class TurretShootingSystem : IEcsRunSystem
{
    private EcsWorld _ecsWorld;

    public void Run(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        var playerFilter = _ecsWorld.Filter<Player>().End();
        var playerPool = _ecsWorld.GetPool<Player>();
        var turretFilter = _ecsWorld.Filter<Turret>().End();
        var turretPool = _ecsWorld.GetPool<Turret>();

        foreach (var playerEntity in playerFilter)
        {
            ref var player = ref playerPool.Get(playerEntity);

            foreach (var turretEntity in turretFilter)
            {
                ref var turret = ref turretPool.Get(turretEntity);

                Vector3 direction = player.PlayerTransform.position - turret.TurretTransform.position;
                var magitude = Vector3.Magnitude(direction);
                if(magitude<4)
                {
                    if(!turret.TurretAnimator.GetBool("Shooting"))
                    turret.TurretAnimator.SetBool("Shooting", true);
                }
                else
                    turret.TurretAnimator.SetBool("Shooting", false);
            }
        }
    }
}
