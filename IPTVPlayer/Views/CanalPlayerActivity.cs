using Com.Google.Android.Exoplayer2;
using Android.Content;
using IPTVPlayer.Models;
using Com.Google.Android.Exoplayer2.UI;
using IPTVPlayer.Data;
using IPTVPlayer.ViewModels;

namespace IPTVPlayer.Views;

[Activity(Label = "Player de Canal")]
public class CanalPlayerActivity : Activity
{
    private string canalNome;
    private string canalUrl;
    private string canalImagem;
    private int canalId;

    private PlayerView _playerView;
    private SimpleExoPlayer _exoPlayer;
    private List<Canal> playlist = [];
    int _currentVideoIndex = 0;
    private CanalViewModel canalViewModel;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_canal_player); // Layout para o player de vídeo
    
        // Recebe os dados do Intent
        canalId = Intent.GetIntExtra("canal_id", 0);
        canalNome = Intent.GetStringExtra("canal_nome");
        canalImagem = Intent.GetStringExtra("canal_foto");
        canalUrl = Intent.GetStringExtra("canal_url");

        var preferences = new SharedPreferencesHelper();
        var ultimo_canal = preferences.GetInt("ultimo_canal", 0);

        canalViewModel = new CanalViewModel();

        Canal canal = new Canal
        {
            Nome = canalNome,
            Id = canalId,
            UrlImagem = canalImagem,
            UrlStream = canalUrl
        };

        if(canal.Id > 0)
        InitializePlayer(canalUrl, canal);
        else InitializePlayer(canalUrl, null);

        SetupWidgets();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _exoPlayer.Stop();
    }
    protected override void OnResume()
    {
        base.OnResume();
        _exoPlayer.Play();
    }
    protected override void OnPause()
    {
        base.OnPause();
        _exoPlayer.Pause();
    }
    private void InitializePlayer(string url, Canal canalAtual)
    {
        _playerView = FindViewById<PlayerView>(Resource.Id.playerView);
        _exoPlayer = new SimpleExoPlayer.Builder(this).Build();
        _playerView.Player = _exoPlayer;

        List<MediaItem> play_list = [];

        var canais = canalViewModel.Canais.ToList();
        
        if (canalAtual != null)
        {
            int index = canais.FindIndex(c => c.Id == canalAtual.Id);
            if (index > 0)
                canais.Insert(0, canalAtual);
            else
            {
                canais.RemoveAt(index);
                canais.Insert(0, canalAtual);
            }
        }
        playlist.AddRange(canais);
        
        foreach (var canal in playlist)
        {
            var it = MediaItem.FromUri(Android.Net.Uri.Parse(canal.UrlStream));
            play_list.Add(it);
        }

        // Cria a fonte de mídia para o ExoPlayer
        // var mediaItem = MediaItem.FromUri(Android.Net.Uri.Parse(url));

        // Prepara o ExoPlayer com a mídia
        _exoPlayer.SetMediaItems(play_list);
        if (playlist.Count == 0)
        {
            var cnal = new Canal()
            {
                Id = 0,
                Nome = "Globo",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/70.m3u8",
                UrlImagem = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRubRFKCgI4s-oj73HK8RuOIPJj58TuZIQTnw&s"
            };

            var it = MediaItem.FromUri(Android.Net.Uri.Parse(cnal.UrlStream));
            play_list.Add(it);
            _exoPlayer.SetMediaItem(it);
        }
        _exoPlayer.Prepare();
        // Inicia a reprodução automaticamente
        _exoPlayer.Play();
    }

    private void SetupWidgets()
    {
        var btnAnterior = FindViewById<Button>(Resource.Id.btnAnterior);
        btnAnterior.Click += (sender, e) =>
        {
            //_currentVideoIndex = (_currentVideoIndex - 1 + _playlist.Count) % _playlist.Count;
            //InitializePlayer(_playlist[_currentVideoIndex]);
        };

        var btnProximo = FindViewById<Button>(Resource.Id.btnProximo);
        btnProximo.Click += (sender, e) =>
        {
            
        };
    }   
}

