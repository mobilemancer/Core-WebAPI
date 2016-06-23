namespace Error.Repository
{
    public interface IErrorRepository
    {
        Error GetByErrorCode(int errorCode);
    }
}
