using UnityEngine;
using UnityEngine.Events;

public class Stats : MonoBehaviour
{
    public int Money { get; private set; } = 100;
    public static Stats Instance;
    public UnityAction<int> OnMoneyChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnMoneyChange?.Invoke(Money);
    }

    private void ReciveMoney(int money)
    {
        Money += money;
        OnMoneyChange?.Invoke(Money);
    }

    public bool Pay(int coast)
    {
        if (Money < coast)
        {          
            return false;
        }
        else
        {
            Money -= coast;
            OnMoneyChange?.Invoke(Money);
            return true;
        }
    }
}
