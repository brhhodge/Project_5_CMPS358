// Brian Hodge
// C00170400
// CMPS 358 
// Project #5

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using static System.Console;
using System.Diagnostics;
using System.Text;

namespace SimpleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write ("Enter Key: ");
            string key = Console.ReadLine ();
            Server (key);

        }

        static void Server (string key)
        {
            TcpListener listen = new TcpListener(IPAddress.Any, 8081);
            listen.Start();
            Console.WriteLine($"Listening on 8081 ...");

            using (TcpClient clientConnect = listen.AcceptTcpClient())
            using (NetworkStream theClient = clientConnect.GetStream())

            {
                BinaryWriter bw = new BinaryWriter(theClient);
                BinaryReader br = new BinaryReader(theClient);

                string phrase = "";

                while (true)
                {
                    try
                    {
                        Console.WriteLine("Listening...");
                        String rcv = br.ReadString();
                        string decryptRcv = Decrypt(rcv, key);
                        Console.WriteLine($"Received-> {decryptRcv}");

                        Console.Write("Enter phrase: ");
                        phrase = Console.ReadLine();
                        string cryptotext = Encrypt(phrase, key);
                        Console.WriteLine ($"Sending-> {phrase}");
                        bw.Write(cryptotext);
                        bw.Flush();

                    }
                    catch (CryptographicException ex)
                    {
                        WriteLine("{0}\n More details: {1}",
                        arg0: "You entered the wrong password!",
                        arg1: ex.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Session Terminated");
                        break;
                    }
                }
            }
        }

        private static readonly byte[] salt = Encoding.Unicode.GetBytes("7BANANAS");
        private static readonly int iterations = 50_000;

        public static string Encrypt(string plainText, string password)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);

            var aes = Aes.Create(); // abstract class factory method

            var stopwatch = Stopwatch.StartNew();

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);

            aes.Key = pbkdf2.GetBytes(32); // set a 256-bit key 
            aes.IV = pbkdf2.GetBytes(16); // set a 128-bit IV 

            WriteLine("{0:N0} milliseconds to generate Key and IV using {1:N0} iterations.",
                arg0: stopwatch.ElapsedMilliseconds,
                arg1: iterations);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                cs.Write(plainBytes, 0, plainBytes.Length);
                }
                encryptedBytes = ms.ToArray();
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cryptoText, string password)
        {
            byte[] plainBytes;
            byte[] cryptoBytes = Convert.FromBase64String(cryptoText);

            var aes = Aes.Create();

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);

            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                cs.Write(cryptoBytes, 0, cryptoBytes.Length);
                }
                plainBytes = ms.ToArray();
            }

            return Encoding.Unicode.GetString(plainBytes);
        }
    }
}


    

