using System.Collections.Concurrent;

namespace Error.Repository
{
    public class ErrorRepository : IErrorRepository
    {
        private static ConcurrentDictionary<int, Error> repo { get; set; }
        public ErrorRepository()
        {
            repo = new ConcurrentDictionary<int, Error>();
            Seed();

        }

        private void Seed()
        {
            var error = new Error
            {
                ErrorCode = 101,
                HttpCode = 404,
                Message = "Dude, shit happens!"
            };
            repo.TryAdd(error.ErrorCode, error);
        }

        public Error GetByErrorCode(int errorCode)
        {
            Error error;
            repo.TryGetValue(errorCode, out error);
            return error;
        }
    }
}
