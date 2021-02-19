using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;


namespace FrostCoin
{
    class Block
    {
        //specific properties required for a block
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public string Timestamp { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Block(int index, string timestamp, List<Transaction> transactions, string previousHash = "")
        {
            this.Index = index;
            this.Timestamp = timestamp;
            this.Transactions = transactions;
            this.PreviousHash = previousHash;
            this.Hash = CalculateHash();
            this.Nonce = 0;
        }

        //calculating the hash to determine if the block has been messed with
        public string CalculateHash()
        {
            //concatinate the string of the info for the block
            string blockData = this.Index + this.PreviousHash + this.Timestamp + this.Transactions.ToString() + this.Nonce;

            //creating the hash of the info for that block
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashByte = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashByte).Replace("-", "");
        }

        //mining method
        public void Mine(int difficulty)
        {
            //the difficulty is the number of 0s, loop goes through until there are that many 0s in the hash
            while (this.Hash.Substring(0,difficulty) != new String('0', difficulty))
            {
                this.Nonce++;
                this.Hash = this.CalculateHash();
                Console.WriteLine("Mining: " + this.Hash);
            }

            Console.WriteLine("Block has been mined " + this.Hash);
        }
    }
}
