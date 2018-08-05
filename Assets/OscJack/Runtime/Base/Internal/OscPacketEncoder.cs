// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using System;

namespace OscJack
{
    internal sealed class OscPacketEncoder
    {
        public Byte[] Buffer { get { return _buffer; } }
        public int Length { get { return _length; } }

        public void Clear()
        {
            _length = 0;
        }

        public void Append(string data)
        {
            var len = data.Length;
            for (var i = 0; i < len; i++)
                _buffer[_length++] = (Byte)data[i];

            var len4 = OscDataTypes.Align4(len + 1);
            for (var i = len; i < len4; i++)
                _buffer[_length++] = 0;
        }

        public void Append(int data)
        {
            _buffer[_length++] = (Byte)(data >> 24);
            _buffer[_length++] = (Byte)(data >> 16);
            _buffer[_length++] = (Byte)(data >> 8);
            _buffer[_length++] = (Byte)(data);
        }

        public void Append(float data)
        {
            _tempFloat[0] = data;
            System.Buffer.BlockCopy(_tempFloat, 0, _tempByte, 0, 4);
            _buffer[_length++] = _tempByte[3];
            _buffer[_length++] = _tempByte[2];
            _buffer[_length++] = _tempByte[1];
            _buffer[_length++] = _tempByte[0];
        }

        public void Append(byte[] data, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _buffer[_length++] = data[i];
            }
        }

        public void Append(DateTime datetime)
        {
            // time tag
            var span = datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            decimal milliseconds = Convert.ToDecimal(span.TotalMilliseconds);

            // https://stackoverflow.com/questions/16763300/converting-between-ntp-and-c-sharp-datetime
            var ntpData = new byte[8];
            decimal intpart = milliseconds / 1000;
            decimal fractpart = ((milliseconds % 1000) * 0x100000000L) / 1000m;

            //
            var temp = intpart;
            for (var i = 3; i >= 0; i--)
            {
                ntpData[i] = (byte)(temp % 256);
                temp = temp / 256;
            }

            // 
            temp = fractpart;
            for (var i = 7; i >= 4; i--)
            {
                ntpData[i] = (byte)(temp % 256);
                temp = temp / 256;
            }

            //
            for (int i = 0; i < 8; i++)
            {
                _buffer[_length++] = ntpData[i];
            }
        }

        Byte[] _buffer = new Byte[4096];
        int _length;

        // Used to change byte order
        static float[] _tempFloat = new float[1];
        static Byte[] _tempByte = new Byte[4];
    }
}
