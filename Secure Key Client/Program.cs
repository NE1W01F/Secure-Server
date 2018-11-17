using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Secure_Key_Client
{
    class Program
    {
        public static string Public_Key;
        public static string msg;
        public static string Key;
        public static string Hashed;

        static void Main()
        {
            TcpClient client = new TcpClient("127.0.0.1", 1010);
            try
            {
                while (true)
                {
                    Stream s = client.GetStream();
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    sw.AutoFlush = true;
                    Public_Key = sr.ReadLine();
                    Console.Write("Please Enter The WIFI Password:");
                    msg = SHA1(Console.ReadLine());
                    Key = msg + Public_Key;
                    Hashed = SHA1(SHA1(Key));
                    sw.WriteLine(Hashed);
                    string a = sr.ReadLine();
                    if (a == "100")
                    {
                        Console.WriteLine("Thats Right");
                        Console.ReadKey();
                        client.Close();
                        Main();
                    }
                    else
                    {
                        Console.WriteLine("Thats Wrong");
                        Console.ReadKey();
                        client.Close();
                        Main();
                    }
                }
            }
            catch (Exception) { }
        }

        public static string SHA1(string input)
        {
            byte[] hash;

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
            }
            var sb = new StringBuilder();

            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
    }
}
