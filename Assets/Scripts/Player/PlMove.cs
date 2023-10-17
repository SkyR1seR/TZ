using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlMove : IPlayerSystem
{
    private float mouseSensitivity = 2.0f;
    private float moveSpeed = 5.0f;
    private float gravity = -9.81f;

    private Transform _playerTransform;
    private Transform _cameraTransform;

    private float rotationX = 0.0f;
    private CharacterController _cc;
    private Vector3 velocity;

    public PlMove(Transform player, PlSettings settings)
    {
        mouseSensitivity = settings.MouseSensitivity;
        moveSpeed = settings.MoveSpeed;
        gravity = settings.Gravity;

        _playerTransform = player;
        _cc = _playerTransform.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTransform = _playerTransform.GetComponentInChildren<Camera>().transform;
    }

    private void Move(Vector2 dir) 
    {
        Vector3 movement = _playerTransform.forward * dir.y + _playerTransform.right * dir.x;
        _cc.Move(movement * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        _cc.Move(velocity * Time.deltaTime);
    }

    private void RotateCamera(Vector2 dir)
    {
        // Поворот камеры по горизонтали (влево/вправо)
        _playerTransform.Rotate(Vector3.up, dir.x * mouseSensitivity);

        // Поворот камеры по вертикали (вверх/вниз), ограничение на угол обзора
        rotationX -= dir.y * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
        _cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    public void Tick()
    {
        Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move(moveDir);
        Vector2 cameraDir = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        RotateCamera(cameraDir);
    }
}
