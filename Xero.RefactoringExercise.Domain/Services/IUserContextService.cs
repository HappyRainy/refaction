using Ninject.Activation;
using Xero.RefactoringExercise.Domain.Context;

namespace Xero.RefactoringExercise.Domain.Services
{
    public interface IUserContextService
    {
        /// <summary>
        /// Gets the current <see cref="IUserContext"/> active in the current <see cref="Context"/>.
        /// </summary>
        IUserContext Current { get; }

        /// <summary>
        /// Finds the <see cref="IUserContext"/> with the specified <see cref="IUserContext.Token"/>.
        /// </summary>
        IUserContext GetByTicket(string ticket);

        /// <summary>
        /// Set 
        /// </summary>
        /// <param name="userContext"></param>
        void Login(IUserContext userContext);

    }
}
