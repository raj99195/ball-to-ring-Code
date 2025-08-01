using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DD.Web3
{
    public class ConnectionManager : MonoBehaviour
    {
        [SerializeField] private Button connectWalletButton;
        [SerializeField] private TMP_Text statusText;

        //[SerializeField] private BigInteger chainId = 1868;
        //[SerializeField] private string dropErc20ContractAddress = "0xeb9415D0B989B18231E6977819c24DEF47c855A8";

        [Header("Wallet Info Display")]
        //[SerializeField] private GameObject walletInfoPanel;
        //[SerializeField] private TMP_Text walletAddressText;
        //[SerializeField] private TMP_Text walletBalanceText;
        //[SerializeField] private TMP_Text dropERC20BalanceText;
        //[SerializeField] private TMP_Text dropERC721BalanceText;


        [SerializeField] private GameObject loadingScreen;

        [SerializeField] private Button disconnectButton;


        [Header("Other Script References")]
        //[SerializeField] private BlockchainManager blockchainManager;
        //[SerializeField] private WalletConnected walletConnected;

        [SerializeField] private WalletProvider externalWalletProvider = WalletProvider.WalletConnectWallet;
        [SerializeField] private bool forceMetaMaskOnWebGL = false;


        // --- Cache for ERC20 Claim ---
        private Drop_ClaimCondition cachedClaimConditionERC20;
        private int cachedERC20Decimals;
        private bool isERC20CacheReady = false;
        private IThirdwebWallet wallet;
        private ThirdwebContract dropErc20Contract = null;
        private ThirdwebContract dropErc20LifeContract = null;

        private ThirdwebContract dropErc721Contract = null;

        
        //private string walletAddress = "";


        private void Start()
{
            
    // Make sure cursor is visible when UI needs interaction
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;

    ShowLoadingScreen(false);

    if (connectWalletButton != null)
    {
        connectWalletButton.gameObject.SetActive(true);
        connectWalletButton.onClick.RemoveAllListeners();
        connectWalletButton.onClick.AddListener(() =>
        {
            _ = ConnectWalletAsync();
        });
    }

    if (disconnectButton != null)
    {
        disconnectButton.gameObject.SetActive(false);
    }
    Invoke("ConnectWalletAsync()", 4f);
}


public async Task ConnectWalletAsync()
{
    ShowLoadingScreen(true, "Connecting wallet...");
    await ConnectExternalWallet();
    ShowLoadingScreen(false);
}



        private void ThirdwebInitialized()
        {
            Debug.Log("BlockchainConnectionManager Confirms Thirdweb Initialized");
            ShowLoadingScreen(false);

            if (connectWalletButton != null)
            {
                connectWalletButton.gameObject.SetActive(true);
            }
        }

        private async void ConnectWallet()
{
    await ConnectExternalWallet(); // ✅ This runs in the user click context
}


        private async void DisconnectWallet()
        {
            if (wallet != null)
            {
                await wallet.Disconnect();
                BlockchainManager.Instance.walletAddress = "";

                /*LocalStorageManager.Instance.addressValue = "";

                LocalStorageManager.Instance.playerInfo.address = "";*/

            }


            if (connectWalletButton != null)
                connectWalletButton.gameObject.SetActive(true);
        }



        private async Task ConnectExternalWallet()
{
    try
    {
        ShowLoadingScreen(true, "Connecting Metamask ...");
        connectWalletButton.gameObject.SetActive(false);

        SwitchChain(); // Optional: might be redundant if wallet.SwitchNetwork works fine

        var externalWalletProvider = 
            Application.platform == RuntimePlatform.WebGLPlayer && forceMetaMaskOnWebGL 
            ? WalletProvider.MetaMaskWallet 
            : WalletProvider.WalletConnectWallet;

        Debug.Log("🦊 Wallet provider = " + externalWalletProvider);

        var options = new WalletOptions(
            provider: externalWalletProvider,
            chainId: BlockchainManager.Instance.currentConfig.ChainId
        );

        Debug.Log("📡 Awaiting MetaMask popup…");

        float startTime = Time.realtimeSinceStartup;

        // ✅ FIXED LINE HERE
        wallet = await ThirdwebManager.Instance.ConnectWallet(options);

        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log($"✅ Wallet response after {elapsedTime:F2} seconds");

        if (wallet != null)
        {
            Debug.Log("🟢 Wallet connected");

            var address = await wallet.GetAddress();
            BlockchainManager.Instance.walletAddress = address;
            Debug.Log("📬 Wallet address: " + address);

            await wallet.SwitchNetwork(BlockchainManager.Instance.currentConfig.ChainId);
            Debug.Log("🔁 Network switched");

            SwitchChain(); // optional

            WalletConnected();

            GetDropErc20Balance();
            GetDropErc20LifeBalance();
        }
        else
        {
            Debug.LogWarning("🚫 Wallet returned null!");
        }

        ShowLoadingScreen(false); // ✅ make sure screen is hidden
    }
    catch (System.Exception e)
    {
        Debug.LogError("❌ Error connecting wallet: " + e.Message);
        ShowLoadingScreen(false);
        connectWalletButton.gameObject.SetActive(true);
    }
}



        private void WalletConnected()
        {
            SaveWalletAddress(BlockchainManager.Instance.walletAddress);
            BlockchainManager.Instance.walletConnected.OnWalletConnected();

        }
        private async void GetDropErc20Balance()
        {
            try
            {
                
                dropErc20Contract = await ThirdwebManager.Instance.GetContract(address: BlockchainManager.Instance.currentConfig.dropERC20ContractAddress, chainId: BlockchainManager.Instance.currentConfig.ChainId);
                var symbol = await dropErc20Contract.ERC20_Symbol();
                    //"abc";
                var balance = await dropErc20Contract.ERC20_BalanceOf(ownerAddress: BlockchainManager.Instance.walletAddress);
                var balanceEth = Thirdweb.Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 0, addCommas: false);
                Debug.Log($" {symbol} Balance: {balanceEth} {symbol}  \n And NormBalance = {balance}");
                BlockchainManager.Instance.onDropErc20BalanceUpdated?.Invoke(balanceEth);
                BlockchainManager.Instance.dropErc20Balance = balanceEth;
                //dropERC20BalanceText.text = balanceEth;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private async void GetDropErc20LifeBalance()
        {
            try
            {

                dropErc20LifeContract = await ThirdwebManager.Instance.GetContract(address: BlockchainManager.Instance.currentConfig.purchasingTokenContractAddress, chainId: BlockchainManager.Instance.currentConfig.ChainId);
                var symbol = await dropErc20LifeContract.ERC20_Symbol();
                //"abc";
                var balance = await dropErc20LifeContract.ERC20_BalanceOf(ownerAddress: BlockchainManager.Instance.walletAddress);
                var balanceEth = Thirdweb.Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 0, addCommas: false);
                Debug.Log($" {symbol} Balance: {balanceEth} {symbol}  \n And NormBalance = {balance}");
                //dropERC20BalanceText.text = balanceEth;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private async void GetDropErc721Balance()
        {
            try
            {

                dropErc721Contract = await ThirdwebManager.Instance.GetContract(address: BlockchainManager.Instance.currentConfig.purchasingTokenContractAddress, chainId: BlockchainManager.Instance.currentConfig.ChainId);
                var symbol = await dropErc721Contract.ERC721_Symbol();
                //"abc";
                var balance = await dropErc721Contract.ERC721_BalanceOf(ownerAddress: BlockchainManager.Instance.walletAddress);
                Debug.Log("ERC 721 balance = " + balance);
                var balanceEth = Thirdweb.Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 0, addCommas: false);
                Debug.Log($" {symbol} Balance: {balanceEth} {symbol}  \n And NormBalance = {balance}");
                //dropERC721BalanceText.text = "ERC721 Balance = " + balance;
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

private async Task<bool> EnsureWalletConnected()
{
    if (wallet == null)
    {
        Debug.LogWarning("Wallet is null, attempting to reconnect...");
        await ConnectExternalWallet(); // Connects and assigns to wallet variable
    }

    return wallet != null;
}



      public async void ClaimDropERC20(int amt = 0, Action<bool> onTransaction = null)
{
    Debug.Log($"Amount to be claimed {amt}");

    try
    {
        ShowLoadingScreen(true, "Claiming Points...");

        // ✅ Ensure wallet is connected
        if (!await EnsureWalletConnected())
        {
            Debug.LogError("🚫 Wallet is not connected. Aborting claim.");
            ShowLoadingScreen(false);
            onTransaction?.Invoke(false);
            return;
        }

        await wallet.SwitchNetwork(BlockchainManager.Instance.currentConfig.ChainId);
        SwitchChain();

        // Re-fetch contract in case of stale instance
        dropErc20Contract = await ThirdwebManager.Instance.GetContract(
            address: BlockchainManager.Instance.currentConfig.dropERC20ContractAddress,
            chainId: BlockchainManager.Instance.currentConfig.ChainId
        );

        // Check claim conditions
        var claimCondition = await dropErc20Contract.DropERC20_GetActiveClaimCondition();
        Debug.Log("Currency: " + claimCondition.Currency);
        Debug.Log("Price per token: " + claimCondition.PricePerToken);
        Debug.Log("Quantity: " + amt);

        // Claim Tokens
        var tx = await dropErc20Contract.DropERC20_Claim(wallet, BlockchainManager.Instance.walletAddress, amt.ToString());

        Debug.Log("Transaction: " + tx.ToString());

        if (tx.Status.Value != BigInteger.One)
        {
            Debug.LogWarning("Claim transaction failed.");
            onTransaction?.Invoke(false);
        }
        else
        {
            Debug.Log("✅ ERC20 Claim successful!");
            PointsClaimedSucceed();
            SaveClaim();
            onTransaction?.Invoke(true);
        }
    }
    catch (Exception e)
    {
        Debug.LogError("🚫 Claiming Error: " + e.Message);
        onTransaction?.Invoke(false);
    }
    finally
    {
        ShowLoadingScreen(false);
        GetDropErc20Balance();
    }
}





        private void PointsClaimedSucceed()
        {
            //GameController.Instance.claimBtn.gameObject.SetActive(false);

            Debug.Log("On Points Claimed Invoked");
        }

        public async void ClaimDropERC721(TMP_InputField tokenAmountIF)
        {
            BigInteger.TryParse(tokenAmountIF.text, out BigInteger amt);
            Debug.Log($"Amount to be claimed {amt}");

            await wallet.SwitchNetwork(BlockchainManager.Instance.currentConfig.ChainId);
            SwitchChain();

            

            try
            {
                ShowLoadingScreen(true,"Sending Purchase Request...");

                var drop_ClaimCondition = await dropErc721Contract.DropERC721_GetActiveClaimCondition();
                Debug.Log("Currency: " + drop_ClaimCondition.Currency);
                Debug.Log("Price per token: " + drop_ClaimCondition.PricePerToken);
                Debug.Log("Quantity: " + amt);


                var transactionReceipt = await dropErc721Contract.DropERC721_Claim(wallet, BlockchainManager.Instance.walletAddress, amt);
                Debug.Log(transactionReceipt.ToString());
                ShowLoadingScreen(false);

                if (!transactionReceipt.Status.Value.Equals(BigInteger.One))
                {
                    Debug.Log(" ERC Purchase Transaction Failed failed ");
                }
                else
                {
                    

                    Debug.Log("ERC 721 purchase done "+transactionReceipt.ToString());

                    int.TryParse(amt.ToString(), out int pointToIncrease);

                    // Update Lives 

                    //GameDataUpdateRequest infoSend = new GameDataUpdateRequest(LocalStorageManager.Instance.playerInfo.address, pointToIncrease, "Purchased NFTS");

                    //LocalStorageManager.Instance.IncreaseLives((int)amt);

                    /*string jsonData = JsonUtility.ToJson(infoSend);
                    Debug.Log(jsonData);
                    LocalStorageManager.Instance.SendData(jsonData);*/


                }

            }
            catch (Exception e)
            {
                ShowLoadingScreen(false,"");
                Debug.Log("Erc721 Claiming Error   : " + e);
            }

            GetDropErc721Balance();

        }





        public async Task<bool> ClaimDropERC721(int tokenAmount = 1)
        {
            BigInteger amt = new BigInteger(tokenAmount);
            Debug.Log($"Amount to be claimed: {amt}");

            await wallet.SwitchNetwork(BlockchainManager.Instance.currentConfig.ChainId);
            SwitchChain();

            try
            {
                ShowLoadingScreen(true, "Sending Purchase Request...");

                var drop_ClaimCondition = await dropErc721Contract.DropERC721_GetActiveClaimCondition();
                Debug.Log("Currency: " + drop_ClaimCondition.Currency);
                Debug.Log("Price per token: " + drop_ClaimCondition.PricePerToken);
                Debug.Log("Quantity: " + amt);
                var totalSupply = await dropErc721Contract.ERC721_TotalSupply();
                Debug.Log("Total Supply : " + totalSupply);

                Debug.Log("wallet: " + wallet);
                Debug.Log("BlockchainManager.Instance: " + BlockchainManager.Instance);
                Debug.Log("BlockchainManager.Instance.walletAddress: " + BlockchainManager.Instance.walletAddress);
                Debug.Log("amt: " + amt);

                var transactionReceipt = await dropErc721Contract.DropERC721_Claim(wallet, BlockchainManager.Instance.walletAddress, amt);

                Debug.Log(transactionReceipt.ToString());
                ShowLoadingScreen(false);

                if (!transactionReceipt.Status.Value.Equals(BigInteger.One))
                {
                    Debug.Log("ERC Purchase Transaction Failed");
                    return false;
                }

                Debug.Log("ERC 721 purchase done " + transactionReceipt.ToString());

                // Optional: update logic
                return true;
            }
            catch (Exception e)
            {
                ShowLoadingScreen(false, "");
                Debug.Log("ERC721 Claiming Error: " + e);
                return false;
            }
            finally
            {
                GetDropErc721Balance();
            }
        }




        public async void PurchaseDropERC20Life(TMP_InputField tokenAmountIF)
        {
            BigInteger.TryParse(tokenAmountIF.text, out BigInteger amt);
            Debug.Log($"Amount to be claimed {amt}");

            await wallet.SwitchNetwork(BlockchainManager.Instance.currentConfig.ChainId);
            SwitchChain();



            try
            {
                ShowLoadingScreen(true, "Sending Life Purchase Request...");

                var drop_ClaimCondition = await dropErc20LifeContract.DropERC20_GetActiveClaimCondition();
                Debug.Log("Currency: " + drop_ClaimCondition.Currency);
                Debug.Log("Price per token: " + drop_ClaimCondition.PricePerToken);
                Debug.Log("Quantity: " + amt);

                //BigInteger scaled = amt * BigInteger.Pow(10, 18);

                var transactionReceipt = await dropErc20LifeContract.DropERC20_Claim(wallet, BlockchainManager.Instance.walletAddress, amt.ToString());
                Debug.Log(transactionReceipt.ToString());
                ShowLoadingScreen(false);

                if (!transactionReceipt.Status.Value.Equals(BigInteger.One))
                {
                    Debug.Log(" ERC Life Purchase Transaction Failed failed ");
                }
                else
                {


                    Debug.Log("ERC 20 Life purchase done " + transactionReceipt.ToString());

                    int.TryParse(amt.ToString(), out int pointToIncrease);

                    // Update Lives 

                    //GameDataUpdateRequest infoSend = new GameDataUpdateRequest(LocalStorageManager.Instance.playerInfo.address, pointToIncrease, "Purchased NFTS");

                    //LocalStorageManager.Instance.IncreaseLives((int)amt);

                    /*string jsonData = JsonUtility.ToJson(infoSend);
                    Debug.Log(jsonData);
                    LocalStorageManager.Instance.SendData(jsonData);*/


                }

            }
            catch (Exception e)
            {
                ShowLoadingScreen(false, "");
                Debug.Log("Life Purchasing Error   : " + e);
            }

            GetDropErc20LifeBalance();

        }

        public static string ConvertToTokenDecimals(int amount, int decimals)
        {
            BigInteger bigAmount = BigInteger.Parse(amount.ToString());
            BigInteger scaled = bigAmount * BigInteger.Pow(10, decimals);
            return scaled.ToString();
        }



        public void ShowLoadingScreen(bool _willShow = true, string loadingText = "Loading...")
        {
            loadingScreen.SetActive(_willShow);
            statusText.text = loadingText;
            
        }


        #region Keep Track Of Game Scene
        private int _sceneReloadedTimes = 0;
        public int SceneReloadedTimes
        {
            get
            {
                return _sceneReloadedTimes;
            }
            set
            {
                _sceneReloadedTimes = value;
            }
        }

        #endregion


        #region SaveOnLocalDevice

        [DllImport("__Internal")]
        private static extern void SaveData(string key, string value);

        [DllImport("__Internal")]
        private static extern IntPtr LoadData(string key);

        private const string CLAIM_COUNT = "ClaimCount";
        private const string WALLET_ADDRESS = "WalletAddress";


        public void SaveClaim()
        {
            int claimCount = 0;
#if UNITY_WEBGL && !UNITY_EDITOR

            IntPtr valuePtr = LoadData(CLAIM_COUNT);
            string value = Marshal.PtrToStringAnsi(valuePtr);

            string savedClaimCountString = value;
            int.TryParse(savedClaimCountString, out claimCount);

            claimCount++;

            SaveData(CLAIM_COUNT, claimCount.ToString());

            //addressValue = value;
            //PlayerPrefs.SetString(PrefsAddressKey, addressValue);

#else
            if (PlayerPrefs.HasKey(CLAIM_COUNT))
            {
                string savedClaimCountString = PlayerPrefs.GetString(CLAIM_COUNT);
                int.TryParse(savedClaimCountString, out claimCount);
            }
            claimCount++;
            PlayerPrefs.SetString(CLAIM_COUNT, claimCount.ToString());

#endif
            Debug.Log("ClaimCount = " + claimCount);
        }

        private void SaveWalletAddress(string _walletAddress)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SaveData(WALLET_ADDRESS, _walletAddress);
#else
            PlayerPrefs.SetString(WALLET_ADDRESS, _walletAddress);
#endif
        }

        #endregion

        #region SwitchNetwork
        //#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
    private static extern void AddAndSwitchChain(string chainId, string rpcUrl, string chainName, string nativeCurrencyJson, string blockExplorerUrl);
//#endif

        public void SwitchChain()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            //string chainId = new HexBigInteger( blockchainManager.currentConfig.ChainId ).ToString(); // Mumbai Polygon Testnet

            BigInteger chainIdDecimal = BlockchainManager.Instance.currentConfig.ChainId;
            //string hexChainId = "0x" + chainIdDecimal.ToString("X"); // Ensure no padding
            string hexChainId = "0x" + (chainIdDecimal == 0 ? "0" : chainIdDecimal.ToString("X").TrimStart('0'));


            Debug.Log("Chain id in hex :  " + hexChainId);


            string rpcUrl = BlockchainManager.Instance.currentConfig.rpcUrl;
        string chainName = BlockchainManager.Instance.currentConfig.networkName;
        string blockExplorer = BlockchainManager.Instance.currentConfig.blockExplorerUrl;
        string nativeCurrencyJson = BlockchainManager.Instance.currentConfig.nativeCurrencyJson;

        AddAndSwitchChain(hexChainId, rpcUrl, chainName, nativeCurrencyJson, blockExplorer);
#else
            Debug.LogWarning("Chain switching only works in WebGL builds.");
#endif
        }
        #endregion
    }

    /*string chainId = "1868"; // Mumbai Polygon Testnet
    string rpcUrl = "https://rpc.soneium.org";
    string chainName = "Soneium";
    string blockExplorer = "https://soneium.blockscout.com";
    string nativeCurrencyJson = "{\"name\":\"Soneium\",\"symbol\":\"ETH\",\"decimals\":18}";*/

}
