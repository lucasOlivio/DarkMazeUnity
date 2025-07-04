using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public enum State
    {
        MainMenu,
        Playing,
        Paused,
        GameWin,
        GameOver
    }

    private State currentState;

    public GameState(State initialState = State.Playing)
    {
        ChangeState(initialState);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public bool IsPlaying() => currentState == State.Playing;
    public bool IsMainMenu() => currentState == State.MainMenu;
    public bool IsPaused() => currentState == State.Paused;
    public bool IsGameWin() => currentState == State.GameWin;
    public bool IsGameOver() => currentState == State.GameOver;
}
