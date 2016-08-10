namespace Xero.RefactoringExercise.Domain.Context
{
    /// <summary>
    /// Simple user context interface to hold user info
    /// </summary>
    public interface IUserContext
    {
        string IdentityName { get; }
    }
}
