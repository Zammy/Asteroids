using SSLAB;
using System;

public enum GameState
{
    Uninitialized = 0,
    MainMenu,
    Game,
    EndGame
}

public interface IGameStateManager : IService, IInitializable
{
    GameState CurrentState { get; }
    void ChangeStateTo(GameState state);
    Action<GameState> OnStateChanged { get; set; }
}

public class GameStateManager : IGameStateManager
{
    public GameState CurrentState { get; private set; }

    public Action<GameState> OnStateChanged { get; set; }

    public GameStateManager()
    {
        CurrentState = GameState.Uninitialized;
    }

    public void Init()
    {
        ChangeStateTo(GameState.MainMenu);
    }

    public void ChangeStateTo(GameState newState)
    {
        CurrentState = newState;

        if (OnStateChanged != null)
            OnStateChanged(newState);
    }
}