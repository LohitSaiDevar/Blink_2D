using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Readme))]
public class ReadmeEditor : Editor
{
    private GUIStyle titleStyle;
    private GUIStyle descriptionStyle;
    private GUIStyle keyStyle;
    private GUIStyle actionStyle;


    // Toggle for the Designer
    private bool isEditing = false;
    private void InitStyles()
    {
        if (titleStyle != null) return;

        titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 26,
            alignment = TextAnchor.MiddleCenter,
            margin = new RectOffset(0, 0, 20, 10)
        };

        descriptionStyle = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true,
            fontSize = 14,
            fontStyle = FontStyle.Italic,
            alignment = TextAnchor.MiddleCenter
        };

        // This makes the Key look like a keyboard button
        keyStyle = new GUIStyle(EditorStyles.helpBox)
        {
            fontSize = 11,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            fixedWidth = 100,
            normal = { textColor = Color.white }
        };

        actionStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold
        };
    }

    public override void OnInspectorGUI()
    {
        Readme readme = target as Readme;
        InitStyles();

        // --- READ ONLY VIEW ---

        EditorGUILayout.LabelField(readme.Title, titleStyle);

        if (!string.IsNullOrEmpty(readme.Description))
        {
            EditorGUILayout.LabelField(readme.Description, descriptionStyle);
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("CONTROLS", EditorStyles.whiteBoldLabel);

        EditorGUILayout.BeginVertical("box");
        foreach (var ctrl in readme.Controls)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(ctrl.Action, actionStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label(ctrl.Key.ToUpper(), keyStyle);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // --- THE "EDIT MODE" TOGGLE ---

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // A small button to toggle editing
        string btnText = isEditing ? "✔ Finish Editing" : "⚙ Edit Content";
        if (GUILayout.Button(btnText, GUILayout.Width(120)))
        {
            isEditing = !isEditing;
        }

        EditorGUILayout.EndHorizontal();

        // Only show the raw fields if isEditing is true
        if (isEditing)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("Editing Mode Enabled. You can modify Title, Description, and Controls below.", MessageType.Info);
            base.OnInspectorGUI(); // Draws the original editable list
        }
    }
}
