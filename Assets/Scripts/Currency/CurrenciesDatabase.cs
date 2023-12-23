using UnityEngine;

[CreateAssetMenu(fileName = "Currencies Database", menuName = "Content/Currencies/Currencies Database")]
public class CurrenciesDatabase : ScriptableObject
{
    [SerializeField] Currency[] currencies;
    
    public Currency[] Currencies => currencies;
}