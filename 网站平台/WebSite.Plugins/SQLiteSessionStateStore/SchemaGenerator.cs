using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace WebSite.Plugins
{
    class SchemaGenerator
    {
        private String _databaseFile;

        public SchemaGenerator(String databaseFile)
        {
            _databaseFile = databaseFile;
        }

        public void Create()
        {
            if (File.Exists(_databaseFile)) return;

            SQLiteConnection.CreateFile(_databaseFile);

            Assembly assembly = typeof(SchemaGenerator).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(typeof(SchemaGenerator), "WebSite.Plugins.Install.txt"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + _databaseFile))
                    {
                        connection.Open();
                        var cmd = connection.CreateCommand();
                        cmd.CommandText = sr.ReadToEnd();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
