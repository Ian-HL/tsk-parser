using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSKParser.Parser
{
    public class BinaryReader
    {
        private byte[] buffer;
        private int position;

        public BinaryReader(byte[] buffer)
        {
            this.buffer = buffer;
            this.position = 0;
        }

        public byte[] ReadAsBytes(int bytes, bool preventAutoForward = false)
        {
            byte[] ret = new byte[bytes];
            Array.Copy(buffer, position, ret, 0, bytes);
            if (!preventAutoForward)
            {
                position += bytes;
            }
            return ret;
        }

        public string ReadAsString(int bytes, bool preventAutoForward = false)
        {
            return System.Text.Encoding.UTF8.GetString(ReadAsBytes(bytes, preventAutoForward));
        }

        public string ReadAsHex(int bytes, bool preventAutoForward = false)
        {
            return BitConverter.ToString(ReadAsBytes(bytes, preventAutoForward)).Replace("-", "").ToLower();
        }

        public int ReadAsInt(int bytes, bool preventAutoForward = false)
        {
            return Convert.ToInt32(ReadAsHex(bytes), 16);
        }

        public string ReadAsBit(int bytesCount, bool preventAutoForward = false)
        {
            //转换成了二进制的字符串格式，
            // 2 => 10
            // 4 => 100
            return Convert.ToString(ReadAsInt(bytesCount), 2);
        }

        public void Skip(int bytesToSkip)
        {
            position += bytesToSkip;
        }

        public void Goto(int position)
        {
            this.position = position;
        }
    }
}
