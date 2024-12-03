using Android.Views;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTVPlayer.Models
{
    public class ParseM3U
    {
        public List<Canal> ImportChannel(string m3uFilePath)
        {
            var canais = new List<Canal>();
            try
            {
                var lines = File.ReadAllLines(m3uFilePath);
                Canal canal = null;

                foreach (var line in lines)
                {
                    if (line.StartsWith("#EXTINF"))
                    {
                        // Extrai os atributos do EXTINF
                        string[] parts = line.Split(new char[] { ',' }, 2);
                        if (parts.Length > 1)
                        {
                            // Extrai os dados como tvg-id, tvg-name, etc.
                            var attributes = parts[0].Replace("#EXTINF:-1 ", "").Split(' ');

                            string tvgId = null, tvgName = null, tvgLogo = null, groupTitle = null;

                            foreach (var attribute in attributes)
                            {
                                if (attribute.Contains("tvg-id"))
                                {
                                    tvgId = attribute.Split('=')[1].Trim('"');
                                }
                                if (attribute.Contains("tvg-name"))
                                {
                                    tvgName = attribute.Split('=')[1].Trim('"');
                                }
                                if (attribute.Contains("tvg-logo"))
                                {
                                    tvgLogo = attribute.Split('=')[1].Trim('"');
                                }
                                if (attribute.Contains("group-title"))
                                {
                                    groupTitle = attribute.Split('=')[1].Trim('"');
                                }
                            }

                            // Crie um objeto Canal para armazenar as informações
                            canal = new Canal
                            {
                                Nome = tvgName,
                                UrlStream = parts[1].Trim(),
                                UrlImagem = tvgLogo,
                                TvgId = tvgId,
                                Grupo = groupTitle
                            };
                        }
                    }
                    else if (!string.IsNullOrEmpty(line) && canal != null)
                    {
                        // Adiciona o canal à lista
                        canais.Add(canal);
                        canal = null; // Reseta para o próximo canal
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo M3U: {ex.Message}");
            }

            return canais;
        }
    }
}
