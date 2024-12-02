using Android.Content;
using IPTVPlayer.Models;
using IPTVPlayer.ViewModels;
using LibVLCSharp.Shared; // LibVLCSharp
using LibVLCSharp.Platforms.Android;
using Android.Media;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;
using IPTVPlayer.Data;
using Java.Util.Prefs;
using Java.Net;

namespace IPTVPlayer.Views
{
    [Activity(Label = "Player de Canal")]
    public class CanalPlayerActivity : Activity
    {
        private string canalNome;
        private string canalUrl;
        private string canalImagem;
        private int canalId;

        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private LibVLCSharp.Platforms.Android.VideoView _videoView; // LibVLCSharp's VideoView

        private List<Canal> playlist = new();
        private CanalViewModel canalViewModel;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_canal_player); // Certifique-se de que este layout tenha um VideoView

            // Inicialize a LibVLC
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            // Configure o VideoView do LibVLCSharp
            _videoView = FindViewById<LibVLCSharp.Platforms.Android.VideoView>(Resource.Id.videoView);
            _videoView.MediaPlayer = _mediaPlayer;

            // Recebe os dados do Intent
            canalId = Intent.GetIntExtra("canal_id", 0);
            canalNome = Intent.GetStringExtra("canal_nome");
            canalImagem = Intent.GetStringExtra("canal_foto");
            canalUrl = Intent.GetStringExtra("canal_url");

            canalViewModel = new CanalViewModel();

            // Inicializa o player com a URL fornecida
            var canalAtual = new Canal
            {
                Nome = canalNome,
                Id = canalId,
                UrlImagem = canalImagem,
                UrlStream = canalUrl
            };
            if(canalId > 0)
            {
                InitializePlayer(canalUrl, canalAtual, null);
            }
            else
            {
                var lista = new List<Canal>();
                var preferences = new SharedPreferencesHelper();
                var ultimo = preferences.GetString("ultimo_canal");
                Console.WriteLine(ultimo);
                if (!string.IsNullOrEmpty(ultimo))
                {
                    var canaile = canalViewModel.Canais.Where(x=> x.UrlStream == ultimo).First();
                    if (canaile != null)
                    {
                        PlayMedia(canaile.UrlStream);
                    }
                }
            }
            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _mediaPlayer?.Play();

            var currentMedia = _mediaPlayer.Media;
            if (currentMedia != null)
            {
                //Console.WriteLine("Título da mídia atual: " + currentMedia.Mrl);
                var preferences = new SharedPreferencesHelper();
                preferences.SaveString("ultimo_canal", currentMedia.Mrl);
            }
            else
            {
                Console.WriteLine("Nenhuma mídia carregada.");
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            _mediaPlayer?.Pause();
        }

        private void InitializePlayer(string url, Canal canalAtual, List<Canal>? canais)
        {

            if(canais == null)
            {
                canais = canalViewModel.Canais.ToList();
            }
            if (canalAtual != null)
            {
                int index = canais.FindIndex(c => c.Id == canalAtual.Id);
                if (index >= 0)
                {
                    canais.RemoveAt(index);
                }
                canais.Insert(0, canalAtual);
            }

            playlist.AddRange(canais);

            if (playlist.Count == 0)
            {
                // Se não houver canais, adiciona um padrão
                PlayMedia("http://tv.mapfibra.com.br:8555/live/1438/123456/70.m3u8");
            }
            else
            {
                PlayMedia(url);
            }
        }

        private void PlayMedia(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var media = new Media(_libVLC, url, FromType.FromLocation);
                _mediaPlayer.Media = media;
                _mediaPlayer.Play();
            }
        }
    }
}