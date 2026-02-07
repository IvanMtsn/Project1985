using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour, IWeaponProperties, IItemSide
{
    [Header("ammo and cooldown and damage")]
    [SerializeField] protected float _spreadAngle;
    [SerializeField] protected float _ammoCost;
    [SerializeField] protected float _firingCooldown;
    [SerializeField] protected float _burstAmount;
    [SerializeField] protected float _burstCooldown;
    [SerializeField] protected float _damage;
    public float Damage { 
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }
    [Header("orientation")]
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Transform _visualFirePoint;
    [SerializeField] protected FireMode _fireMode;
    protected bool _isBursting;
    protected float _lastTimeSinceFire = 0;
    public bool IsDroppable { get; set; }
    public ItemSide Itemside { get; set; }
    protected IAmmo _ammoComponent;
    protected Animator _animator;
    protected bool _shotQueued;
    protected void Start()
    {
        _ammoComponent = GetComponent<IAmmo>();
        _animator = GetComponent<Animator>();
        if (_ammoComponent == null)
        {
            Debug.LogError($"NO IAmmo COMPONENT FOUND on {gameObject.name}!");
        }
    }
    protected void Update()
    {
        if (_lastTimeSinceFire < _firingCooldown)
        {
            _lastTimeSinceFire += Time.deltaTime;
        }
        if (GetComponent<ReloadableComponent>() != null && GetComponent<ReloadableComponent>().IsReloading)
        {
            return;
        }
        HandleGunControls();
    }
    protected void HandleGunControls()
    {
        bool firing = (Itemside == ItemSide.left) ? InputManager.Instance.FiringLeft : InputManager.Instance.FiringRight;
        bool firePressed = (Itemside == ItemSide.left) ? InputManager.Instance.FirePressedLeft : InputManager.Instance.FirePressedRight;
        bool reloadPressed = (Itemside == ItemSide.left) ? InputManager.Instance.ReloadLeft : InputManager.Instance.ReloadRight;
        //Lord help me
        switch (_fireMode)
        {
            case FireMode.Fullauto:
                if (firing && _lastTimeSinceFire >= _firingCooldown) Shoot();
                break;
            case FireMode.Semiauto:
                if (firePressed)
                    /**NTS: because spamming the mousebutton is slower than rythmically clicking for SOME REASON?????????*/
                    _shotQueued = true;

                if (_shotQueued && _lastTimeSinceFire >= _firingCooldown)
                {
                    Shoot();
                    _shotQueued = false;
                }
                break;
            case FireMode.Burst:
                if (firePressed && !_isBursting && _lastTimeSinceFire >= _firingCooldown) StartCoroutine(ShootBurst());
                break;
        }
        if (GetComponent<ReloadableComponent>() != null && reloadPressed) GetComponent<ReloadableComponent>().Reload();
    }
    protected IEnumerator ShootBurst()
    {
        _isBursting = true;
        for (int i = 0; i < _burstAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(_burstCooldown);
        }
        _isBursting = false;
    }
    protected abstract void Shoot();
}
