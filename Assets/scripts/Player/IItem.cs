using UnityEngine;

public interface IItem
{
    public ItemSide Itemside { get; set; }
    public bool IsDroppable { get; }
    public GameObject ItemPickup { get; }
}
