using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class RenameHandBones : EditorWindow
{
    public Transform handRoot;
    public bool isLeftHand = true;

    private static readonly string[] BoneNames = new string[]
    {
        "Wrist",              // [0]  Bone

        "IndexMetacarpal",    // [1]  Bone.001
        "IndexProximal",      // [2]  Bone.002
        "IndexIntermediate",  // [3]  Bone.003
        "IndexTip",           // [4]  Bone.003_end  (no Distal in this rig)

        "LittleMetacarpal",   // [5]  Bone.004
        "LittleProximal",     // [6]  Bone.005
        "LittleIntermediate", // [7]  Bone.006
        "LittleDistal",       // [8]  Bone.007
        "LittleTip",          // [9]  Bone.007_end

        "MiddleMetacarpal",   // [10] Bone.008
        "MiddleProximal",     // [11] Bone.009
        "MiddleIntermediate", // [12] Bone.010
        "MiddleDistal",       // [13] Bone.011
        "MiddleTip",          // [14] Bone.011_end

        "RingMetacarpal",     // [15] Bone.012
        "RingProximal",       // [16] Bone.013
        "RingIntermediate",   // [17] Bone.014
        "RingDistal",         // [18] Bone.015
        "RingTip",            // [19] Bone.015_end

        "ThumbMetacarpal",    // [20] Bone.016
        "ThumbProximal",      // [21] Bone.017
        "ThumbDistal",        // [22] Bone.018
        "ThumbTip",           // [23] Bone.019
        // [24] Bone.019_end  → intentionally unmapped (no Palm bone)
    };

    [MenuItem("Tools/Rename Hand Bones")]
    public static void ShowWindow()
    {
        GetWindow<RenameHandBones>("Rename Hand Bones");
    }

    void OnGUI()
    {
        GUILayout.Label("Hand Bone Renamer", EditorStyles.boldLabel);
        handRoot = (Transform)EditorGUILayout.ObjectField(
            "Hand Root (Armature)", handRoot, typeof(Transform), true);
        isLeftHand = EditorGUILayout.Toggle("Is Left Hand", isLeftHand);

        if (GUILayout.Button("List Bones (Debug)"))
        {
            if (handRoot == null) { Debug.LogError("Assign a root!"); return; }
            ListBones();
        }

        if (GUILayout.Button("Rename Bones"))
        {
            if (handRoot == null) { Debug.LogError("Assign a root!"); return; }
            RenameBones();
        }
    }

    List<Transform> GetSortedBones()
    {
        Transform[] all = handRoot.GetComponentsInChildren<Transform>(true);
        var filtered = new List<Transform>();

        foreach (var t in all)
        {
            string n = t.name;
            // Match "Bone", "Bone.001", AND "Bone.003_end"
            if (n == "Bone" || Regex.IsMatch(n, @"^Bone(\.\d+)?(_end)?$"))
                filtered.Add(t);
        }

        filtered.Sort((a, b) => GetBoneNumber(a.name).CompareTo(GetBoneNumber(b.name)));
        return filtered;
    }
    
    
    int GetBoneNumber(string name)
    {
        if (name == "Bone") return 0;

        // Extract the numeric part — works for both "Bone.007" and "Bone.007_end"
        var match = Regex.Match(name, @"\d+");
        if (!match.Success) return 0;

        int num = int.Parse(match.Value);

        // "_end" bones sort AFTER their parent (e.g. Bone.007_end comes after Bone.007)
        // We add 0.5 effectively by using a secondary sort signal
        return name.EndsWith("_end") ? num * 10 + 5 : num * 10;
    }
    void ListBones()
    {
        var bones = GetSortedBones();
        Debug.Log($"Found {bones.Count} bones:");
        for (int i = 0; i < bones.Count; i++)
            Debug.Log($"  [{i}] {bones[i].name}  →  would become: " +
                      (i < BoneNames.Length ? (isLeftHand ? "L_" : "R_") + BoneNames[i] : "NO MAPPING"));
    }

    void RenameBones()
    {
        string prefix = isLeftHand ? "L_" : "R_";
        var bones = GetSortedBones();

        Debug.Log($"Renaming {bones.Count} bones...");

        for (int i = 0; i < bones.Count && i < BoneNames.Length; i++)
        {
            string newName = prefix + BoneNames[i];
            Debug.Log($"  {bones[i].name} → {newName}");
            Undo.RecordObject(bones[i].gameObject, "Rename Bone");
            bones[i].name = newName;
        }

        Debug.Log("✅ Done!");
    }
}