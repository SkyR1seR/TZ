using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player Settings", menuName = "Player/Player Settings", order = 0)]
public class PlSettings : ScriptableObject
{
    public float MouseSensitivity => _mouseSensitivity;
    public float MoveSpeed => _moveSpeed;
    public float Gravity => _gravity;

    public float GrabDistance => _grabDistance;

    [SerializeField]private float _mouseSensitivity;
    [SerializeField]private float _moveSpeed;
    [SerializeField] private float _gravity;

    [SerializeField] private float _grabDistance;
}
