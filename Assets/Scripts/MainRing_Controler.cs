using UnityEngine;

public class MainRing_Controler : MonoBehaviour
{
    public int curent_step;
    public bool can_tuch_to_rotate;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void rotate(int dirction)
    {
        
        if (can_tuch_to_rotate)
        {
            
            if (curent_step < 4)
            {
                if (dirction == 1)
                {
                    animator.SetTrigger("R120");
                }
                else if (dirction == -1)
                {
                    animator.SetTrigger("L120");
                }
            }
            else if (curent_step >= 4)
            {
                if (dirction == 1)
                {
                    animator.SetTrigger("R90");
                }
                else if (dirction == -1)
                {
                    animator.SetTrigger("L90");
                }
            }

        }


    }
    
}
