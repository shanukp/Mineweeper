using System;
using System.Collections.Generic;
using Game.GameStates;
using Game.Utility;

namespace Game.Core
{
    /// <summary>
    /// Handles creating and removing game modes.
    /// </summary>
    public class GameStateManager : IDisposable
    {
        private Stack<IGameState> gameModes = new Stack<IGameState>();
        private bool disposed = false;


        /// <summary>
        /// Adds a new GameMode with context.
        /// </summary>
        public void Add<TGameMode, TGameModeContext>(TGameModeContext context, Action activated = null)
            where TGameMode : GameStateBase<TGameModeContext>, new()
            where TGameModeContext : GameModeContextBase
        {
            Trace("Create", typeof(TGameMode).Name);
            var gameMode = new TGameMode();
            gameMode.Context = context;

            if (gameModes.Count > 0)
            {
                var currentGameMode = gameModes.Peek();
                if (!currentGameMode.AlwaysActive && context.DeactivatesPreviousGameMode)
                {
                    Trace("Deactivate", currentGameMode.GetType().Name);
                    currentGameMode.OnDeactivate();
                }
            }
            gameModes.Push(gameMode);
            Trace("Add", gameMode.GetType().Name);
            gameMode.OnAdd();
            Trace("Activate", gameMode.GetType().Name);
            gameMode.OnActivate(activated);
        }

        /// <summary>
        /// Checks if the current GameMode is of type T
        /// </summary>
        public bool Current<TGameMode>() where TGameMode : IGameState
        {
            if (gameModes.Count == 0)
            {
                return false;
            }
            return gameModes.Peek() is TGameMode;
        }

        /// <summary>
        /// Checks of a GameMode of type T exists on the stack
        /// </summary>
        public bool Contains<TGameMode>() where TGameMode : IGameState
        {
            if (gameModes.Count == 0)
            {
                return false;
            }

            foreach (var gameMode in gameModes)
            {
                if (gameMode is TGameMode)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the top GameMode on the stack.
        /// </summary>
        public void Pop(Action deactivated = null)
        {
            if (gameModes.Count > 0)
            {
                var gameMode = gameModes.Pop();
                Trace("Deactivate", gameMode.GetType().Name);
                gameMode.OnDeactivate(() =>
                {
                    deactivated?.Invoke();
                    Trace("Remove Async", gameMode.GetType().Name);
                    gameMode.OnRemove();
                });
                if (gameModes.Count > 0)
                {
                    var currentGameMode = gameModes.Peek();
                    Trace("Activate", gameMode.GetType().Name);
                    currentGameMode.OnActivate();
                }
            }
        }

        /// <summary>
        /// Removes all GameModes instantly.
        /// </summary>
        public void PopAll()
        {
            while (gameModes.Count > 0)
            {
                var gameMode = gameModes.Pop();
                Trace("Deactivate", gameMode.GetType().Name);
                gameMode.OnDeactivate();
                Trace("Remove", gameMode.GetType().Name);
                gameMode.OnRemove();
            }
        }

        /// <summary>
        /// Pops GameModes until a GameMode of type T is found.
        /// If GameMode of type T not in stack, no action is taken.
        /// </summary>
        public void PopUntil<TGameState>() where TGameState : IGameState
        {
            if (gameModes.Count == 0 || !Contains<TGameState>())
            {
                return;
            }

            while (!Current<TGameState>() && gameModes.Count > 0)
            {
                Pop();
            }
        }

        /// <summary>
        /// Pops GameModes until a GameMode of type T is found, then pops it as well.
        /// If GameMode of type T not in stack, no action is taken.
        /// </summary>
        public void PopUntilAndIncluding<TGameMode>() where TGameMode : IGameState
        {
            PopUntil<TGameMode>();

            if (gameModes.Count == 0)
            {
                return;
            }

            if (Current<TGameMode>())
            {
                Pop();
            }
        }

        
        /// <summary>
        /// Utility method to trace log and track logic in Bugsnag.
        /// </summary>
        /// <param name="action">Action to trace track.</param>
        /// <param name="gameMode">Game mode which the action occured for.</param>
        private void Trace(string action, string gameMode)
        {

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                if (disposing)
                {
                    PopAll();
                }
            }
        }

    }
}
