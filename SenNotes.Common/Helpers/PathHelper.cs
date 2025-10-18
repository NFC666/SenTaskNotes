namespace SenNotes.Common.Helpers
{
    public class PathHelper
    {
        public static string GetDbPath()
        {
            string appDataPath =Path.Combine( Environment
                .GetFolderPath(Environment
                    .SpecialFolder.ApplicationData)
                ,"SenNotes","Db","app.db");
            return appDataPath;
        }
    }
}