using ProofOfWork;


class CLIMiner
{
        static async Task Main(string[] args)
    {    
        AnonymousPow pow = new AnonymousPow("https://staging-v3.skalenodes.com/v1/staging-utter-unripe-menkar");
        await pow.send("0xa9eC34461791162Cae8c312C4237C9ddd1D64336", "0x0c11dedd000000000000000000000000d52577D6c373b948222dCA6166098eCC9Ff42875", 75000);
    }
}
