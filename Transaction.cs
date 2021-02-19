using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace FrostCoin
{
    class Transaction
    {
        public PublicKey FromAddress { get; set; }
        public PublicKey ToAddress { get; set; }
        public decimal Amount { get; set; }
        public Signature Signature { get; set; }

        //constructor method for the class
        public Transaction(PublicKey fromAddress, PublicKey toAddress, decimal amount)
        {
            this.FromAddress = fromAddress;
            this.ToAddress = toAddress;
            this.Amount = amount;
        }

        //signing the transaction
        public void SignTransation(PrivateKey signingKey)
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string signingDER = BitConverter.ToString(signingKey.publicKey().toDer()).Replace("-", "");

            //actual validation that the person approves the transaction
            if (fromAddressDER != signingDER)
            {
                throw new Exception("You can't sign a transaction for another wallet!");
            }

            string txHash = this.CaluculateHash();
            this.Signature = Ecdsa.sign(txHash, signingKey);
        }

        //create the hash for the transaction
        public string CaluculateHash()
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string toAddressDER = BitConverter.ToString(ToAddress.toDer()).Replace("-", "");
            string transactionData = fromAddressDER + toAddressDER + Amount;
            byte[] tdBytes = Encoding.ASCII.GetBytes(transactionData);
            return BitConverter.ToString(SHA256.Create().ComputeHash(tdBytes)).Replace("-", "");
        }

        //if transaction is valid
        public bool IsValid()
        {
            //there will be a mining bonus assigned
            if (this.FromAddress is null) return true;

            if (this.Signature is null)
            {
                throw new Exception("No signature in this transaction.");
            }

            return Ecdsa.verify(this.CaluculateHash(), this.Signature, this.FromAddress);
        }
    }
}
