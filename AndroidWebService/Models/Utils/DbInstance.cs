namespace AndroidWebService.Models.Utils
{
    using System;
    using System.Diagnostics;

    public sealed class DbInstance
    {
        public static DbInstance Execute 
            { get; private set; } = new DbInstance();

        private DoAnAndroidEntities db; 

        private DbInstance() 
        {
            try
            {
                if (DbInstance.Execute == null)
                {
                    DbInstance.Execute = new DbInstance();
                    this.db = new DoAnAndroidEntities();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public DoAnAndroidEntities GetDatabase 
        {
            get => this.db;
        }  
    }
}
