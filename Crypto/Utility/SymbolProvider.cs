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
        private static List<Symbol> Symbols { get => GetSymbols(); }
        private static List<string> Names { get => GetSymbolNames(); }

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
            //string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            //using (StreamReader r = new StreamReader(Path.Combine(new string[] { projectDirectory, "Files", "names.json" })))
            try
            {
                using (StreamWriter w = new StreamWriter("names.json"))
                {
                    string json = JsonConvert.SerializeObject(Symbols);
                    w.Write(json);
                    w.Close();
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Zapisywanie do pliku nie powiodło się!!! " + ex.Message, Type.Error);
            }
            Invalidate();
            return true;
        }

        public static void AddSymbols(params Symbol[] symbol)
        {
            Symbols.AddRange(symbol);
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
            Symbols.AddRange(symbols);
            SaveToFile();
            Invalidate();
            NameTranslator.Invalidate();
        }

        public static int FixBinanceBSymbols(bool invalidate = true)
        {
            var toRemove = new List<Symbol>();

            foreach (var symbol in Symbols)
            {
                var name = symbol.Name!;
                var foundSymbol = Symbols.FirstOrDefault(s => s.Name == name.Substring(0, name.Length - 1));
                if (name.EndsWith("B") && foundSymbol != null)
                {
                    foundSymbol.BinanceBUSD = symbol.Binance;
                    toRemove.Add(symbol);
                }
            }

            var removed = Symbols.RemoveAll(s => toRemove.Contains(s));

            if(invalidate)
            {
                SaveToFile();
                Invalidate();
            }

            return removed;
        }

        public static void Invalidate()
        {
            _initialized = false;
        }

        /// <summary>
        /// Must be called after any operation that modifies the list of symbols
        /// </summary>
        /// <returns>true if no error</returns>
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

        public static List<string> ReadFile(string filename, string suffix)
        {
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line.Split()[2].Replace(suffix, ""));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"problem z plikiem {filename}: {e.Message}", Type.Error);
            }

            return lines;
        }
    }
}
