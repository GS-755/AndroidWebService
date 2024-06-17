namespace AndroidWebService.Models.Utils
{
    using System;
    using System.Diagnostics;

    public sealed class DbInstance
    {
        private static DbInstance execute;

        private DoAnAndroidEntities db; 

        private DbInstance() 
        {
            try
            {
                this.db = new DoAnAndroidEntities();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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
            get => this.db;
        }  
    }
}
