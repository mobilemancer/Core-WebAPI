using System;
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
                Message = "Too many fingers on keyboard error!"
            };
            repo.TryAdd(error.ErrorCode, error);

            error = new Error
            {
                ErrorCode = 666,
                HttpCode = 404,
                Message = "Is it just me or is it getting hot in here?"
            };
            repo.TryAdd(error.ErrorCode, error);
        }

        public Error GetByErrorCode(int errorCode)
        {
            Error error;
            repo.TryGetValue(errorCode, out error);
            Console.WriteLine(error);
            return error;
        }
    }
}
