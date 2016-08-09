using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xero.RefactoringExercise.DAL.Supports;

namespace Xero.RefactoringExercise.Tests.Support.Data
{
    public static class DbSetMockingHelper
    {
        public static Mock<DbSet<T>> CreateMockedDbSet<T>(IQueryable<T> data)
            where T : class 
        {
            var mockedDbSet = new Mock<DbSet<T>>();
            mockedDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockedDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockedDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockedDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockedDbSet;
        }
    }
}
