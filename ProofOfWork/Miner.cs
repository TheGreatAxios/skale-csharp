namespace ProofOfWork;

using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using BNSharp;
using Nethereum.ABI;
using Org.BouncyCastle.Security;

public class Miner
{
    private static readonly BigInteger DIFFICULTY = new BigInteger(1);

    public async Task<string> MineGasForTransaction(HexBigInteger nonce, long gas, String from)
    {
        return await MineFreeGas(gas, from, nonce);

    }

    private byte[] generateRandomBytes()
    {
        byte[] randomBytes = new byte[32];

        SecureRandom secureRandom = new SecureRandom();
        secureRandom.NextBytes(randomBytes);

        return randomBytes;
    }

    private async Task<string> MineFreeGas(long gasAmount, string address, HexBigInteger nonce)
    {

        BN nonceHash = new BN(GetSoliditySha3(nonce), 16);
        BN addressHash = new BN(GetSoliditySha3(address.HexToByteArray()), 16); // BN addressHash = new BN(GetSoliditySha3(address.HexToByteArray()), 16);
        BN nonceAddressXOR = nonceHash.Xor(addressHash);
        BN maxNumber = new BN(2).Pow(new BN(256)).Sub(new BN(1));
        
        BN divConstant = maxNumber.Div(new BN(1));
        BN candidate;

        long iterations = 0;

        while (true)
        {
            byte[] candidateBytes = generateRandomBytes(); // .PadTo32Bytes();
            // byte[] candidateBytes = "a8b9d09fbb5f8b31b2bc19c558288005aea13b52f3de4d5bf271530a5d41682a".HexToByteArray().pad


            candidate = new BN(candidateBytes, 16);
        
            BN candidateHash = new BN(GetSoliditySha3((candidate.ToString()).HexToByteArray()),16);

            BN resultHash = nonceAddressXOR.Xor(candidateHash);

            long externalGas = divConstant.Div(resultHash).ToNumber();

            if (externalGas >= gasAmount)
            {
                // Debug.Log("externalGas " + externalGas);
                // Debug.Log("candidate " + candidate);
                break;
            }

            if (iterations++ % 2_000 == 0)
            {
                await Task.Delay(0);
            }
        }

        return candidate.ToString();
    }


    /**
     * It works like the function soliditySha3 from the JS package web3-utils 
     */
    private string GetSoliditySha3(object val)
    {
        var abiEncode = new ABIEncode();
        var result = abiEncode.GetSha3ABIEncodedPacked(val);
        return result.ToHex();
    }
}