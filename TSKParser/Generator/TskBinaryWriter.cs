using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TSKParser.Generator
{
    public class TskBinaryWriter
    {
        BinaryWriter bw;
        public bool OpenLog = false;
        public TskBinaryWriter(Stream stream)
        {
            bw = new BinaryWriter(stream);
        }

        public void Close()
        {
            bw?.Close();
        }

        public void WriteAsBytes(byte[] bytes)
        {

            if (OpenLog)
            {
                Console.WriteLine(ByteToHexString(bytes));
            }

            bw.Write(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intValue">int值</param>
        /// <param name="bytesNumber">字节数，如果转换出的字符数不够。则需要使用00进行补充</param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteAsInt(int intValue, int bytesNumber)
        {
            if (bytesNumber == 1)
            {
                WriteAsBytes(new byte[] { (byte)intValue });
            }
            else if (bytesNumber == 2)
            {
                byte[] bytes = BitConverter.GetBytes((Int16)intValue);
                bytes = bytes.Reverse().ToArray();
                WriteAsBytes(bytes);
            }
            else if (bytesNumber == 4)
            {
                byte[] bytes = BitConverter.GetBytes(intValue);
                bytes = bytes.Reverse().ToArray();
                WriteAsBytes(bytes);
            }
        }


        /// <summary>
        /// 将byte[] 转换为字符串格式
        /// aa 00   => "aa 00"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] data)
        {
            string strTemp = "";
            for (int i = 0; i < data.Length; i++)
            {
                string a = Convert.ToString(data[i], 16).PadLeft(2, '0');
                strTemp = strTemp + a;
            }
            return strTemp;
        }


        /// <summary>
        /// 将二进制字符串转换为16进行的bytes，例如：
        /// 1101 0001  => d1
        /// 0010 0001  => 21
        /// </summary>
        /// <returns></returns>
        public static byte[] BinaryStringToBytes(string values)
        {

            if (values.StartsWith("0x"))
            {
                values = values.Substring(2, values.Length - 2);
            }
            if (values.Length % 8 != 0)
            {
                //在字符串前面补0
                values = values.PadLeft(values.Length + (8 - values.Length % 8), '0');
            }

            string strTemp = "";
            byte[] b = new byte[values.Length / 8];
            for (int i = 0; i < values.Length / 8; i++)
            {
                strTemp = values.Substring(i * 8, 8);
                b[i] = Convert.ToByte(strTemp, 2);
            }
            return b;
        }

        /// <summary>
        /// 将字符串 转换为16进制byte
        /// "aa 00"   => aa 00
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hs)
        {

            if (hs.StartsWith("0x"))
            {
                hs = hs.Substring(2, hs.Length - 2);
            }

            string strTemp = "";
            byte[] b = new byte[hs.Length / 2];
            for (int i = 0; i < hs.Length / 2; i++)
            {
                strTemp = hs.Substring(i * 2, 2);
                b[i] = Convert.ToByte(strTemp, 16);
            }
            //按照指定编码将字节数组变为字符串
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue">数据值</param>
        /// <param name="length">最大长度，不够要补空格。（不会超过，但是超过就截取吧）</param>
        /// <exception cref="NotImplementedException"></exception>
        public void WriteAsString(string stringValue, int length)
        {
            if (stringValue.Length > length)
            {
                stringValue = stringValue.Substring(0, length);
            }
            else
            {
                stringValue = stringValue.PadRight(length);
            }
            byte[] bytes = Encoding.ASCII.GetBytes(stringValue);
            WriteAsBytes(bytes);
        }
    }
}
