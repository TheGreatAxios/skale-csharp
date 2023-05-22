using ProofOfWork;

using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;

using System.Numerics;

public class WalletPow : Miner {

    private Account account;
    private Web3 web3;

    public WalletPow(string privateKey, string rpcUrl) {
        Console.WriteLine(privateKey);
        this.account = new Account(privateKey);
        Console.WriteLine("Address: " + this.account.Address);
        this.web3 = new Web3(this.account, rpcUrl);
    }

    public async Task<string> send(String to, String data, long gas = 100000) {
        Console.WriteLine(0);
        // long gas = gasAmount ? gasAmount : 100000;
        Console.WriteLine("Gas:");
        Console.WriteLine(gas);

        HexBigInteger nonce = await this.web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(this.account.Address, BlockParameter.CreatePending());

        Console.WriteLine(2);

        string result = await MineGasForTransaction(nonce, gas, this.account.Address);
        
        // Console.WriteLine("Result");
        // Console.WriteLine(result);  
        // Console.WriteLine(new HexBigInteger(BigInteger.Parse(result)));

        Console.WriteLine("Address: " + this.account.Address);

        TransactionInput tx = new TransactionInput(
            data,
            to,
            addressFrom: this.account.Address,
            value: new HexBigInteger(0),
            gasPrice: BigInteger.Parse(result).ToHexBigInteger(),
            gas: new HexBigInteger(gas)
        );
        Console.WriteLine(tx.From);
        tx.Nonce = nonce;

        Console.WriteLine(tx);
        string signedTx = await web3.Eth.TransactionManager.SignTransactionAsync(tx);
        Console.WriteLine(signedTx);
        // return await web3.Eth.TransactionManager.SendTransactionAndWaitForReceiptAsync(tx);

        return "";
    }
}

// 114300136082501599864048599238123214497793374349333258509328848624001693396662
// 28359845180812678060123784446694493676845667182330883574334589667417536038403
// 41573526148156300086939675893764641959041302997817238081803970288791848636546