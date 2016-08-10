using System;
using System.Collections.Generic;

namespace Xero.RefactoringExercise.Domain.Context
{

    /// <summary>
    /// Application context, used to help better logging per request/task
    /// </summary>
    public abstract class AppContext
    {
        protected AppContext(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            _id = id;
            _items = new Dictionary<string, object>();
        }

        readonly string _id;

        /// <summary>
        /// An ID for this context instance.
        /// </summary>
        public string Id
        {
            get
            {
                CheckDisposed();
                return _id;
            }
        }

        readonly IDictionary<string, object> _items;

        /// <summary>
        /// A key/value collection that can be used to store data to be shared with other code
        /// running in the same context.
        /// </summary>
        public IDictionary<string, object> Items
        {
            get
            {
                CheckDisposed();
                return _items;
            }
        }

        bool _disposed;

        /// <summary>
        /// A context should be disposed after the job it was created to perform has ended.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _disposed = true;
        }

        protected void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException($"{GetType().Name} with Id {_id} is disposed and cannot be used");
        }
    }

}

