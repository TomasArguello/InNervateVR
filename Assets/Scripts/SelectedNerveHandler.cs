using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedNerveHandler : MonoBehaviour
{

    public Button[] nerveStateOptions;
    public int currentNerveModeClicked;

    public cutNerve[] nerveObjectList;


    // Start is called before the first frame update

    void Start()
    {
        //int index = 0;
        /*foreach (Button item in nerveStateOptions) {

            index += 1;
            item.onClick.AddListener(() => RegisterButtonClick(index)    );
        }*/

        currentNerveModeClicked = 0;


        nerveStateOptions[0].onClick.AddListener(() => RegisterButtonClick(0));
        nerveStateOptions[1].onClick.AddListener(() => RegisterButtonClick(1));
        nerveStateOptions[2].onClick.AddListener(() => RegisterButtonClick(2));
        nerveStateOptions[3].onClick.AddListener(() => RegisterButtonClick(3));

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void RegisterButtonClick( int myIndex )
    {

        currentNerveModeClicked = myIndex + 1;

       

        foreach( cutNerve item in nerveObjectList)
        {
            item.SetNerveMode( currentNerveModeClicked);
            
        }


        foreach( Button item in nerveStateOptions)
        {
            item.targetGraphic.color = item.colors.normalColor;

        }

        nerveStateOptions[myIndex].targetGraphic.color = nerveStateOptions[myIndex].colors.highlightedColor;

        //set current button selected
        //change color of current button selected 
        //reset all other buttons to not selected color

    }


    void HighlightButton()
    {







    }

}
