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
using Android.Views;

namespace IPTVPlayer.Views
{
    [Activity(Label = "Player de Canal"), ]
    public class CanalPlayerActivity : Activity, GestureDetector.IOnGestureListener
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
        private GestureDetector _gestureDetector;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Window.AddFlags(WindowManagerFlags.Fullscreen);

            // Oculta a barra de navegação (se necessário)
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                SystemUiFlags.ImmersiveSticky
                | SystemUiFlags.HideNavigation
                | SystemUiFlags.Fullscreen);

            SetContentView(Resource.Layout.activity_canal_player); // Certifique-se de que este layout tenha um VideoView

            _gestureDetector = new GestureDetector(this);

            // Exibe o botão de voltar no ActionBar
            ActionBar?.SetDisplayHomeAsUpEnabled(true);

            // Inicialize a LibVLC
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            // Configure o VideoView do LibVLCSharp
            _videoView = FindViewById<LibVLCSharp.Platforms.Android.VideoView>(Resource.Id.videoView);
            _videoView.MediaPlayer = _mediaPlayer;
            _mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;

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
               await canalViewModel.LoadCanaisAsync();

                var lista = new List<Canal>();
                var preferences = new SharedPreferencesHelper();
                var ultimo = preferences.GetString("ultimo_canal");
                Console.WriteLine(ultimo);
                if (!string.IsNullOrEmpty(ultimo))
                {
                    //await canalViewModel.LoadCanaisAsync();
                    var canaile = canalViewModel.LoadCanais(ultimo);
                    //var canaile = canalViewModel.Canais.Where(x=> x.UrlStream == ultimo).FirstOrDefault();
                    if (canaile != null)
                    {
                        PlayMedia(canaile.UrlStream);
                    }
                }
            }
            
        }
        protected override void OnStart()
        {
            base.OnStart();
            HideSystemUI();
        }
        private void MediaPlayer_EncounteredError(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
                Toast.MakeText(this, "Falha ao carregar a mídia.", ToastLength.Long).Show()
            );
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
                var preferences = new SharedPreferencesHelper();
                preferences.SaveString("ultimo_canal", currentMedia.Mrl);

                var canaile = canalViewModel.LoadCanais(currentMedia.Mrl);
                //var canaile = canalViewModel.Canais.Where(x=> x.UrlStream == ultimo).FirstOrDefault();
                if (canaile != null)
                {
                    ActionBar.Title = canaile.Nome;
                }
            }
            else
            {
                Console.WriteLine("Nenhuma mídia carregada.");
            }
        }


        private void InitializePlayer(string url, Canal canalAtual, List<Canal>? canais)
        {
            if (canais == null)
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

            playlist.Clear();
            playlist.AddRange(canais);

            if (!string.IsNullOrEmpty(url))
            {
                PlayMedia(url);
            }
            else if (playlist.Count > 0)
            {
                PlayMedia(playlist.First().UrlStream);
            }
            else
            {
                // Fallback para uma URL padrão
                PlayMedia("http://tv.mapfibra.com.br:8555/live/1438/123456/70.m3u8");
            }
        }


        private void PlayMedia(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("URL vazia ou inválida.");
                return;
            }

            var media = new Media(_libVLC, url, FromType.FromLocation);
            _mediaPlayer.Media = media;

            _mediaPlayer.EncounteredError += (sender, args) =>
            {
                Console.WriteLine($"Erro ao reproduzir mídia: {url}");
                // Aqui, você pode implementar fallback ou notificar o usuário
            };

            _mediaPlayer.Play();
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return base.OnTouchEvent(e);
        }

        public bool OnDown(MotionEvent e)
        {
            ShowActionBar();
            return true;
        }

        public void ShowActionBar()
        {
            // Mostra a ActionBar temporariamente
            ActionBar?.Show();

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;

            // Oculta novamente após 3 segundos
            Task.Delay(3000).ContinueWith(t =>
            {
                RunOnUiThread(() =>
                {
                    HideSystemUI();
                });
            });
        }

        public void HideSystemUI()
        {
            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                SystemUiFlags.ImmersiveSticky |
                SystemUiFlags.HideNavigation |
                SystemUiFlags.Fullscreen);

            ActionBar?.Hide();
        }

        //public override bool OnSupportNavigateUp()
        //{
        //    // Botão de voltar
        //    Finish();
        //    return true;
        //}

        // Implementação de outros métodos do GestureDetector.IOnGestureListener

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }
            return base.OnOptionsItemSelected(item);

        }
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) => false;
        public void OnLongPress(MotionEvent e) { }
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) => false;
        public void OnShowPress(MotionEvent e) { }
        public bool OnSingleTapUp(MotionEvent e) => false;
    }
}