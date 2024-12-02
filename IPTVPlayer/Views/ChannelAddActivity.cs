using Android.Views;
using IPTVPlayer.Models;
using IPTVPlayer.ViewModels;

namespace IPTVPlayer.Views;

[Activity(Label = "ChannelAddActivity"/*, MainLauncher = true*/)]
public class ChannelAddActivity : Activity
{
    private CanalViewModel canalViewModel;
    private EditText nomeCanalEditText;
    private EditText urlStreamEditText;
    private EditText urlImagemEditText;
    private Button btSalvar;
    private int _canalId;
    string canalNome ;
    string canalFoto;
    string canalUrl;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_channel_add);
        canalViewModel = new CanalViewModel();

        nomeCanalEditText = FindViewById<EditText>(Resource.Id.nome_canal_edittext);
        urlStreamEditText = FindViewById<EditText>(Resource.Id.url_stream_edittext);
        urlImagemEditText = FindViewById<EditText>(Resource.Id.url_imagem_edittext);
        btSalvar = FindViewById<Button>(Resource.Id.bt_salvar);


        // Obter os dados do canal a partir da Intent
        _canalId = Intent.GetIntExtra("canal_id", -1);
        var canalNome = Intent.GetStringExtra("canal_nome");
        var canalFoto = Intent.GetStringExtra("canal_foto");
        var canalUrl = Intent.GetStringExtra("canal_url");

        if (_canalId > 0)
        {
            ActionBar.Title = "Atualizar Canal: " + _canalId;
            nomeCanalEditText.Text = canalNome;
            urlStreamEditText.Text = canalUrl;
            urlImagemEditText.Text = canalFoto;
        }
        else
            ActionBar.Title = "Novo Canal";



        btSalvar.Click += (sender, e) =>
        {
            OnSaveButtonClick(sender, e);
        };
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        
    }
    private void OnSaveButtonClick(object sender, System.EventArgs e)
    {
        var nomeCanal = nomeCanalEditText.Text;
        var urlCanal = urlStreamEditText.Text;
        var fotoUrl = urlImagemEditText.Text;

        if (string.IsNullOrEmpty(nomeCanal) || string.IsNullOrEmpty(fotoUrl) || string.IsNullOrEmpty(urlCanal))
        {
            Toast.MakeText(this, "Preencha todos os campos.", ToastLength.Short).Show();
            return;
        }

        // Atualizar o canal no banco de dados
        var canal = new Canal
        {
            Id = _canalId,
            Nome = nomeCanal,
            UrlImagem = fotoUrl,
            UrlStream = urlCanal
        };
        if (_canalId > 0)
        {
            canalViewModel.UpdateCanal(canal);
            Toast.MakeText(this, "Canal atualizado com sucesso!", ToastLength.Short).Show();
        }
        else
        {
            canalViewModel.AddCanal(canal);
            // Exibir uma mensagem de sucesso
            Toast.MakeText(this, "Canal criado com sucesso!", ToastLength.Short).Show();
        }
            

        // Finalizar a Activity e voltar para a tela anterior
        Finish();
    }
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.menu_channel_edit, menu); // Inflando o menu
        return base.OnCreateOptionsMenu(menu);
    }
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        if (item.ItemId == Android.Resource.Id.Home)
        {
            Finish();
            return true;
        }
        if (item.ItemId == Resource.Id.action_menu_save)
        {
            OnSaveButtonClick(null, null);
            Finish();
            return true;
        }
        if (item.ItemId == Resource.Id.action_menu_delete)
        {
            
            canalViewModel.DeleteCanal(new Canal
            {
                Id = _canalId,
                Nome = canalNome,
                UrlImagem = canalFoto,
                UrlStream = canalUrl
            });
            Toast.MakeText(this, "Canal excluido com sucesso!", ToastLength.Short).Show();
            Finish();
            return true;
        }

        return base.OnOptionsItemSelected(item);
    }
}