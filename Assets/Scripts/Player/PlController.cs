using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlController : MonoBehaviour
{
    [SerializeField] private PlSettings settings;
    [SerializeField]private List<IPlayerSystem> playerSystems = new List<IPlayerSystem>();

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        foreach (IPlayerSystem system in playerSystems)
        {
            system.Tick();
        }
    }

    public void Init()
    {
        PlMove moveSys = new PlMove(transform, settings);
        playerSystems.Add(moveSys);
        PlGrab grabSys = new PlGrab(transform, settings);
        playerSystems.Add(grabSys);
    }
}
