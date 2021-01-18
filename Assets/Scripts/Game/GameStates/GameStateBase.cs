using System;
using System.Threading.Tasks;
using Game.Core;
using Game.UI.Views;
using Game;

namespace Game.GameStates
{
    public interface IGameState
    {
        /// <summary>
        /// Should this GameMode remain active even when it is not
        /// the top GameMode on the stack?
        /// </summary>
        bool AlwaysActive { get; }
        
        /// <summary>
        /// Called when this GameMode is first added to the stack.
        /// </summary>
        void OnAdd();

        /// <summary>
        /// Called when this GameMode is at the top of the stack.
        /// </summary>
        void OnActivate(Action activated = null);

        /// <summary>
        /// Called when this GameMode is no longer at the top of the stack.
        /// </summary>
        void OnDeactivate(Action deactivated = null);

        /// <summary>
        /// Called when this GameMode is removed from the stack.
        /// </summary>
        void OnRemove();
    }
    
    /// <summary>
    /// Base class for all GameModes with context.
    /// </summary>
    public abstract class GameStateBase<TContext> : IGameState where TContext : GameModeContextBase
    {
        /// <summary>
        /// The context passed into this GameMode upon creation.
        /// </summary>
        public TContext Context;

        /// <summary>
        /// Tracking if this game mode is active.
        /// </summary>
        private bool isActive = false;
        
        private Task<DialogViewBase> viewTask;

        protected Task<TView> CreateView<TView>(UIRoot.CanvasLayer canvasLayer = UIRoot.CanvasLayer.Default) where TView : DialogViewBase
        {
            var task = ServiceLocator.ViewManager.CreateView<TView>(Context.ViewPrefabPath, canvasLayer);

            viewTask = task.ContinueWith(inner => inner.Result as DialogViewBase, TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        /// <inheritdoc/>
        public virtual bool AlwaysActive
        {
            get { return false; }
        }

        /// <inheritdoc/>
        public abstract void OnAdd();

        /// <inheritdoc/>
        public virtual void OnActivate(Action activated = null)
        {
            isActive = true;

            if (viewTask != null)
            {
                viewTask.ContinueWith(task =>
               {
                   if (!task.IsFaulted && isActive)
                   {
                       task.Result.Open(activated);
                   }
               },TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                if (activated != null)
                {
                    activated();
                }
            }
        }

        /// <inheritdoc/>
        public virtual void OnDeactivate(Action deactivated = null)
        {
            isActive = false;

            if (viewTask != null)
            {
                viewTask.ContinueWith(task =>
               {
                   if (!task.IsFaulted && !isActive)
                   {
                       task.Result.Close(deactivated);
                   }
               }, TaskContinuationOptions.ExecuteSynchronously);
            }
            else
            {
                if (deactivated != null)
                {
                    deactivated();
                }
            }
        }

        /// <inheritdoc/>
        public virtual void OnRemove()
        {
            if (viewTask != null)
            {
                viewTask.ContinueWith(task =>
               {
                   if (!task.IsFaulted)
                   {
                       task.Result.Destroy();
                   }
               }, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
    }
}