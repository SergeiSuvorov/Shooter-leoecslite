using Leopotam.EcsLite;

public class ProjectileHitSystem : IEcsRunSystem
{
   
    public void Run(EcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        var hitFilter = ecsWorld.Filter<Projectile>().Inc<ProjectileHit>().End();
        var projectilePool = ecsWorld.GetPool<Projectile>();
        var hitPool = ecsWorld.GetPool<ProjectileHit>();

        foreach (var hitEntity in hitFilter)
        {
            ref var projectile = ref projectilePool.Get(hitEntity);

            projectile.ProjectileGO.SetActive(false);
        }
    }
}
