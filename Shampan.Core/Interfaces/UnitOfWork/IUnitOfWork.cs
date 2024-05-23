namespace UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IUnitOfWorkAdapter Create();

        IUnitOfWorkAdapter CreateSage();

        IUnitOfWorkAdapter CreateAuth();    

    }
}
