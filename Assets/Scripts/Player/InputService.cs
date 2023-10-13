using SSLAB;
using UnityEngine;

public interface IInputService : ITickable, IService
{
    bool IsPlayerMoveForwardPressed();
    bool IsPlayerMoveBackwardPressed();
    bool IsPlayerTurnLeftPressed();
    bool IsPlayerTurnRightPressed();
    bool IsPlayerShootPressed();

    bool IsAnyButtonPressed();
}

public class InputService : IInputService
{
    public bool IsPlayerMoveForwardPressed()
    {
        return _playerForward;
    }

    public bool IsPlayerMoveBackwardPressed()
    {
        return _playerBackward;
    }

    public bool IsPlayerTurnLeftPressed()
    {
        return _playerLeftTurn;
    }

    public bool IsPlayerTurnRightPressed()
    {
        return _playerRightTurn;
    }

    public bool IsPlayerShootPressed()
    {
        return _playerShoot;
    }

    public bool IsAnyButtonPressed()
    {
        return _anyButton;
    }

    public void Tick(float _)
    {
        _playerForward = Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W);
        _playerBackward = Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S);
        _playerLeftTurn = Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A);
        _playerRightTurn = Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D);
        _playerShoot = Input.GetKey(KeyCode.Space);
        _anyButton = Input.anyKeyDown;
    }

    bool _playerForward, _playerBackward, _playerLeftTurn, _playerRightTurn, _playerShoot, _anyButton;
}