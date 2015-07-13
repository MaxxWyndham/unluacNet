using System;

namespace unluacNet
{
    public class ByteBuffer
    {
        byte[] _bytes;
        int index = 0;
        int length = 0;
        public bool HasNextValue
        {
            get { return index < length; }
        }

        public ByteBuffer(byte[] bytes)
        {
            _bytes = bytes;
            length = _bytes.Length;
        }

        public byte GetByte()
        {
            if (!HasNextValue) throw new Exception("ByteBuffer::GetByte error, readed end of buffer!");
            byte output = _bytes[index];
            index++;
            return output;
        }

        public short GetShort()
        {
            byte[] byteArray = new byte[2];
            byteArray[0] = GetByte();
            byteArray[1] = GetByte();

            return BitConverter.ToInt16(byteArray, 0);
        }
        public int GetInt()
        {
            byte[] byteArray = new byte[4];
            byteArray[0] = GetByte();
            byteArray[1] = GetByte();
            byteArray[2] = GetByte();
            byteArray[3] = GetByte();

            return BitConverter.ToInt32(byteArray, 0);
        }
        public long GetLong()
        {
            byte[] byteArray = new byte[8];
            byteArray[0] = GetByte();
            byteArray[1] = GetByte();
            byteArray[2] = GetByte();
            byteArray[3] = GetByte();
            byteArray[4] = GetByte();
            byteArray[5] = GetByte();
            byteArray[6] = GetByte();
            byteArray[7] = GetByte();

            return BitConverter.ToInt64(byteArray, 0);
        }
        public float GetFloat()
        {
            byte[] byteArray = new byte[4];
            byteArray[0] = GetByte();
            byteArray[1] = GetByte();
            byteArray[2] = GetByte();
            byteArray[3] = GetByte();

            return BitConverter.ToSingle(byteArray, 0);
        }
        public double GetDouble()
        {
            byte[] byteArray = new byte[8];
            byteArray[0] = GetByte();
            byteArray[1] = GetByte();
            byteArray[2] = GetByte();
            byteArray[3] = GetByte();
            byteArray[4] = GetByte();
            byteArray[5] = GetByte();
            byteArray[6] = GetByte();
            byteArray[7] = GetByte();

            return BitConverter.ToDouble(byteArray, 0);
        }
    }
}
