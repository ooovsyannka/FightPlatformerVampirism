using TMPro;
using UnityEngine;

public class AmmunitionInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammunitionText;
    [SerializeField] private Ammunition _ammunition;

    private void OnEnable()
    {
        _ammunition.ValueChanged += ShowAmmunitionCount;    
    }

    private void OnDisable()
    {
        _ammunition.ValueChanged -= ShowAmmunitionCount;       
    }

    private void ShowAmmunitionCount() =>
       _ammunitionText.text = $"AMMUNITION \n {_ammunition.CurrentBulletCountInClip} / {_ammunition.CurrentAllBulletCount}";
}