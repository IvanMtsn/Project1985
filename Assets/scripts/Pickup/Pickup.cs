using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected Sprite _itemIcon;
    public GameObject ItemPrefab;
    public Sprite ItemIcon => _itemIcon;
}
