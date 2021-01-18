using System;
using Game.UI.Models;
using Game.UI.Views;

namespace Game.UI.Controllers
{

    /// <summary>
    /// Base class for all controllers.
    /// </summary>
    public abstract class ControllerBase<TModel, TView> : IDisposable where TModel : ModelBase where TView : ViewBase
    {

        protected bool disposed = false;
        protected readonly TModel model;
        protected readonly TView view;

        public ControllerBase(TModel model, TView view)
        {
            this.model = model;
            this.view = view;
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

                }
            }
        }

    }

}
