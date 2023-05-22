using Nethereum.Hex.HexConvertors.Extensions;

public class AnonymousPow : WalletPow
{
    
    public AnonymousPow(String rpcUrl) : base(generatePrivateKey(), rpcUrl) {}

    public static string generatePrivateKey() {
        var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        return ecKey.GetPrivateKeyAsBytes().ToHex();
    }
}