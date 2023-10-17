using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicItem : MonoBehaviour, IGrabbable
{
    public string ItemName;
    private bool _inFixZone;
    private StaticItem _partForFix;

    private Collider _itemCollider;
    private Rigidbody _itemRigidbody;

    private bool _isGrabbed = false;

    private void Start()
    {
        _itemCollider = GetComponent<Collider>();
        _itemRigidbody = GetComponent<Rigidbody>();
        _itemCollider.isTrigger = false;
        _itemRigidbody.isKinematic = false;
    }
    public bool Grab(Transform grabPoint)
    {
        if (_isGrabbed) return false;

        _isGrabbed = true;
        _itemRigidbody.isKinematic = true;
        _itemCollider.isTrigger = true;
        transform.SetParent(grabPoint);
        transform.localPosition = Vector3.zero;

        return true;
    }

    public void Release()
    {
        if (!_isGrabbed) return;

        if (_inFixZone)
        {
            _partForFix.FixPart();
            Destroy(gameObject);
        }

        transform.SetParent(null);
        _isGrabbed = false;
        _itemRigidbody.isKinematic = false;
        _itemCollider.isTrigger = false;

    }
    public void SetFixPart(StaticItem fixPart)
    {
        _partForFix = fixPart;
        _inFixZone = true;
    }
    public void ClearFixPart()
    {
        _partForFix = null;
        _inFixZone = false;
    }
}
