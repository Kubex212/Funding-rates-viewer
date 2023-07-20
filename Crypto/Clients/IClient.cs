using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Objects;

namespace Crypto.Clients
{
    public abstract class BaseClient
    {
        public abstract string Name { get; }
        public HttpClient Client { get; set; }
        public abstract Task<List<TableData>> GetTableDataAsync(List<string> symbols);
        public List<TableData> HandleUnknowns(List<string> symbols)
        {
            List<TableData> result = new List<TableData>();
            foreach(var s in symbols)
            {
                if (Utility.NameTranslator.GlobalToClientName(s, Name) == "?")
                {
                    result.Add(new TableData(s, -99f, Name, -99f));
                }
            }
            return result;
        }
        public List<string> RemoveUnknowns(List<string> symbols)
        {
            var result = new List<string>();
            foreach (var s in symbols)
            {
                if (Utility.NameTranslator.GlobalToClientName(s, Name) != "?")
                {
                    result.Add(s);
                }
            }
            return result;
        }
    }
}
