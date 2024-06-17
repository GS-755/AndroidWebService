namespace AndroidWebService.Models.Utils
{
    public sealed class DbInstance
    {
        private static DbInstance execute;

        private DbInstance() { }

        public static DbInstance Execute
        {
            get
            {
                if(DbInstance.execute == null)
                {
                    DbInstance.execute = new DbInstance();
                }

                return DbInstance.execute;
            }
        }
        public DoAnAndroidEntities GetDatabase 
        {
            get => new DoAnAndroidEntities();
        }  
    }
}
