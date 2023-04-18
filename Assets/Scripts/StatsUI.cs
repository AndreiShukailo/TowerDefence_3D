using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Stats _stats;
    [SerializeField] private TMP_Text _moneyCount;

    private void OnEnable()
    {
        _stats.OnMoneyChange += ChangeMoneyCount;
    }

    private void OnDisable()
    {
        _stats.OnMoneyChange -= ChangeMoneyCount;
    }

    private void ChangeMoneyCount(int count)
    {
        _moneyCount.text = count.ToString();
    }
}
