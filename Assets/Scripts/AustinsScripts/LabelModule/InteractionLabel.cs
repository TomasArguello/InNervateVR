using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class InteractionLabel : GeneralInteraction
{
    [Header("Anatomy Settings")]
    [SerializeField] LabelCategory AnatomyLabelCategory;
    [SerializeField] MeshRenderer MeshLabel;
    [Header("Mesh Interaction Settings")]
    [SerializeField] Color NormalMeshEmissionColor;
    [SerializeField] Color SelectMeshEmissionColor;
    [Header("UI Settings")]
    [SerializeField] string LabelTitle;
    [SerializeField] [TextArea(2, 4)] string LabelDescription;

    public string GetLabelTitle { get { return LabelTitle; } }
    public string GetLabelDescription { get { return LabelDescription; } }
    public LabelCategory GetLabelCategory { get { return AnatomyLabelCategory; } }
    private LineRenderer LabelLine;

    new void Awake()
    {
        base.Awake();
        LabelLine = GetComponent<LineRenderer>();
        UpdateLineRendererPositions();
        LabelLine.enabled = false;
    }

    public void UpdateLineRendererPositions()
    {
        Vector3[] LinePositions = new Vector3[] { transform.position, transform.GetChild(0).transform.position };

        if (LabelLine == null)
            LabelLine = GetComponent<LineRenderer>();

        LabelLine.SetPositions(LinePositions);
    }

    // This is triggered by the InteractionLabelManager. Not by the EventTrigger component
    public override void UnSelect()
    {
        base.UnSelect();
        MeshLabel.material.SetColor("_EmissionColor", NormalMeshEmissionColor);
        LabelLine.enabled = false;
    }

    public override void Select(PointerEventData data)
    {
        base.Select(data);
        MeshLabel.material.SetColor("_EmissionColor", SelectMeshEmissionColor);
        ScriptManager.Instance.LabelManager.ChangeLabel(this);
    }

    public override void Hover(PointerEventData data)
    {
        if (!IsSelected)
        {
            InteractableMesh.material.SetColor("_EmissionColor", HighlightedEmissionColor);
            LabelLine.enabled = true;
        }
    }

    public override void UnHover(PointerEventData data)
    {
        if (!IsSelected)
        {
            InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
            LabelLine.enabled = false;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Vector3 end_point = transform.GetChild(0).transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(end_point, 0.008f);
    }
}