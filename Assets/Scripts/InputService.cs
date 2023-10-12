using SSLAB;
using UnityEngine;

public interface IInputService : ITickable, IService
{
    bool IsPlayerMoveForwardPressed();
    bool IsPlayerTurnLeftPressed();
    bool IsPlayerTurnRightPressed();
    bool IsPlayerShootPressed();
}

public class InputService : IInputService
{
    public bool IsPlayerMoveForwardPressed()
    {
        return _playerForward;
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

    public void Tick(float _)
    {
        _playerForward = Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W);
        _playerLeftTurn = Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A);
        _playerRightTurn = Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D);
        _playerShoot = Input.GetKey(KeyCode.Space);

    }

    bool _playerForward, _playerLeftTurn, _playerRightTurn, _playerShoot;
}