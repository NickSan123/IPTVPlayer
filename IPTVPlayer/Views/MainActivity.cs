using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using IPTVPlayer.ViewModels;
using IPTVPlayer.Models;
using IPTVPlayer.Views;
using IPTVPlayer.Adapter;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using IPTVPlayer.Data;
using Android.Content;

namespace IPTVPlayer.Views
{
    [Activity(Label = "IPTVPlayer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private RecyclerView recyclerView;
        private Button btAdd;
        private CanalAdapter canalAdapter;
        private CanalViewModel canalViewModel;

        protected override void OnResume()
        {
            base.OnResume();
            ReloadCanais();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Inicializar o RecyclerView
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            btAdd = FindViewById<Button>(Resource.Id.btAdd);

            // Criar o ViewModel
            canalViewModel = new CanalViewModel();

            // Configurar o RecyclerView
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            try
            {
                var preferences = new SharedPreferencesHelper();
                var ultimo_canal = preferences.GetString("ultimo_canal");

                var primeira = preferences.GetBool("primeira_configuracao", true);

                if (primeira)
                {
                    var qtude_canal = canalViewModel.Canais.Count();
                    if (qtude_canal > 0)
                    {
                        canalViewModel.DeleteAll();

                        InsertPrimeirosCanais();

                    }
                    else
                    {
                        InsertPrimeirosCanais();
                    }
                    preferences.SaveBool("primeira_configuracao", false);
                }

                if (string.IsNullOrEmpty(ultimo_canal))
                {
                    var canal = canalViewModel.LoadCanais(ultimo_canal);

                    if (canal != null)
                    {
                        OpenCanalPlayerActivity(canal);
                    }

                }
                else OpenCanalPlayerActivity(null);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Erro: " + ex.Message, ToastLength.Short).Show();
            }


            // Criar e configurar o Adapter
            canalAdapter = new CanalAdapter(this, canalViewModel.Canais.ToList(), OnCanalLongClick);

            // Setar o adapter no RecyclerView
            recyclerView.SetAdapter(canalAdapter);

            btAdd.Click += (sender, args) =>
            {
                StartActivity(typeof(ChannelAddActivity));
            };
        }
        protected override void OnStart()
        {
            base.OnStart();
        }
        private void InsertPrimeirosCanais()
        {
            var canais = new List<Canal>();
            canais.Add(new Canal()
            {
                Nome = "Band",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/19.m3u8",
                UrlImagem = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRubRFKCgI4s-oj73HK8RuOIPJj58TuZIQTnw&s"
            });
            canais.Add(new Canal()
            {
                Nome = "Globo Minas",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/70.m3u8",
                UrlImagem = "https://static.wikia.nocookie.net/programacao/images/4/45/TV_Globo_Minas.png/revision/latest?cb=20240327235439&path-prefix=pt-br"
            });
            canais.Add(new Canal()
            {
                Nome = "Play TV",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/255.m3u8",
                UrlImagem = "https://i.pinimg.com/originals/ba/e0/7b/bae07b8a58bd9fb22459b0b38861caf8.jpg"
            });
            canais.Add(new Canal()
            {
                Nome = "PLAYPOCA FILMES",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/238.m3u8",
                UrlImagem = ""
            });
            canais.Add(new Canal()
            {
                Nome = "RECORD NEWS",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/97.m3u8",
                UrlImagem = "https://upload.wikimedia.org/wikipedia/pt/thumb/b/b1/Logotipo_da_Record_News.png/220px-Logotipo_da_Record_News.png"
            });
            canais.Add(new Canal()
            {
                Nome = "REDE TV",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/102.m3u8",
                UrlImagem = "https://upload.wikimedia.org/wikipedia/pt/8/89/Logotipo_da_RedeTV%21.png"
            });

            foreach (var can in canais)
            {
                canalViewModel.AddCanal(can);
            }
        }
        private void ReloadCanais()
        {
            canalViewModel.Canais.Clear();
            canalViewModel.LoadCanais();
            canalAdapter.AtualizarLista(canalViewModel.Canais.ToList());
        }
        private void OpenCanalPlayerActivity(Canal canal)
        {
            var intent = new Android.Content.Intent(this, typeof(CanalPlayerActivity));

            if (canal == null)
                StartActivity(intent);
            else
            {
                // Passa o canal selecionado para a tela de edição
                intent.PutExtra("canal_id", canal.Id);
                intent.PutExtra("canal_nome", canal.Nome);
                intent.PutExtra("canal_foto", canal.UrlImagem);
                intent.PutExtra("canal_url", canal.UrlStream);
            }

        }
        // Método chamado ao segurar o clique em um item de canal
        private void OnCanalLongClick(Canal canal)
        {
            // Chama a tela de edição do canal
            var intent = new Android.Content.Intent(this, typeof(ChannelAddActivity));

            
            // Passa o canal selecionado para a tela de edição
            intent.PutExtra("canal_id", canal.Id);
            intent.PutExtra("canal_nome", canal.Nome);
            intent.PutExtra("canal_foto", canal.UrlImagem);
            intent.PutExtra("canal_url", canal.UrlStream);
            
           
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu); // Inflando o menu
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add_channel:
                    // Chama a tela de adicionar canal
                    StartActivity(typeof(ChannelAddActivity));
                    return true;
                
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}