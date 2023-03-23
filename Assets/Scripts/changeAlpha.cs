using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeAlpha : MonoBehaviour {

    public float alphalevel;
    public Slider opacitySlider;
    public bool healthyLeg;
	private Renderer MeshRenderer;

	// Use this for initialization
	void Start () {
        //opacitySlider = GameObject("")
        alphalevel = 0.5f;
		MeshRenderer = GetComponent<Renderer>();
		
		 opacitySlider.onValueChanged.AddListener(delegate {
            UpdateAlpha(opacitySlider);
        });
    }
	




    public void UpdateAlpha( Slider target){
        alphalevel = target.value;
        //MeshRenderer.material.shader = Shader.Find("_BaseColor");
        MeshRenderer.material.EnableKeyword("_BaseColor");
        MeshRenderer.material.SetColor("_BaseColor", Color.red);

        //MeshRenderer.material.color = new Color(1, 1, 1, alphaTarget);
    }
}
