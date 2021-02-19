using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace FrostCoin
{
    class Program
    {
        static void Main(string[] args)
        {
            Blockchain frostcoin = new Blockchain();

            frostcoin.AddBlock(new Block(1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amoutnt: 50"));
            frostcoin.AddBlock(new Block(2, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amoutnt: 50"));

            string blockJSON = JsonConvert.SerializeObject(frostcoin, Formatting.Indented);
            Console.WriteLine(blockJSON);

            if (frostcoin.IsChainValid())
            {
                Console.WriteLine("Blockchain Valid");
            }
            else
            {
                Console.WriteLine("Blockchain Not Valid");
            }
        }
    }

}
