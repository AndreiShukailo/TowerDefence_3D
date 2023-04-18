using UnityEngine;
using UnityEngine.UI;

public class BildProgress : MonoBehaviour
{
    [SerializeField] private Image _fill;

    public void SetBildProgress(float current, float max)
    {
        _fill.fillAmount = current / max;
    }
}
