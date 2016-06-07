namespace ActionResults.Repository
{
    public interface IDroidRepository
    {
        bool Exists(string name);
        bool Delete(string name);
        Droid Get(string name);
        bool Put(Droid newDroid);
    }
}
