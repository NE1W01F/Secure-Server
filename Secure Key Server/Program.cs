using System;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Secure_Key_Server
{
    class Program
    {
        public static string Public_Key;
        public static string msg;
        public static string Key;
        public static string Hashed;
        public static Socket soc;


        static void Main(string[] args)
        {
            Console.Title = "WIFI Router - WPA3 Encrypted";
            TcpListener listener = new TcpListener(IPAddress.Any, 1010);
            listener.Start();
            soc = listener.AcceptSocket();
            while (true)
            {
                try
                {

                    Stream s = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    sw.AutoFlush = true;

                    // Generate Keys
                    msg = SHA1("johnboy1984"); // Password
                    Public_Key = SHA1(GenerateKey()); // Generates a Public Key
                    Key = msg + Public_Key; // Salts the Password with Public Key
                    Hashed = SHA1(SHA1(Key)); // Salts the Key two times

                    // Login Process
                    EndPoint x = soc.RemoteEndPoint;
                    Console.WriteLine("[+] " + x.ToString() + " Connected To Server");
                    sw.WriteLine(Public_Key);
                    Console.WriteLine("[+] Public Key Sent To Client " + "(" + Public_Key + ")");
                    Thread.Sleep(1000);
                    string input = sr.ReadLine();
                    Console.WriteLine("[+] Received Password Form Client " + "(" + input + ")");
                    Thread.Sleep(1000);
                    Console.WriteLine("[+] Checking Password (" + Hashed + ")");
                    Thread.Sleep(1000);
                    if (input == Hashed)
                    {
                        sw.WriteLine("100");
                        Console.WriteLine("[+] Password Matched");
                        Console.WriteLine();
                    }
                    else
                    {
                        sw.WriteLine("503");
                        Console.WriteLine("*** Wrong Password ***");
                        Console.WriteLine();
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                }
            }
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

        private static string GenerateKey()
        {
            string data = "qwertyuiop1234567890asdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
            Random ran = new Random();
            int L = data.Length;
            int a = 0;
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            string KEYGen = string.Empty;
            while (a < 20)
            {
                KEYGen += data[ran.Next(0, L)];
                a++;
            }
            return KEYGen;
        }
    }
}
