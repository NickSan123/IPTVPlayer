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

            // Criar e configurar o Adapter
            canalAdapter = new CanalAdapter(this, canalViewModel.Canais.ToList(), OnCanalLongClick);

            // Setar o adapter no RecyclerView
            recyclerView.SetAdapter(canalAdapter);

            btAdd.Click += (sender, args) =>
            {
                StartActivity(typeof(ChannelAddActivity));
            };
        }
            
        

        // Método para carregar os canais
        private void LoadCanais()
        {
            canalAdapter.AtualizarLista(canalViewModel.Canais.ToList());
        }

        private void ReloadCanais()
        {
            canalViewModel.Canais.Clear();
            canalViewModel.LoadCanais();
            canalAdapter.AtualizarLista(canalViewModel.Canais.ToList());
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

            StartActivity(intent);
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