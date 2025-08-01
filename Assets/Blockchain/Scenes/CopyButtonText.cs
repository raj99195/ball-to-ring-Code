using DD.Web3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopyButtonText : MonoBehaviour
{
    public Button targetButton; // Assign the button in Inspector
    public string address;

    private void Start()
    {
        address = BlockchainManager.Instance.walletAddress;
        targetButton.onClick.AddListener(CopyButtonLabel);
    }

    void CopyButtonLabel()
    {
        GUIUtility.systemCopyBuffer = address;
        Debug.Log("Copied: " + address);

       
    }
}
