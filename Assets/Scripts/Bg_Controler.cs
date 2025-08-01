using System.Collections;
using UnityEngine;

public class Bg_Controler : MonoBehaviour
{
    ArrayList number_for_random_bgColor;

    public Color[] color1, color2, color3, color4, color5, color6;
    public Color[] curent_ring_colors;
    public int last_random = -1;
    public int random;
    void Start()
    {
        set_random_array();  
    }
    public void set_random_array()
    {
        number_for_random_bgColor = new ArrayList();
        for (int i = 0; i<6; i++)
        {
            number_for_random_bgColor.Add(i);
        }
    }

 
    int create_random_num()
    {
        int a = Random.Range(0, number_for_random_bgColor.Count);
        int random_number = (int) number_for_random_bgColor[a];
        number_for_random_bgColor.Remove(random_number);
        //Debug.Log(number_for_random_bgColor);
        return random_number;
    }

    public void last_random_hidener()
    {
        //print("last");
        if (last_random != -1)
        {
            if(last_random != random) {
                transform.GetChild(last_random).GetComponent<Animator>().SetTrigger("bgHiden");
            }
        }
        last_random = random;
    }
    
    public void set()
    {

        random = create_random_num();
        
        switch (random)
        {
            case 0:
                {
                    curent_ring_colors = color1;
                }
                break;
            case 1:
                {
                    curent_ring_colors = color2;
                }
                break;
            case 2:
                {
                    curent_ring_colors = color3;
                }
                break;
            case 3:
                {
                    curent_ring_colors = color4;
                }
                break;
            case 4:
                {
                    curent_ring_colors = color5;
                }
                break;
            case 5:
                {
                    curent_ring_colors = color6;
                }
                break;

        }
        
        
    }

    public void setBg()
    {
        if (last_random != -1)
        {
            transform.GetChild(last_random).GetComponent<Canvas>().sortingOrder = 0;
        }
        transform.GetChild(random).GetComponent<Canvas>().sortingOrder = 1;
        
        if(last_random != random)
            transform.GetChild(random).GetComponent<Animator>().SetTrigger("bgView");
    }

}
