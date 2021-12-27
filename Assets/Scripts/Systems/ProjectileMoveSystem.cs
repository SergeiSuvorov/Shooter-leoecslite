using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileMoveSystem : IEcsRunSystem
{
   
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var projectileFilter = ecsWorld.Filter<Projectile>().End();
        var projectilePool = ecsWorld.GetPool<Projectile>();

        foreach (var projectileEntity in projectileFilter)
        {

            ref var projectile = ref projectilePool.Get(projectileEntity);

            var position = projectile.ProjectileGO.transform.position;
            position += projectile.Direction * projectile.Speed * Time.deltaTime;
            projectile.ProjectileGO.transform.position = position;

            var displacementSinceLastFrame = position - projectile.PreviousPos;
            var hit = Physics.SphereCast(projectile.PreviousPos, projectile.Radius,
                displacementSinceLastFrame.normalized, out var hitInfo, displacementSinceLastFrame.magnitude);
            if (hit)
            {
                var hitPool = ecsWorld.GetPool<ProjectileHit>();
                if(!hitPool.Has(projectileEntity))
                {
                    ref var projectileHit = ref hitPool.Add(projectileEntity);
                    projectileHit.RaycastHit = hitInfo;
                }              
            }

            projectile.PreviousPos = projectile.ProjectileGO.transform.position;
        }
    }
}
