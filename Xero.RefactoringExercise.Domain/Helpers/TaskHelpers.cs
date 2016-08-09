using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Xero.RefactoringExercise.Domain.Helpers
{
    /// <summary>
    /// Helpers for dealing with <see cref="Task" /> objects, inspiration taken from the ASP.NET
    /// internals and the following post:
    /// http://bradwilson.typepad.com/blog/2012/04/tpl-and-servers-pt4.html
    /// </summary>
    public static class TaskHelpers
    {
        static readonly Task<object> CompletedTaskReturningNull = FromResult<object>(null);
        static readonly Task DefaultCompleted = FromResult(new AsyncVoid());

        public static Task Canceled()
        {
            return CancelCache<AsyncVoid>.Instance;
        }

        public static Task<TResult> Canceled<TResult>()
        {
            return CancelCache<TResult>.Instance;
        }

        public static Task Completed()
        {
            return DefaultCompleted;
        }

        public static Task<object> NullResult()
        {
            return CompletedTaskReturningNull;
        }

        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            var source = new TaskCompletionSource<TResult>();
            source.SetException(exception);
            return source.Task;
        }

        public static Task FromErrors(IEnumerable<Exception> exceptions)
        {
            return FromErrors<AsyncVoid>(exceptions);
        }

        public static Task<TResult> FromErrors<TResult>(IEnumerable<Exception> exceptions)
        {
            var source = new TaskCompletionSource<TResult>();
            source.SetException(exceptions);
            return source.Task;
        }

        public static Task<TResult> FromResult<TResult>(TResult result)
        {
            var source = new TaskCompletionSource<TResult>();
            source.SetResult(result);
            return source.Task;
        }

        static class CancelCache<TResult>
        {
            public static readonly Task<TResult> Instance;

            static CancelCache()
            {
                Instance = GetCancelledTask();
            }

            static Task<TResult> GetCancelledTask()
            {
                var source = new TaskCompletionSource<TResult>();
                source.SetCanceled();
                return source.Task;
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct AsyncVoid { }
    }

}
