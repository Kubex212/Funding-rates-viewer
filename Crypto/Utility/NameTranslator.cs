using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Crypto.Objects;

namespace Crypto.Utility
{
    public static class NameTranslator
    {
        private static bool _initialized = false;
        private static List<Symbol> _symbols = new List<Symbol>();
        private static dynamic _arrayOfObjects;
        
        private static bool Initialize()
        {
            try
            {
                using (StreamReader r = new StreamReader("names.json"))
                {
                    string json = r.ReadToEnd();
                    _symbols = JsonConvert.DeserializeObject<List<Symbol>>(json);
                    _arrayOfObjects = JsonConvert.DeserializeObject(json);
                }
            }
            catch (FileNotFoundException ex)
            {
                Logger.Log($"Nie ma pliku {ex.FileName}! {ex.Message}", Type.Error);
                return false;
            }
            return _initialized = true;
        }

        /// <summary>
        /// Translates global name to client name.
        /// </summary>
        /// <param name="symbol">Symbol to translate</param>
        /// <param name="clientName">Name of the market</param>
        /// <returns>client name of the provided symbol</returns>
        public static string GlobalToClientName(string symbol, string clientName)
        {
            if (!_initialized)
                if (!Initialize()) throw new InvalidOperationException();
            foreach (var s in _arrayOfObjects)
            {
                if (s["Name"] == symbol) return s[clientName];
            }
            Logger.Log($"W pliku .json symbol {symbol} nie istnieje!", Utility.Type.Warning);
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Translates list of global names to a list of client names.
        /// </summary>
        /// <param name="symbol">List of symbols to translate</param>
        /// <param name="clientName">Name of the market</param>
        /// <returns>list of client names of the provided symbols</returns>
        public static List<string> GlobalToClientNames(List<string> symbols, string clientName)
        {
            if (!_initialized)
                if (!Initialize()) throw new InvalidOperationException();
            var result = new List<string>();
            foreach(var symbol in symbols)
            {
                bool found = false;
                foreach (var s in _arrayOfObjects)
                {
                    if (s["Name"] == symbol)
                    {
                        result.Add(s[clientName].ToString());
                        found = true;
                        break;
                    }
                }
                if(!found)
                {
                    Logger.Log($"W pliku .json symbol {symbol} nie istnieje!", Utility.Type.Warning);
                    throw new InvalidOperationException();
                }
            }
            return result;
        }

        /// <summary>
        /// Translates client name to global name.
        /// </summary>
        /// <param name="symbol">Symbol to translate</param>
        /// <param name="clientName">Name of the market</param>
        /// <returns>global name of the provided symbol</returns>
        public static string ClientToGlobalName(string symbol, string clientName)
        {
            if (!_initialized) 
                if(!Initialize()) throw new InvalidOperationException();
            foreach (var s in _arrayOfObjects)
            {
                if (s[clientName] == symbol) return s.Name;
            }
            throw new InvalidOperationException($"Giełda {clientName} zwróciła symbol {symbol}, jednak w pliku .json nie ma jego odpowiednika. Zostanie on pominięty w tabeli.");
        }
    }
}
