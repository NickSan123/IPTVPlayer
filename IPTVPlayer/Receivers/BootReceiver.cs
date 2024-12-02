using Android.Content;
using IPTVPlayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;

namespace IPTVPlayer.Receivers
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionBootCompleted)
            {
                // Inicia o aplicativo após o boot
                var startIntent = new Intent(context, typeof(MainActivity));
                startIntent.AddFlags(ActivityFlags.NewTask); // Necessário para iniciar uma Activity fora de um contexto existente
                context.StartActivity(startIntent);
            }
        }
    }
}
