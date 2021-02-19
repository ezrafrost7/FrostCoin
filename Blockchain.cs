using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace FrostCoin
{
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

        //method to tell if a chain is valid
        public bool IsChainValid()
        {
            for (int i = 1; i < this.Chain.Count; i++)
            {
                Block currentBlock = this.Chain[i];
                Block previousBlock = this.Chain[i - 1];

                //check if the current hash is the same as the calculated hash
                //recalculate the hash, does it match?
                //checks current block
                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                //checking the chain
                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            //if neither of those above are false, than the blockchain is valid
            return true;
        }

    }
}
