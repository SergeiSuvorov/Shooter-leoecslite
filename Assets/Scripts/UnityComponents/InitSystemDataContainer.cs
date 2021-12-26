public sealed class InitSystemDataContainer
{
    public StaticData Configuration { get; private set; }
    public SceneData SceneData { get; private set; }

    public InitSystemDataContainer(StaticData configuration, SceneData sceneData)
    {
        Configuration = configuration;
        SceneData = sceneData;
    }

}


