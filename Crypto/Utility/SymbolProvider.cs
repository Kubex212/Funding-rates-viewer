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

        public static void AddSymbols(params Symbol[] symbol)
        {
            _symbols.AddRange(symbol);
            SaveToFile();
            Invalidate();
            NameTranslator.Invalidate();
        }

        public static void AddSymbols(params string[] names)
        {
            var symbols = names.Select(n =>
                new Symbol()
                {
                    Name = n,
                    Bitfinex = $"t{n}F0:USTF0",
                    Phemex = $"{n}USD",
                    PhemexUsdt = $"{n}USDT",
                    Huobi = $"{n}-USDT",
                    Binance = $"{n}USDT",
                    Okx = $"{n}-USD-SWAP",
                    OkxUsd = $"{n}-USDT-SWAP",
                    ByBitInverse= $"{n}USDT",
                    ByBitLinear = $"{n}USD",
                    ByBitPerp = $"{n}PERP",
                    Ftx = "to delete"
                }
            );
            SaveToFile();
            Invalidate();
            NameTranslator.Invalidate();
        }

        public static void Invalidate()
        {
            _initialized = false;
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
                    _symbols = JsonConvert.DeserializeObject<List<Symbol>>(json)!;
                    _symbolNames = _symbols.Select(x => x.Name).ToList()!;
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
