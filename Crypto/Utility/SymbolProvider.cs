using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Crypto.Objects;

namespace Crypto.Utility
{
    public static class SymbolProvider
    {
        private static bool _initialized = false;
        private static List<Symbol> _symbols = new List<Symbol>();
        private static List<string> _symbolNames = new List<string>();

        public static List<Symbol> GetSymbols()
        {
            if (!_initialized)
                if (!Initialize()) throw new InvalidOperationException();
            return _symbols;
        }
        public static List<string> GetSymbolNames()
        {
            if (!_initialized)
                if (!Initialize()) throw new InvalidOperationException();
            return _symbolNames;
        }

        public static bool SaveToFile()
        {
            if (!_initialized)
                if (!Initialize()) throw new InvalidOperationException();
            //string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            //using (StreamReader r = new StreamReader(Path.Combine(new string[] { projectDirectory, "Files", "names.json" })))
            try
            {
                using (StreamWriter w = new StreamWriter("names.json"))
                {
                    string json = JsonConvert.SerializeObject(_symbols);
                    w.Write(json);
                    w.Close();
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Zapisywanie do pliku nie powiodło się!!! " + ex.Message, Type.Error);
            }
            _initialized = false;
            return true;
        }

        private static bool Initialize()
        {
            try
            {
                //string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
                //using (StreamReader r = new StreamReader(Path.Combine(new string[] { projectDirectory, "Files", "names.json" })))
                using (StreamReader r = new StreamReader("names.json"))
                {
                    string json = r.ReadToEnd();
                    _symbols = JsonConvert.DeserializeObject<List<Symbol>>(json);
                    foreach (var symbol in _symbols) _symbolNames.Add(symbol.Name);
                }
            }
            catch (FileNotFoundException ex)
            {
                Logger.Log($"Nie ma pliku {ex.FileName}! {ex.Message}", Type.Error);
                return false;
            }
            return _initialized = true;
        }
    }
}
