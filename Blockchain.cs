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
        public int Difficulty { get; set; }
        public List<Transaction> pendingTransactions { get; set; }
        public decimal MiningReward { get; set; }

        public Blockchain(int difficulty, decimal miningReward)
        {
            this.Chain = new List<Block>();
            //adding the genesis block to the chain
            this.Chain.Add(CreateGenesisBlock());
            this.Difficulty = difficulty;
            this.MiningReward = miningReward;
            this.pendingTransactions = new List<Transaction>();
        }

        //creating the genesis block
        public Block CreateGenesisBlock()
        {
            return new Block(0, DateTime.Now.ToString("yyyyMMddHHmmssffff"), new List<Transaction>());
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

        //adding a transaction
        public void addPendingTransaction(Transaction transaction)
        {
            //some checks for the transactions
            if (transaction.FromAddress is null || transaction.ToAddress is null)
            {
                throw new Exception("Transaction must include a to and from address");
            }
            if (transaction.Amount > this.GetBalanceOfWallet(transaction.FromAddress))
            {
                throw new Exception("There is not sufficient money in your wallet");
            }
            if (transaction.IsValid() == false)
            {
                throw new Exception("Cannot add an invalid transaction to a block");
            }

            this.pendingTransactions.Add(transaction);
        }

        //getting the balance of a wallet
        public decimal GetBalanceOfWallet(PublicKey address)
        {
            decimal balance = 0;

            string addressDER = BitConverter.ToString(address.toDer()).Replace("-", "");

            //you need to add up the # of transactions in a block
            foreach (Block block in this.Chain)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (!(transaction.FromAddress is null))
                    {

                        string fromDER = BitConverter.ToString(transaction.FromAddress.toDer()).Replace("-", "");

                        if (fromDER == addressDER)
                        {
                            balance -= transaction.Amount;
                        }
                    }

                    string toDER = BitConverter.ToString(transaction.ToAddress.toDer()).Replace("-", "");
                    if (toDER == addressDER)
                    {
                        balance += transaction.Amount;
                    }
                }
            }

            return balance;
        }

        //mining the transactions
        public void MinePendingTransaction(PublicKey miningRewardWallet)
        {
            Transaction rewardTx = new Transaction(null, miningRewardWallet, MiningReward);
            this.pendingTransactions.Add(rewardTx);

            Block newBlock = new Block(GetLatestBlock().Index + 1, DateTime.Now.ToString("yyyyMMddHHmmssffff"), this.pendingTransactions, GetLatestBlock().Hash);
            newBlock.Mine(this.Difficulty);

            Console.WriteLine("Block Successfully mined!");
            this.Chain.Add(newBlock);
            this.pendingTransactions = new List<Transaction>();
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
