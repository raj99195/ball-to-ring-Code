using UnityEngine;

public class BallPathManeger : MonoBehaviour
{
    public bool canViwed = false; // for Dont show object's in meno
    public int curent_step;
    public GameObject prefab_path;
    public GameObject main_ring;
    float main_ring_first_scale_x;
    float radius;
    float main_ring_Distanse_from_center;

    void Start()
    {
        radius = main_ring.GetComponent<Main_Ring>().radius;
        main_ring_first_scale_x = main_ring.GetComponent<Main_Ring>().main_ring_first_scale_x ; 
        main_ring_Distanse_from_center = main_ring.transform.position.y;
    }

    


    void createPath()
    {

            float secend_angel = 0;
            switch (curent_step)
            {

                case 1:
                    {
                        secend_angel = 90f;
                        Instantiate(
                            prefab_path,
                            new Vector3(
                                main_ring.transform.localScale.x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                main_ring.transform.localScale.x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        
                    }
                    break;
                case 2:
                    {
                        secend_angel = 30f;
                        Instantiate(
                            prefab_path,
                            new Vector3(
                                main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        Instantiate(
                            prefab_path,
                            new Vector3(
                                -1 * main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                     main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        
                    }
                    break;
                case 3:
                    {
                        secend_angel = 90f;
                        Instantiate( // right
                            prefab_path,
                            new Vector3(
                                main_ring_first_scale_x * radius * (Mathf.Cos((secend_angel - 120f) * Mathf.Deg2Rad)),
                                main_ring_first_scale_x * radius * (Mathf.Sin((secend_angel - 120f) * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        Instantiate(  // center
                            prefab_path,
                            new Vector3(
                               main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                               main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        Instantiate( // left
                            prefab_path,
                            new Vector3(
                                -1 * main_ring_first_scale_x * radius * (Mathf.Cos((secend_angel - 120f) * Mathf.Deg2Rad)),
                                     main_ring_first_scale_x * radius * (Mathf.Sin((secend_angel - 120f) * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        
                    }
                    break;
                case 4:
                    {
                        secend_angel = 90f;
                        Instantiate(
                            prefab_path,
                            new Vector3(
                               main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                               main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                       
                    }
                    break;
                case 5:
                    {
                        secend_angel = 45f;
                        Instantiate(
                            prefab_path,
                            new Vector3( // right 
                                main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );

                        Instantiate(
                            prefab_path,
                            new Vector3( //left 
                                -1 * main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                     main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                       
                    }
                    break;
                case 6:
                    {
                        secend_angel = 0f;
                        Instantiate(
                            prefab_path,
                            new Vector3( // right 
                                main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );

                        Instantiate(
                            prefab_path,
                            new Vector3( //left 
                                -1 * main_ring_first_scale_x * radius * (Mathf.Cos(secend_angel * Mathf.Deg2Rad)),
                                     main_ring_first_scale_x * radius * (Mathf.Sin(secend_angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        Instantiate(
                            prefab_path,
                            new Vector3( //top
                                main_ring_first_scale_x * radius * (Mathf.Cos((secend_angel + 90f) * Mathf.Deg2Rad)),
                                main_ring_first_scale_x * radius * (Mathf.Sin((secend_angel + 90f) * Mathf.Deg2Rad)) + main_ring_Distanse_from_center,
                                0
                            ),
                            Quaternion.Euler(0, 0, 0),
                            transform
                            );
                        
                    }
                    break;
            }


    }

    public void path_viwer(bool view , bool hidenAndRemove)
    {
        string action = null;
        if (view && canViwed)
        {
            //print("view path");
            action = "view";
        }else if(!view && hidenAndRemove)
        {
            //print("hiden and remove path");
            action = "hiden and remove";
        }else if(!view && !hidenAndRemove)
        {
            //print("hiden path");
            action = "hiden";
        }
        
        foreach(Animator animator in GetComponentsInChildren<Animator>())
        {
            if(action != null)
            {
                animator.SetTrigger(action);
            }
            
        }
    }

    public void pathControler(int step)
    {
        curent_step = step;

        path_viwer(false, true);

        createPath();

       
        path_viwer(true, false);



    }
}
