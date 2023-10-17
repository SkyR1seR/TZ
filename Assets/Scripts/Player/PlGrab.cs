using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlGrab : IPlayerSystem
{
    private Transform _grabPoint;
    private Transform _cameraTransform;
    private float grabDistance = 10f;

    public PlGrab(Transform player, PlSettings settings)
    {
        grabDistance = settings.GrabDistance;

        _grabPoint = player.GetComponentInChildren<PlGrabPoint>().transform;
        _cameraTransform = player.GetComponentInChildren<Camera>().transform;
    }

    public void Tick()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            GrabItem();
        }
    }

    private void GrabItem()
    {
        RaycastHit hit;
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 rayDirection = _cameraTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, grabDistance))
        {
            if (!hit.transform.TryGetComponent(out IGrabbable item)) return;
            
            if (!item.Grab(_grabPoint))
            {
                item.Release();
            }
            if (_grabPoint.childCount > 1)
            {
                _grabPoint.GetChild(0).GetComponent<IGrabbable>().Release();
            }

        }
    }
}
