using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Item.Type _type;

    public Item.Type Type => _type;
}