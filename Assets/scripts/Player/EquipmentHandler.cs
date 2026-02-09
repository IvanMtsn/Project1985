using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{
    public GameObject LeftItem;
    public GameObject RightItem;
    [SerializeField] Transform _leftItemHolder;
    [SerializeField] Transform _rightItemHolder;
    bool _isLeftItemChangeable;
    bool _isRightItemChangeable;
    void Start()
    {
        if (_leftItemHolder.childCount > 0)
        {
            LeftItem = _leftItemHolder.GetChild(0).gameObject;
            LeftItem.GetComponent<IItem>().Itemside = ItemSide.left;
        }
        if (_rightItemHolder.childCount > 0)
        {
            RightItem = _rightItemHolder.GetChild(0).gameObject;
            RightItem.GetComponent<IItem>().Itemside = ItemSide.right;
        }
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ItemPickup")) { return; }
        _isLeftItemChangeable = !LeftItem.name.Contains(other.GetComponent<Pickup>().ItemPrefab.name);
        _isRightItemChangeable = !RightItem.name.Contains(other.GetComponent<Pickup>().ItemPrefab.name);
    }
}
