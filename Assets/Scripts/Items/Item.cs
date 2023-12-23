using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Content/Items/Item")]
public class Item : ScriptableObject
{
    [SerializeField] Type itemType;
    [Space]
    [SerializeField] Sprite icon;
    [SerializeField] GameObject model;
    [SerializeField] float modelHeight;

    public Type ItemType => itemType;
    public Sprite Icon => icon;
    public GameObject Model => model;
    public float ModelHeight => modelHeight;

    public enum Type
    {
        None = -1,
        Soap = 0,
        Injection = 1,
        Pill = 2,
        Mixture = 3,
        Clism = 4,
        Scalpel = 5
    }
}