using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Xero.RefactoringExercise.DAL.Entities;
using Xero.RefactoringExercise.DAL.Supports;
using Xero.RefactoringExercise.Domain;
using Xero.RefactoringExercise.Domain.Context;
using Xero.RefactoringExercise.Domain.Services;
using Xero.RefactoringExercise.WebApi;

namespace Xero.RefactoringExercise.Tests.Support
{
    public abstract class UnitTestBase
    {
        /// <summary>
        /// The test ninject kernel 
        /// </summary>
        public MoqMockingKernel Kernel { get; private set; }

        protected IRepository Repository { get; private set; }

        protected Mock<XeroRefactoringExerciseContext> DbContext { get; private set; }

        protected Mock<IUserContextService> UserContextService { get; private set; }

        protected Mock<IUserContext> UserContext;

        protected UnitTestBase()
        {
            Kernel = new MoqMockingKernel(new NinjectSettings
            {
                // Allow use of private constructors for domain objects in ORM
                InjectNonPublic = true
            });

            DbContext = new Mock<XeroRefactoringExerciseContext>();
            UserContext = new Mock<IUserContext>();
            UserContextService = new Mock<IUserContextService>();

            Repository = new EntityFrameworkRepository<XeroRefactoringExerciseContext>(DbContext.Object);

            DomainModule.DefineMapper();
            WebApiModule.DefineMapper();
        }

    }
}
