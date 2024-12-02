using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTVPlayer.Data
{
    public class SharedPreferencesHelper
    {
        private readonly ISharedPreferences _sharedPreferences;
        private readonly ISharedPreferencesEditor _editor;

        // Construtor: inicializa o SharedPreferences e o editor
        public SharedPreferencesHelper()
        {
            _sharedPreferences = Android.App.Application.Context.GetSharedPreferences("MyAppPreferences", FileCreationMode.Private);
            _editor = _sharedPreferences.Edit();
        }

        // Salvar um dado no SharedPreferences
        public void SaveString(string key, string value)
        {
            _editor.PutString(key, value); // Salva uma string
            _editor.Apply(); // Confirma a gravação de forma assíncrona
        }
        public void SaveBool(string key, bool value)
        {
            _editor.PutBoolean(key, value); // Salva uma string
            _editor.Apply(); // Confirma a gravação de forma assíncrona
        }

        // Recuperar um dado do SharedPreferences
        public string GetString(string key, string defaultValue = "") => _sharedPreferences.GetString(key, defaultValue);
        public int GetInt(string key, int defaultValue = 0)
        {
            return _sharedPreferences.GetInt(key, defaultValue);
        }
        public bool GetBool(string key, bool defaultValue = false)
        {
            return _sharedPreferences.GetBoolean(key, defaultValue);
        }
    }
}
