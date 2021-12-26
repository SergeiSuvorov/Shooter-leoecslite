using Leopotam.EcsLite;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public int PlayerWeapon;
    public EcsWorld EcsWorld;
    public void Shoot()
    {
        EcsWorld.GetPool<Shoot>().Add(PlayerWeapon);
    }
    public void Reload()
    {
        Debug.Log("Reload on view");
        EcsWorld.GetPool<ReloadingFinished>().Add(PlayerWeapon);
    }
}
