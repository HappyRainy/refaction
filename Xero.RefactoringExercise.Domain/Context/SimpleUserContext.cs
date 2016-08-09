namespace Xero.RefactoringExercise.Domain.Context
{
    public class SimpleUserContext : IUserContext
    {

        public SimpleUserContext(string identityName)
        {
            IdentityName = identityName;
        }

        public string IdentityName { get; }
    }
}
