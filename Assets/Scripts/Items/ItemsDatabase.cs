using UnityEngine;

[CreateAssetMenu(fileName = "Items Database", menuName = "Content/Items/Items Database")]
public class ItemsDatabase : ScriptableObject
{
    [SerializeField] Item[] items;
    public Item[] Items => items;
}