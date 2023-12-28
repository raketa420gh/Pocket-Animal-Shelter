using UnityEngine;

[System.Serializable]
public class AnimalData
{
    [SerializeField] Type _animalType;
    [SerializeField] float _storageHeight = 1;
    [Space]
    [SerializeField] Currency.Type _rewardType;
    [SerializeField] int _rewardAmount = 20;

    public Type AnimalType => _animalType;
    public float StorageHeight => _storageHeight;
    public int RewardAmount => _rewardAmount;
    public Currency.Type RewardType => _rewardType;

    public enum Type
    {
        Cat_1 = 0,
        Cat_2 = 1,
        Cat_3 = 2,
        Cat_4 = 3,
        Cat_5 = 4,
        Dog_1 = 5,
        Dog_2 = 6,
        Pig_1 = 7,
        Hare_1 = 8,
    }
}