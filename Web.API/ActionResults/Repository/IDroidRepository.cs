namespace ActionResults.Repository
{
    public interface IDroidRepository
    {
        bool Delete(string name);
        Droid Get(string name);
        bool Put(Droid newDroid);
        Droid Update(Droid droid);
    }
}
