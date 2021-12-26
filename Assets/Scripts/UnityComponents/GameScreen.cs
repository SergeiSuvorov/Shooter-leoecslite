using TMPro;
using UnityEngine;

public class GameScreen : Screen
{
    [SerializeField] private TextMeshProUGUI _currentInMagazineLabel;
    [SerializeField] private TextMeshProUGUI _totalAmmoLabel;

    public void SetAmmoInfo(int current, int total)
    {
        _currentInMagazineLabel.text = current.ToString();
        _totalAmmoLabel.text = total.ToString();
    }
}
