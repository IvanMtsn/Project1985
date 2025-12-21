using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject WeaponPrefab;
    [SerializeField] Mesh WeaponModel;
    [SerializeField] Texture WeaponTexture;
    MeshRenderer _mr;
    MeshFilter _mf;
    private void Start()
    {
        _mr = GetComponent<MeshRenderer>();
        _mf = GetComponent<MeshFilter>();
        _mr.material.mainTexture = WeaponTexture;
        _mf.mesh = WeaponModel;
    }
}
