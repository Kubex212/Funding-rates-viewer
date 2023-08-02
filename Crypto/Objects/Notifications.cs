using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Objects
{
    public struct Notification
    {
        //string Name, string Prop1, string Prop2, bool Predicted, float Difference, bool Sound
        public string Name { get; set; }
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public bool Predicted { get; set; }
        public float Difference { get; set; }
        public bool Sound { get; set; }

        public Notification(string name, string prop1, string prop2, bool predicted, float difference, bool sound)
        {
            Name = name;
            Prop1 = prop1;       
            Prop2 = prop2;
            Predicted = predicted;
            Difference = difference;
            Sound = sound;
        }
    }
}
