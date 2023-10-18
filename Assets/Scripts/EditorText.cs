using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorText : MonoBehaviour
{
    public string labelText = "Hello World!";
    public Color labelColor = Color.white;
    public Vector3 labelOffset = Vector3.zero;
    public int fontSize = 12;

    void OnDrawGizmos()
    {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = labelColor;
        labelStyle.fontSize = fontSize;
        Handles.Label(transform.position + labelOffset, labelText, labelStyle);
    }
}