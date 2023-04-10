using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public class NotificationSettings
    {
        public double Difference { get; set; }
        public NotificationType Type { get; set; }
        public List<string> SymbolNames { get; set; } = new List<string>();
    }

    public enum NotificationType
    {
        All,
        Specified,
        AllExcept
    }
}
