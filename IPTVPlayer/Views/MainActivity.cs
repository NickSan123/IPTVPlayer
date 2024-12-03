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
                Nome = "Globo Nordeste Full HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/2437",
                UrlImagem = "http://storage4you.net:2095/logoscanais/GLOBO.png",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "Globo Nordeste HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/2436",
                UrlImagem = "http://storage4you.net:2095/logoscanais/GLOBO.png",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "Globo Nordeste",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/2437",
                UrlImagem = "http://storage4you.net:2095/logoscanais/GLOBO.png",
                Grupo = "Canais"
            });
           
            canais.Add(new Canal()
            {
                Nome = "SBT SP FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/244161",
                UrlImagem = "http://storage4you.net:2095/logoscanais/SBT.png",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "RecordTV RIO HD",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/238.m3u8",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/RECORDTV.png"
            });
            canais.Add(new Canal()
            {
                Nome = "RECORD NEWS",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/244090",
                Grupo = "Canais",
                UrlImagem = "https://upload.wikimedia.org/wikipedia/pt/thumb/b/b1/Logotipo_da_Record_News.png/220px-Logotipo_da_Record_News.png"
            });
            canais.Add(new Canal()
            {
                Nome = "REDE TV",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/367119",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/HBOMAX.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Fish TV FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/249",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/FISHTV.png"
            });
            canais.Add(new Canal()
            {
                Nome = "TV Cultura FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/149",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/TVCULTURA.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Futura FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/159",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/FUTURA.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Canal do Boi FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/364661",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/CANALDOBOI.png"
            });
            canais.Add(new Canal()
            {
                Nome = "CNN Brasil FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/474",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/CNNBRASIL.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Cinemax HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/680",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/CINEMAX.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Discovery Channel HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/112063",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/Portugal.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Disc. Turbo HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/2590",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/DISCOVERYTURBO.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Animal Planet HD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/365234",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/ANIMALPLANET.png"
            });
            canais.Add(new Canal()
            {
                Nome = "History FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/2617",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/HISTORY.png"
            });
            canais.Add(new Canal()
            {
                Nome = "Band",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/19.m3u8",
                UrlImagem = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRubRFKCgI4s-oj73HK8RuOIPJj58TuZIQTnw&s",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "Globo Minas",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/70.m3u8",
                UrlImagem = "https://static.wikia.nocookie.net/programacao/images/4/45/TV_Globo_Minas.png/revision/latest?cb=20240327235439&path-prefix=pt-br",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "Play TV",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/255.m3u8",
                UrlImagem = "https://i.pinimg.com/originals/ba/e0/7b/bae07b8a58bd9fb22459b0b38861caf8.jpg",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "PLAYPOCA FILMES",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/238.m3u8",
                UrlImagem = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSq4ceqaLIoC1XIQXGuoYs0TIDCQV47Pps6Xg&s",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "RECORD NEWS",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/97.m3u8",
                UrlImagem = "https://upload.wikimedia.org/wikipedia/pt/thumb/b/b1/Logotipo_da_Record_News.png/220px-Logotipo_da_Record_News.png",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "REDE TV",
                UrlStream = "http://tv.mapfibra.com.br:8555/live/1438/123456/102.m3u8",
                UrlImagem = "https://www.imagensempng.com.br/wp-content/uploads/2023/05/17-1.png",
                Grupo = "Canais"
            });
            canais.Add(new Canal()
            {
                Nome = "Band Sports FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/235",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/BANDSPORTS.png"
            });

            canais.Add(new Canal()
            {
                Nome = "SporTV FHD",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/118881",
                Grupo = "Canais",
                UrlImagem = "http://storage4you.net:2095/logoscanais/SPORTV.png"
            });
            canais.Add(new Canal()
            {
                Nome = "NÃO CLICAR",
                UrlStream = "http://smart.cdn23.click:80/22981577489/n613670y/260418",
                Grupo = "Canais",
                UrlImagem = "https://st2.depositphotos.com/2899123/5948/v/450/depositphotos_59480467-stock-illustration-x-red-handwritten-letter.jpg"
            });
            //canais.Add(new Canal()
            //{
            //    Nome = "",
            //    UrlStream = "",
            //    Grupo = "Canais",
            //    UrlImagem = ""
            //});
            foreach (var can in canais)
            {
                canalViewModel.AddCanal(can);
            }
        }
        private async void ReloadCanais()
        {
            canalViewModel.Canais.Clear();
            await canalViewModel.LoadCanaisAsync();
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