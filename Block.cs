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
