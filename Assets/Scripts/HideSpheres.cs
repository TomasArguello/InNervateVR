using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpheres : MonoBehaviour
{

    Animator anim;
    public GameObject[] indicatorSpheres;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DefaultState"))
        {
            //Debug.Log("Animations Playing");

            foreach ( GameObject sphere in indicatorSpheres)
            {
                sphere.SetActive(false);
            }
        }

        else
        {
            //Debug.Log("No animation");
            foreach (GameObject sphere in indicatorSpheres)
            {
                sphere.SetActive(true);
            }
        }
    }
}
