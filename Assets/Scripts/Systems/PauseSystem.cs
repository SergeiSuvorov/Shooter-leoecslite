using Leopotam.EcsLite;
using UnityEngine;

public class PauseSystem : IEcsRunSystem, IEcsInitSystem
{
    private RuntimeData _runtimeData;
    private EcsWorld _ecsWorld;
    private UI _ui;
    public void Init(EcsSystems systems)
    {
        _ecsWorld = systems.GetWorld();
        _ui = systems.GetShared<InitSystemDataContainer>().UI;
        _runtimeData = systems.GetShared<InitSystemDataContainer>().RuntimeData;
    }
   

    public void Run(EcsSystems systems)
    {
        var pauseFilter = _ecsWorld.Filter<PauseEvent>().End();
        var pausePool = _ecsWorld.GetPool<PauseEvent>();
        foreach (var pauseEntity in pauseFilter)
        {
            pausePool.Del(pauseEntity);
            _runtimeData.IsPaused = !_runtimeData.IsPaused;
            Time.timeScale = _runtimeData.IsPaused ? 0f : 1f;
            _ui.PauseScreen.Show(_runtimeData.IsPaused);
        }
    }
}
