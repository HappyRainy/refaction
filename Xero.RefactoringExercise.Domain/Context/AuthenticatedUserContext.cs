namespace Xero.RefactoringExercise.Domain.Context
{
    /// <summary>
    /// Simple user context to hold authenticated user info
    /// </summary>
    public class AuthenticatedUserContext : IUserContext
    {
        public AuthenticatedUserContext(string identityName)
        {
            IdentityName = identityName;
        }

        public string IdentityName { get; }
    }
}
