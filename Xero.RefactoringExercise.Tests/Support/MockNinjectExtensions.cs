using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ninject;
using Ninject.Activation;

namespace Xero.RefactoringExercise.Tests.Support
{
    public static class MockNinjectExtensions
    {
        /// <summary>
        /// Rebinds a type to a constant mock.
        /// </summary>
        public static void RebindMock<T>(this IKernel kernel, Mock<T> implementation)
            where T : class
        {
            kernel.Rebind<T>().ToConstant(implementation.Object);
        }

        /// <summary>
        /// Rebinds a type to a constant mock.
        /// </summary>
        public static void RebindMock<T>(this IKernel kernel, MockBehavior behavior = MockBehavior.Default)
            where T : class
        {
            kernel.RebindMock<T, T>(behavior);
        }

        /// <summary>
        /// Rebinds a type to a constant mock.
        /// </summary>
        public static void RebindMock<T, TImplemenation>(this IKernel kernel, MockBehavior behavior = MockBehavior.Default)
            where T : class
            where TImplemenation : class, T
        {
            kernel.Rebind<T>().ToProvider(new MockedProvider<TImplemenation>(behavior));
        }

        public static Mock<T> GetMockService<T>(this IKernel kernel)
            where T : class
        {
            var mocked = kernel.Get<T>();

            return Mock.Get(mocked);
        }

        /// <summary>
        /// Creates a mock for type <see cref="T"/>. If the type is a class which has constructor
        /// dependencies, they will be resolved using ninject and passed to the mock as arguments.
        /// </summary>
        public static Mock<T> CreateMock<T>(IKernel kernel, MockBehavior behavior, bool callBase = true)
            where T : class
        {
            // For interfaces we create a mock in the usual way
            if (!typeof(T).IsClass)
                return new Mock<T>(behavior);

            // For classes we potentially need to resolve constructor dependencies for the mocked object

            var constructors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Find ninject constructor for class (hopefully this is the correct one)
            var constructor = constructors.Length == 1
                ? constructors.Single()
                : constructors.FirstOrDefault(c => c.GetCustomAttribute(typeof(InjectAttribute), false) != null);

            if (constructor == null) throw new ArgumentException("Type does not have a suitable constructor");

            var constructorParams = constructor.GetParameters();

            // Resolve all constructor dependencies now
            var dependencies = constructorParams.Select(p => kernel.Get(p.ParameterType)).ToArray();

            // Create mock with resolved dependencies
            return new Mock<T>(behavior, dependencies) { CallBase = callBase };
        }

        /// <summary>
        /// Ninject provider which returns singleton instance using <see cref="CreateMock{T}"/>.
        /// </summary>
        class MockedProvider<T> : Provider<T>
            where T : class
        {
            public T Instance { get; private set; }

            public MockBehavior Behavior { get; private set; }

            public MockedProvider(MockBehavior behavior)
            {
                Behavior = behavior;
            }

            protected override T CreateInstance(IContext context)
            {
                return Instance ?? (Instance = CreateMock<T>(context.Kernel, Behavior).Object);
            }
        }
    }
}
