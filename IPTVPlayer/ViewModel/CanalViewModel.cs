using CommunityToolkit.Mvvm.Input;
using IPTVPlayer.Data;
using IPTVPlayer.Models;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IPTVPlayer.ViewModels
{
    public class CanalViewModel
    {
        private readonly SQLiteConnection _db;

        public ICommand OnCanalSelectedCommand { get; }
        public ICommand OnCanalLongSelectedCommand { get; }
        public ObservableCollection<Canal> Canais { get; set; }

        public CanalViewModel()
        {
            _db = DatabaseHelper.GetConnection();
            Canais = new ObservableCollection<Canal>();
        }

        // Carrega os canais do banco de dados de forma assíncrona
        public async Task LoadCanaisAsync()
        {
            var canais = await Task.Run(() => _db.Table<Canal>().ToList());
            foreach (var canal in canais)
            {
                if (!Canais.Any(c => c.Id == canal.Id))
                {
                    Canais.Add(canal);
                }
            }
        }

        public Canal LoadCanais(string url)
        {
            return _db.Table<Canal>().FirstOrDefault(x => x.UrlStream == url);
        }

        // Adiciona um novo canal ao banco de dados
        public void AddCanal(Canal canal)
        {
            if (!_db.Table<Canal>().Any(c => c.UrlStream == canal.UrlStream))
            {
                _db.Insert(canal);
                Canais.Add(canal);  // Atualiza a coleção na View
            }
        }

        // Atualiza um canal existente
        public void UpdateCanal(Canal canal)
        {
            _db.Update(canal);
            var index = Canais.IndexOf(Canais.FirstOrDefault(c => c.Id == canal.Id));
            if (index >= 0)
            {
                Canais[index] = canal; // Atualiza a coleção local
            }
        }

        // Deleta um canal
        public void DeleteCanal(Canal canal)
        {
            _db.Delete(canal);
            Canais.Remove(canal);  // Atualiza a coleção na View
        }

        public void DeleteAll()
        {
            _db.DeleteAll<Canal>();
            Canais.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db?.Dispose();
            }
        }
    }
}
