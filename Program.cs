using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

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
        }
    }

    class Blockchain
    {
        public List<Block> Chain { get; set; }

        public Blockchain()
        {
            this.Chain = new List<Block>();
            //adding the genesis block to the chain
            this.Chain.Add(CreateGenesisBlock());
        }

        //creating the genesis block
        public Block CreateGenesisBlock()
        {
            return new Block(0, DateTime.Now.ToString("yyyyMMddHHmmssffff"), "GENESIS BLOCK");
        }

        //accessing the most recent block in the chain
        public Block GetLatestBlock()
        {
            return this.Chain.Last();
        }

        //adding a block to the chain
        public void AddBlock(Block newBlock)
        {
            newBlock.PreviousHash = this.GetLatestBlock().Hash;
            newBlock.Hash = newBlock.CalculateHash();
            this.Chain.Add(newBlock);
        }

    }

    class Block
    {
        //specific properties required for a block
        public int Index { get; set; }
        public string PreviousHash { get; set; }
        public string Timestamp { get; set; }
        public string Data { get; set; }
        public string Hash { get; set; }

        public Block(int index, string timestamp, string data, string previousHash = "")
        {
            this.Index = index;
            this.Timestamp = timestamp;
            this.Data = data;
            this.PreviousHash = previousHash;
            this.Hash = CalculateHash();
        }

        //calculating the hash to determine if the block has been messed with
        public string CalculateHash()
        {
            //concatinate the string of the info for the block
            string blockData = this.Index + this.PreviousHash + this.Timestamp + this.Data;

            //creating the hash of the info for that block
            byte[] blockBytes = Encoding.ASCII.GetBytes(blockData);
            byte[] hashByte = SHA256.Create().ComputeHash(blockBytes);
            return BitConverter.ToString(hashByte).Replace("-", "");
        }
    }
}
