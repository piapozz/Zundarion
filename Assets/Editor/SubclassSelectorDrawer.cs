#if UNITY_2019_3_OR_NEWER

using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    private bool initialized = false;
    private Type[] inheritedTypes;
    private string[] typePopupNameArray;
    private string[] typeFullNameArray;
    private int currentTypeIndex;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.LabelField(position, label.text, "Use with ManagedReference only.");
            return;
        }

        if (!initialized || property.serializedObject.targetObject == null)
        {
            Initialize(property);
            GetCurrentTypeIndex(property.managedReferenceFullTypename);
            initialized = true;
        }

        EditorGUI.BeginProperty(position, label, property);

        // ドロップダウンメニューで型選択
        int selectedTypeIndex = EditorGUI.Popup(GetPopupPosition(position), currentTypeIndex, typePopupNameArray);
        if (selectedTypeIndex != currentTypeIndex)
        {
            UpdatePropertyToSelectedTypeIndex(property, selectedTypeIndex);
        }

        // 通常のプロパティフィールド描画
        EditorGUI.PropertyField(position, property, label, true);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }

    private void Initialize(SerializedProperty property)
    {
        try
        {
            SubclassSelectorAttribute utility = (SubclassSelectorAttribute)attribute;
            GetAllInheritedTypes(GetType(property), utility.IsIncludeMono());
            GetInheritedTypeNameArrays();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error initializing SubclassSelector: {ex.Message}");
            inheritedTypes = new Type[0];
            typePopupNameArray = new string[] { "<Error>" };
            typeFullNameArray = new string[] { "" };
        }
    }

    private void GetCurrentTypeIndex(string typeFullName)
    {
        currentTypeIndex = Array.IndexOf(typeFullNameArray, typeFullName);
        if (currentTypeIndex == -1)
            currentTypeIndex = 0; // <null>にフォールバック
    }

    private void GetAllInheritedTypes(Type baseType, bool includeMono)
    {
        try
        {
            Type monoType = typeof(ScriptableObject);
            inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && type.IsClass && (!monoType.IsAssignableFrom(type) || includeMono))
                .Prepend(null) // <null>オプションを追加
                .ToArray();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error retrieving inherited types: {ex.Message}");
            inheritedTypes = new Type[0];
        }
    }

    private void GetInheritedTypeNameArrays()
    {
        typePopupNameArray = inheritedTypes.Select(type => type == null ? "<null>" : type.Name).ToArray();
        typeFullNameArray = inheritedTypes.Select(type => type == null ? "" : type.FullName).ToArray();
    }

    private void UpdatePropertyToSelectedTypeIndex(SerializedProperty property, int selectedTypeIndex)
    {
        currentTypeIndex = selectedTypeIndex;
        Type selectedType = inheritedTypes[selectedTypeIndex];

        if (selectedType == null)
        {
            property.managedReferenceValue = null;
        }
        else if (typeof(ScriptableObject).IsAssignableFrom(selectedType))
        {
            property.managedReferenceValue = SelectOrCreateScriptableObject(property, selectedType);
        }
        else
        {
            property.managedReferenceValue = Activator.CreateInstance(selectedType);
        }
    }

    private ScriptableObject SelectOrCreateScriptableObject(SerializedProperty property, Type selectedType)
    {
        string[] guids = AssetDatabase.FindAssets($"t:{selectedType.Name}");
        ScriptableObject[] foundAssets = guids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToArray();

        string[] options = foundAssets.Select(asset => asset.name).Prepend("Create New").ToArray();
        int selectedIndex = EditorGUILayout.Popup("Select Asset", 0, options);

        if (selectedIndex == 0)
        {
            ScriptableObject newAsset = ScriptableObject.CreateInstance(selectedType);
            string path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableObject",
                selectedType.Name,
                "asset",
                "Select save location for the ScriptableObject"
            );

            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(newAsset, path);
                AssetDatabase.SaveAssets();
                return newAsset;
            }
        }
        else if (selectedIndex > 0)
        {
            return foundAssets[selectedIndex - 1];
        }

        return null;
    }

    private Rect GetPopupPosition(Rect currentPosition)
    {
        Rect popupPosition = new Rect(currentPosition);
        popupPosition.width -= EditorGUIUtility.labelWidth;
        popupPosition.x += EditorGUIUtility.labelWidth;
        popupPosition.height = EditorGUIUtility.singleLineHeight;
        return popupPosition;
    }

    public static Type GetType(SerializedProperty property)
    {
        const BindingFlags bindingAttr =
            BindingFlags.NonPublic |
            BindingFlags.Public |
            BindingFlags.FlattenHierarchy |
            BindingFlags.Instance;

        try
        {
            var propertyPaths = property.propertyPath.Split('.');
            var parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = parentType.GetField(propertyPaths[0], bindingAttr);

            foreach (string path in propertyPaths.Skip(1))
            {
                if (path == "Array")
                    continue;

                fieldInfo = fieldInfo.FieldType.GetField(path, bindingAttr);
            }

            return fieldInfo?.FieldType ?? typeof(object);
        }
        catch
        {
            return typeof(object);
        }
    }
}
#endif
