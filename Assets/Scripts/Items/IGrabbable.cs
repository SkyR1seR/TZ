using UnityEngine;

public interface IGrabbable
{
    public bool Grab(Transform grabPoint);

    public void Release();
}