using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardDataSO), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    private BoardDataSO GameDataInstane => target as BoardDataSO;
    private ReorderableList _dataList;

    private void OnEnable() {
        InitializeReorderableList(ref _dataList, "SearchWords", "Searching Words");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        GameDataInstane.timeInSeconds = EditorGUILayout.IntField("Max Game Time (seconds)", GameDataInstane.timeInSeconds);

        DrawInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();

        if(GameDataInstane.Board != null && GameDataInstane.Columns > 0 && GameDataInstane.Rows > 0) {
            DrawBoardTable();
        }

        EditorGUILayout.BeginHorizontal();

        ClearBoardButton();
        FillUpWithRamdomLetterButton();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed) {
            EditorUtility.SetDirty(GameDataInstane);
        }
    }

    private void DrawInputFields() {
        var columnsTemp = GameDataInstane.Columns;
        var rowsTemp = GameDataInstane.Rows;

        GameDataInstane.Columns = EditorGUILayout.IntField("Columns", GameDataInstane.Columns);
        GameDataInstane.Rows = EditorGUILayout.IntField("Rows", GameDataInstane.Rows);

        if ((GameDataInstane.Columns != columnsTemp || GameDataInstane.Rows != rowsTemp)
            && GameDataInstane.Columns > 0 && GameDataInstane.Rows > 0) {
            GameDataInstane.CreateBoard();
        }
    }
   
    private void DrawBoardTable() {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var textFieldStyle = new GUIStyle();

        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for ( int x = 0; x < GameDataInstane.Columns; x++ ) {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for (int y = 0; y < GameDataInstane.Rows; y++ ) {
                if (x >= 0 && y >= 0) {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstane.Board[x].Row[y], textFieldStyle);
                    if (GameDataInstane.Board[x].Row[y].Length > 1) { 
                        character = GameDataInstane.Board[x].Row[y].Substring(0, 1);
                    }

                    GameDataInstane.Board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void InitializeReorderableList( ref ReorderableList list, string propertyName, string listLabel) {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Word"), GUIContent.none);
        };
    }

    private void ConvertToUpperButton() {
        if (GUILayout.Button("To Upper")) {
            for (int i = 0; i < GameDataInstane.Columns; i++) {
                for(int j = 0; j < GameDataInstane.Rows; j++) {
                    var errorCounter = Regex.Matches(GameDataInstane.Board[i].Row[j], @"[a-z]").Count;

                    if (errorCounter > 0) {
                        GameDataInstane.Board[i].Row[j] = GameDataInstane.Board[i].Row[j].ToUpper();
                    }
                }
            }

            foreach(var searchWord in GameDataInstane.SearchWords) {
                var errorCounter = Regex.Matches(searchWord.Word, @"[a-z]").Count;

                if (errorCounter > 0) {
                    searchWord.Word = searchWord.Word.ToUpper();
                }
            }
        }
    }

    private void ClearBoardButton() {
        if(GUILayout.Button("Clear Board")) {
            for (int i = 0;i < GameDataInstane.Columns; i++) {
                for (int j = 0; j < GameDataInstane.Rows; j++) {
                    GameDataInstane.Board[i].Row[j] = " ";
                }
            }
        }
    }

    private void FillUpWithRamdomLetterButton() {
        if (GUILayout.Button("Fill Up With Random")) {
            for (int i = 0; i < GameDataInstane.Columns; i++) {
                for (int j = 0; j < GameDataInstane.Rows; j++) {
                    int errorCounter = Regex.Matches(GameDataInstane.Board[i].Row[j], @"[a-zA-Z]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = UnityEngine.Random.Range(0, letters.Length);

                    if(errorCounter == 0) {
                        GameDataInstane.Board[i].Row[j] = letters[index].ToString();
                    }
                }
            }
        }
    }
}
