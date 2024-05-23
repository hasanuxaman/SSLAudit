using System;
using Shampan.Core.Interfaces.UnitOfWork;

namespace UnitOfWork.Interfaces
{
    public interface IUnitOfWorkAdapter : IDisposable
    {
        IUnitOfWorkRepository Repositories { get; }
        void SaveChanges();
        void RollBack();
    }
}
