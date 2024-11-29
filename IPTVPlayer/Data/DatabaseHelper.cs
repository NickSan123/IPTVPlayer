using SQLite;
using System.IO;
using Android.OS;
using IPTVPlayer.Models;

namespace IPTVPlayer.Data
{
    public class DatabaseHelper
    {
        private static SQLiteConnection _database;
        private static readonly object Locker = new object();


        public static SQLiteConnection GetConnection()
        {
            if (_database == null)
            {
                lock (Locker)
                {
                    if (_database == null)
                    {
                        // Diretório de armazenamento pessoal
                        string directoryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                        // Verificar se o diretório existe, se não, criá-lo
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string dbPath = Path.Combine(directoryPath, "canais.db");

                        // Verifique se o banco de dados pode ser aberto ou criado
                        _database = new SQLiteConnection(dbPath);
                        _database.CreateTable<Canal>(); // Cria a tabela 'Canal' no banco
                    }
                }
            }
            return _database;
        }
    }
}
