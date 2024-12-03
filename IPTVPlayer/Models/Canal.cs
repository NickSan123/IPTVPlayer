using SQLite;

namespace IPTVPlayer.Models
{
    public class Canal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string UrlStream { get; set; }
        public string? UrlImagem { get; set; }
        public string? TvgId { get; set; }
        public string? Grupo { get; set; }
    }
}
