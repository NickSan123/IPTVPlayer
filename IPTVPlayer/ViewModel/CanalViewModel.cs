using IPTVPlayer.Data;
using IPTVPlayer.Models;
using System.Collections.ObjectModel;

namespace IPTVPlayer.ViewModels
{
    public class CanalViewModel
    {
        public ObservableCollection<Canal> Canais { get; set; }

        public CanalViewModel()
        {
            Canais = new ObservableCollection<Canal>();
        }

        // Carrega os canais do banco de dados
        public void LoadCanais()
        {
            var db = DatabaseHelper.GetConnection();
            var canais = db.Table<Canal>().ToList();
            foreach (var canal in canais)
            {
                if (!Canais.Contains(canal))
                {
                    Canais.Add(canal);
                }
            }
        }
        public Canal LoadCanais(int id)
        {
            var db = DatabaseHelper.GetConnection();
            var canais = db.Table<Canal>().Where(x=> x.Id == id).First();
            return canais;
        }
        // Adiciona um novo canal ao banco de dados
        public void AddCanal(Canal canal)
        {
            var db = DatabaseHelper.GetConnection();
            db.Insert(canal);
            Canais.Add(canal);  // Atualiza a coleção na View
        }

        // Atualiza um canal existente
        public void UpdateCanal(Canal canal)
        {
            var db = DatabaseHelper.GetConnection();
            db.Update(canal);
        }

        // Deleta um canal
        public void DeleteCanal(Canal canal)
        {
            var db = DatabaseHelper.GetConnection();
            db.Delete(canal);
            Canais.Remove(canal);  // Atualiza a coleção na View
        }

        public void DeleteAll()
        {
            var db = DatabaseHelper.GetConnection();
            db.DeleteAll<Canal>();
        }
    }
}