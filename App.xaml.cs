namespace LaundryScan
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        static DbController database;

     
        public static DbController Database
        {
            get
            {
                if (database == null)
                {
                    database = new DbController();
                }
                return database;
            }
        }
    }
}
