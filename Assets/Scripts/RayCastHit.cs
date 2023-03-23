using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastHit : MonoBehaviour
{
    //[Header("SteamVR Action Settings")]
    //public SteamVR_Action_Boolean grabPinch;
    //SteamVR_Input_Sources InputSource;
    InteractableButton interactableButton;
    InteractableObject interactableObject;

    [Header("Pointer Visual Settings")]
    public Color PointerColor;
    public float PointerThickness;
    GameObject Point;
    GameObject Line;

    // Start is called before the first frame update
    void Awake()
    {
        //InputSource = this.gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;

        Point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Point.AddComponent<Rigidbody>();
        Point.GetComponent<Rigidbody>().isKinematic = true;
        Point.layer = 2;
        Point.transform.parent = this.transform;
        Point.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        Point.transform.localPosition = new Vector3(0f, 0f, 0f);
        Point.transform.localRotation = Quaternion.identity;
        Point.GetComponent<MeshRenderer>().material.color = PointerColor;

        Line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Line.transform.parent = Point.transform;
        Line.transform.parent = this.transform;
        Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, 100f);
        Line.transform.localPosition = new Vector3(0f, 0f, 50f);
        Line.transform.localRotation = Quaternion.identity;
        Line.GetComponent<MeshRenderer>().material.color = PointerColor;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.05f, out hit))
        {
            Point.transform.position = hit.point;
            CheckButton(hit);

            Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, hit.distance);
            Line.transform.parent = this.transform;
           // Line.transform.localPosition = new Vector3(0f, 0f, hit.distance);
            //Line.GetComponent<MeshRenderer>().material.color = PointerColor;

            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log("Hit collider");
        }
        else
        {
            //grabPinch.RemoveOnStateDownListener(OnTriggerPressed, InputSource);
            interactableButton = null;

            DrawLaser();
        }
    }

    void DrawLaser()
    {
        Line.transform.parent = this.transform;
        Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, 100f);
        Line.transform.localPosition = new Vector3(0f, 0f, 50f);
        Line.transform.localRotation = Quaternion.identity;
        Line.GetComponent<MeshRenderer>().material.color = PointerColor;

    }

    void CheckButton(RaycastHit hit)
    {
        InteractableButton tempButton = hit.collider.gameObject.GetComponent<InteractableButton>();
        InteractableObject tempObject = hit.collider.gameObject.GetComponent<InteractableObject>();

        if (tempObject == null)
        {
            //grabPinch.RemoveOnStateDownListener(OnTriggerPressed, InputSource);
            interactableObject = null;
        } 
        else if (tempObject != null && interactableObject != tempObject)
        {
            interactableObject = tempObject;
            //grabPinch.AddOnStateDownListener(OnTriggerPressed, InputSource);
        }
    }

    /*private void OnTriggerPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources input)
    {
        interactableObject.CallOnClickInteractable();

        //know this isnt right, just testing
        //interactableMesh.DoYourThing();

        Debug.Log("Trigger was pressed or released");
    }*/
}

/*using Valve.VR;

public class RayCastHit : MonoBehaviour
{
    [Header("SteamVR Action Settings")]
    public SteamVR_Action_Boolean grabPinch;
    SteamVR_Input_Sources InputSource;
    InteractableButton interactableButton;
    InteractableObject interactableObject;

    [Header("Pointer Visual Settings")]
    public Color PointerColor;
    public float PointerThickness;
    GameObject Point;
    GameObject Line;

    // Start is called before the first frame update
    void Awake()
    {
        InputSource = this.gameObject.GetComponent<SteamVR_Behaviour_Pose>().inputSource;

        Point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Point.AddComponent<Rigidbody>();
        Point.GetComponent<Rigidbody>().isKinematic = true;
        Point.layer = 2;
        Point.transform.parent = this.transform;
        Point.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        Point.transform.localPosition = new Vector3(0f, 0f, 0f);
        Point.transform.localRotation = Quaternion.identity;
        Point.GetComponent<MeshRenderer>().material.color = PointerColor;

        Line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Line.transform.parent = Point.transform;
        Line.transform.parent = this.transform;
        Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, 100f);
        Line.transform.localPosition = new Vector3(0f, 0f, 50f);
        Line.transform.localRotation = Quaternion.identity;
        Line.GetComponent<MeshRenderer>().material.color = PointerColor;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.05f, out hit))
        {
            Point.transform.position = hit.point;
            CheckButton(hit);

            Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, hit.distance);
            Line.transform.parent = this.transform;
           // Line.transform.localPosition = new Vector3(0f, 0f, hit.distance);
            //Line.GetComponent<MeshRenderer>().material.color = PointerColor;

            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log("Hit collider");
        }
        else
        {
            grabPinch.RemoveOnStateDownListener(OnTriggerPressed, InputSource);
            interactableButton = null;

            DrawLaser();
        }
    }

    void DrawLaser()
    {
        Line.transform.parent = this.transform;
        Line.transform.localScale = new Vector3(PointerThickness, PointerThickness, 100f);
        Line.transform.localPosition = new Vector3(0f, 0f, 50f);
        Line.transform.localRotation = Quaternion.identity;
        Line.GetComponent<MeshRenderer>().material.color = PointerColor;

    }

    void CheckButton(RaycastHit hit)
    {
        InteractableButton tempButton = hit.collider.gameObject.GetComponent<InteractableButton>();
        InteractableObject tempObject = hit.collider.gameObject.GetComponent<InteractableObject>();

        if (tempObject == null)
        {
            grabPinch.RemoveOnStateDownListener(OnTriggerPressed, InputSource);
            interactableObject = null;
        } 
        else if (tempObject != null && interactableObject != tempObject)
        {
            interactableObject = tempObject;
            grabPinch.AddOnStateDownListener(OnTriggerPressed, InputSource);
        }
    }

    private void OnTriggerPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources input)
    {
        interactableObject.CallOnClickInteractable();

        //know this isnt right, just testing
        //interactableMesh.DoYourThing();

        Debug.Log("Trigger was pressed or released");
    }
}
*/