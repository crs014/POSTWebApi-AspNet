namespace POSTWebApi.Services
{
    public class DbManagement
    {
        public DbManagement()
        {
            db = new DbPOS();
            db.Configuration.LazyLoadingEnabled = false;
        }

        public DbPOS db { get; }

    }
}