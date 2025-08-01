using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg_img : MonoBehaviour
{
    Bg_Controler bg_Controler;
    void Start()
    {
        bg_Controler = gameObject.transform.parent.GetComponent<Bg_Controler>();
    }

    public void last_random_gameObj_hidener()
    {
        bg_Controler.last_random_hidener();
    }
}
