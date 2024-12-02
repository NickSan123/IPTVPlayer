using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using IPTVPlayer.Models;
using IPTVPlayer.ViewModels;
using IPTVPlayer.Views;

namespace IPTVPlayer.Adapter
{
    public class CanalAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        private List<Canal> _canais;
        private readonly Action<Canal> _onCanalClicked;

        // Construtor
        public CanalAdapter(Context context, List<Canal> canais, Action<Canal> onCanalClicked)
        {
            _context = context;
            _canais = canais;
            _onCanalClicked = onCanalClicked;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(_context).Inflate(Resource.Layout.channel_item, parent, false);
            var viewHolder = new CanalViewHolder(itemView);
            itemView.Click += (sender, e) =>
            {
                var canal = _canais[viewHolder.BindingAdapterPosition];



                OpenCanalPlayerActivity(canal); // Abrindo a activity de player com o canal
            };
            // Configurando clique longo
            itemView.LongClick += (sender, e) =>
            {
                if (viewHolder.BindingAdapterPosition != RecyclerView.NoPosition)
                {
                    var canal = _canais[viewHolder.BindingAdapterPosition];
                    _onCanalClicked(canal);
                }
            };

            return viewHolder;
        }
        private void OpenCanalPlayerActivity(Canal canal)
        {
            var intent = new Intent(_context, typeof(CanalPlayerActivity)); // CanalPlayerActivity será a Activity de player

            // Passa o canal selecionado para a tela de edição
            intent.PutExtra("canal_id", canal.Id);
            intent.PutExtra("canal_nome", canal.Nome);
            intent.PutExtra("canal_foto", canal.UrlImagem);
            intent.PutExtra("canal_url", canal.UrlStream);

            _context.StartActivity(intent);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var canal = _canais[position];
            var canalHolder = (CanalViewHolder)holder;

            // Nome do canal
            canalHolder.NomeTextView.Text = canal.Nome;

            // Usando Glide para carregar imagem
            Glide.With(_context)
                .Load(canal.UrlImagem)
                .Placeholder(Android.Resource.Drawable.IcMenuCamera) // Placeholder enquanto carrega
                .Error(Android.Resource.Drawable.StatNotifyError)            // Imagem de erro caso falhe
                .Into(canalHolder.FotoImageView);
        }

        public override int ItemCount => _canais.Count;

        public void AtualizarLista(List<Canal> canais)
        {
            _canais = canais;
            NotifyDataSetChanged();
        }
    }

    public class CanalViewHolder : RecyclerView.ViewHolder
    {
        public TextView NomeTextView { get; private set; }
        public ImageView FotoImageView { get; private set; }

        public CanalViewHolder(View itemView) : base(itemView)
        {
            NomeTextView = itemView.FindViewById<TextView>(Resource.Id.channelName);
            FotoImageView = itemView.FindViewById<ImageView>(Resource.Id.channelImage);
        }
    }
}