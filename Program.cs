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
            //setting up a couple of wallets
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();

            Blockchain frostcoin = new Blockchain(2,100);

            Console.WriteLine("Start up the miner.");
            frostcoin.MinePendingTransaction(wallet1);
            Console.WriteLine("\nBalance of wallet1 is $" + frostcoin.GetBalanceOfWallet(wallet1));

            //instead of adding blocks, they need to be mined now
            //frostcoin.AddBlock(new Block(1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amoutnt: 50"));
            //frostcoin.AddBlock(new Block(2, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "amoutnt: 50"));

            Transaction tx1 = new Transaction(wallet1, wallet2, 10);
            tx1.SignTransaction(key1);
            frostcoin.addPendingTransaction(tx1);
            Console.WriteLine("Start the miner");
            frostcoin.MinePendingTransaction(wallet2);
            Console.WriteLine("\nBalance of wallet1 is $" + frostcoin.GetBalanceOfWallet(wallet1).ToString());
            Console.WriteLine("\nBalance of wallet2 is $" + frostcoin.GetBalanceOfWallet(wallet2).ToString());

            string blockJSON = JsonConvert.SerializeObject(frostcoin, Formatting.Indented);
            Console.WriteLine(blockJSON);

            if (frostcoin.IsChainValid())
            {
                Console.WriteLine("Blockchain Valid");
            }
            else
            {
                Console.WriteLine("Blockchain NOT Valid");
            }
        }
    }

}
