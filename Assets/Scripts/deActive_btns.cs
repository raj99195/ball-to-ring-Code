using UnityEngine;

public class deActive_btns : MonoBehaviour
{
    /// <summary>
    /// deActive Button intractable that called in anim
    /// </summary>
    public void deActive()
    {
        print("deActive");
        gameObject.SetActive(false);
        gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
}
