//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SO_Item), true)] // true để hỗ trợ cả class con
//public class SO_ItemEditor : Editor
//{
//    SerializedProperty commonData;
//    SerializedProperty itemID, itemName, icon, description;
//    SerializedProperty itemType, isStackable, maxQuantityAllowed, price;

//    void OnEnable()
//    {
//        commonData = serializedObject.FindProperty("commonData");
//        itemID = commonData.FindPropertyRelative("itemID");
//        itemName = commonData.FindPropertyRelative("itemName");
//        icon = commonData.FindPropertyRelative("icon");
//        description = commonData.FindPropertyRelative("description");
//        itemType = commonData.FindPropertyRelative("itemType");
//        isStackable = commonData.FindPropertyRelative("isStackable");
//        maxQuantityAllowed = commonData.FindPropertyRelative("maxQuantityAllowed");
//        price = commonData.FindPropertyRelative("price");

//        // Set itemType theo loại class tự động (nếu là lần đầu)
//        SO_Item so = (SO_Item)target;
//        if (so.commonData.itemType == default)
//        {
//            if (so is SO_Outfit)
//            {

//            }
//                //so.commonData.itemType = ItemType.Outfit;
//            else if (so is SO_Consumable)
//                so.commonData.itemType = ItemType.Consumable;

//            EditorUtility.SetDirty(so); // Đảm bảo nó lưu
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        EditorGUILayout.PropertyField(itemID);
//        EditorGUILayout.PropertyField(itemName);
//        EditorGUILayout.PropertyField(icon);
//        EditorGUILayout.PropertyField(description);
//        EditorGUILayout.PropertyField(itemType);
//        EditorGUILayout.PropertyField(isStackable);

//        if (isStackable.boolValue)
//        {
//            EditorGUILayout.PropertyField(maxQuantityAllowed);
//        }

//        EditorGUILayout.PropertyField(price);

//        // Vẽ phần riêng của từng loại SO con
//        DrawPropertiesExcluding(serializedObject, "commonData");

//        serializedObject.ApplyModifiedProperties();
//    }
//}