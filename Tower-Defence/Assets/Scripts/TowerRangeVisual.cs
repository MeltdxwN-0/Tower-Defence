using UnityEngine;

public class TowerRangeVisual : MonoBehaviour
{
    [SerializeField] private int segments = 80;
    [SerializeField] private float lineWidth = 0.04f;
    [SerializeField] private Color rangeColor = new Color(1f, 1f, 1f, 0.35f);

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = rangeColor;
        lineRenderer.endColor = rangeColor;

        Hide();
    }

    public void SetRange(float range)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / segments) * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * range;
            float y = Mathf.Sin(angle) * range;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    public void Show()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = true;
    }

    public void Hide()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }
}
