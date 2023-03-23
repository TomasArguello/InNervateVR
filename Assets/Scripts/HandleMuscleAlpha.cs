using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleMuscleAlpha : MonoBehaviour
{
    private float startAlpha;
    private float currentAlpha;
    private float alphaTarget;

    private bool visibleBool;
    private Renderer MuscleRenderer;

    private IEnumerator FadeCoroutine;
    public Toggle healthyLegShowToggle;
    public Slider opacitySlider;


    // Use this for initialization
    void Start()
    {
        //Starting alpha
        currentAlpha = 0;
        alphaTarget = 1;
        visibleBool = false;

        MuscleRenderer = GetComponent<Renderer>();

        MuscleRenderer.material.color = new Color(1, 1, 1, 0);

        MuscleRenderer.enabled = false;

        opacitySlider.onValueChanged.AddListener(delegate {
            UpdateTargetAlpha(opacitySlider);
        });
		
		
		healthyLegShowToggle.onValueChanged.AddListener(delegate {
            ToggleVisBool();
        });
    }

   

    public void UpdateTargetAlpha(Slider target)
    {
        alphaTarget = target.value;

        if(healthyLegShowToggle.isOn == true){
            MuscleRenderer.material.color = new Color(1, 1, 1, alphaTarget);
        }
    }



    public void ToggleVisBool()
    {

        visibleBool = healthyLegShowToggle.isOn;
		CallFadeFunc(visibleBool);
		

    }
    


    public void CallFadeFunc(bool FadeIn) {
        
	
		if (FadeCoroutine != null) {
            StopCoroutine(FadeCoroutine);
        }

        //pass parameters here
        FadeCoroutine = FadeFunction(1.0f, FadeIn);

        StartCoroutine(FadeCoroutine);
    }

    IEnumerator FadeFunction(float timer, bool FadeIn)
    {
        if (FadeIn == true)
        {
            MuscleRenderer.enabled = true;
        }

        Color StartColor = MuscleRenderer.material.color;
        Color EndColor = (FadeIn) ? new Color(1, 1, 1, alphaTarget) : new Color(1, 1, 1, 0);
        float t = 0.0f;

        while (t <= timer)
        {
          

            MuscleRenderer.material.color = Color.Lerp(StartColor, EndColor, t / timer);

            t += Time.deltaTime;
            yield return null;
        }

        MuscleRenderer.material.color = EndColor;

        if (FadeIn == false)
        {
            MuscleRenderer.enabled = false;
        }

        FadeCoroutine = null;
    }
	
	
	
}
