public sealed class InitSystemDataContainer
{
    public StaticData Configuration { get; private set; }
    public SceneData SceneData { get; private set; }
    public RuntimeData RuntimeData { get; private set; }
    public UI UI { get; private set; }

    public InitSystemDataContainer(StaticData configuration, SceneData sceneData, RuntimeData runtimeData, UI uI)
    {
        Configuration = configuration;
        SceneData = sceneData;
        RuntimeData = runtimeData;
        UI = uI;
    }
}


