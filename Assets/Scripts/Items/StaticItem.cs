using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticItem : MonoBehaviour
{
    public string ItemName;
    private bool _isFixed;
    private Item _parentPart;

    private Renderer _renderer;
    private Material hintMaterial;
    private Material defaultMaterial;

    private Collider _itemCollider;
    private Rigidbody _itemRigidbody;

    private void Start()
    {
        _itemCollider = GetComponent<Collider>();
        _itemRigidbody = GetComponent<Rigidbody>();
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
    private void OnTriggerEnter(Collider other)
    {
        if (_isFixed) return;
        if (!other.TryGetComponent(out DynamicItem item)) return;
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
        if (!other.TryGetComponent(out Item item)) return;
        if (item.CheckStatic()) return;

        HideHint();
        item.ClearFixPart();
    }
    public void FixPart()
    {
        _isFixed = true;
        _renderer.enabled = true;
        _renderer.material = defaultMaterial;
        _itemCollider.isTrigger = false;
    }
    public bool CheckFix()
    {
        return _isFixed;
    }
    public bool CheckPart(DynamicItem item)
    {
        if (ItemName != item.ItemName) return false;
        return true;
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
}
