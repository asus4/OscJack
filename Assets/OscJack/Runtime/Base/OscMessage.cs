using System.Linq;
using System.Text;

namespace OscJack
{
    public struct OscMessage
    {
        public string address;
        public object[] data;

        public OscMessage(string address, params object[] data)
        {
            this.address = address;
            this.data = data;
        }

        string MakeTags()
        {
            var sb = new StringBuilder();
            sb.Append(',');
            if (data != null)
            {
                foreach (var d in data)
                {
                    var type = d.GetType();
                    if (type == typeof(int)) sb.Append('i');
                    else if (type == typeof(float)) sb.Append('f');
                    else if (type == typeof(bool)) sb.Append((bool)d ? 'T' : 'F');
                    else if (type == typeof(string)) sb.Append('s');
                    else sb.Append('?');
                }
            }
            return sb.ToString();
        }

        internal void Encode(OscPacketEncoder _encoder)
        {
            _encoder.Append(address);
            _encoder.Append(MakeTags());

            if (data == null)
            {
                return;
            }

            foreach (var d in data)
            {
                var type = d.GetType();

                if (type == typeof(int)) { _encoder.Append((int)d); }
                else if (type == typeof(float)) { _encoder.Append((float)d); }
                else if (type == typeof(string)) { _encoder.Append((string)d); }
                else if (type == typeof(bool)) { } // nothins 
            }
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(address);
            sb.Append("  ");
            sb.Append(MakeTags());
            if (data != null)
            {
                sb.Append("  ");
                foreach (var d in data)
                {
                    sb.Append(d);
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }


    }
}