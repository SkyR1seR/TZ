using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public abstract class Item : MonoBehaviour
{
    public string ItemName;
    [SerializeField] private bool isStaticPart;
    private bool _isFixed;
    private Item _parentPart;
    private bool _inFixZone;
    private Item _partForFix;

    private Renderer _renderer;
    private Material hintMaterial;
    private Material defaultMaterial;

    private Collider _itemCollider;
    private Rigidbody _itemRigidbody;

    private bool _isGrabbed = false;

    private void Start()
    {
        _itemCollider = GetComponent<Collider>();
        _itemRigidbody = GetComponent<Rigidbody>();
        _itemCollider.isTrigger = false;
        _itemRigidbody.isKinematic = false;

        if (!isStaticPart) return;
        _itemRigidbody.isKinematic = true;
        _itemCollider.isTrigger = true;
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
        defaultMaterial = _renderer.material;
        hintMaterial = new Material(defaultMaterial);
        hintMaterial.color = new Color(0, 1, 0);

        if (transform.parent == null) return;
        if (!transform.parent.TryGetComponent(out Item item)) return;
        _parentPart = item;
    }
    public bool Grab(Transform grabPoint)
    {
        if (_isGrabbed) return false;
        if (isStaticPart) return false;

        _isGrabbed=true;
        _itemRigidbody.isKinematic = true;
        _itemCollider.isTrigger = true;
        transform.SetParent(grabPoint);
        transform.localPosition = Vector3.zero;

        return true;
    }

    public void Release()
    {
        if (!_isGrabbed) return;
        if (isStaticPart) return;

        if (_inFixZone)
        {
            _partForFix.FixPart();
            Destroy(gameObject);
        }

        transform.SetParent(null);
        _isGrabbed=false;
        _itemRigidbody.isKinematic = false;
        _itemCollider.isTrigger = false;

    }

    public bool CheckStatic()
    {
        return isStaticPart;
    }
    public bool CheckFix()
    {
        return _isFixed;
    }
    public bool CheckPart(Item item)
    {
        if (ItemName != item.ItemName) return false;
        return true;
    }
    public void SetFixPart(Item fixPart)
    {
        _partForFix = fixPart;
        _inFixZone = true;
    }
    public void ClearFixPart()
    {
        _partForFix = null;
        _inFixZone = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isFixed) return;
        if (!isStaticPart) return;
        if (!other.TryGetComponent(out Item item)) return;
        if (item.CheckStatic()) return;
        if (!CheckPart(item)) return;

        if (_parentPart != null)
        {
            if (_parentPart.CheckFix())
            {
                ShowHint();
            }
        }
        else
        {
            ShowHint();
        }
        item.SetFixPart(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isFixed) return;
        if (!isStaticPart) return;
        if (!other.TryGetComponent(out Item item)) return;
        if (item.CheckStatic()) return;

        HideHint();
        item.ClearFixPart();
    }

    private void ShowHint()
    {
        _renderer.enabled = true;
        _renderer.material = hintMaterial;
    }
    private void HideHint()
    {
        _renderer.enabled = false;
    }

    private void FixPart()
    {
        _isFixed = true;
        _renderer.enabled = true;
        _renderer.material = defaultMaterial;
        _itemCollider.isTrigger = false;
    }
}
