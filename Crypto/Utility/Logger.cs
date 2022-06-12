using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Utility
{
    public static class Logger
    {
        static List<(string msg, Type type)> _log = new List<(string msg, Type type)> ();
        public static List<(string msg, Type type)> Logs { get { return _log; } }

        public static void Log(string message, Type type = Type.Warning)
        {
            _log.Add(new (message + "\n", type));
        }

        public static void ViewAsMessageBox()
        {
            string msg = "";
            foreach (var m in _log) msg += m.msg;
            MessageBox.Show(msg);
        }
    }

    public enum Type
    {
        Message,
        Warning,
        Error
    }
}
