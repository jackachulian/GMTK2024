#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// sets the phys mat of all colliders in scene
public class SetPhysMat : MonoBehaviour
{
    [SerializeField] private PhysicMaterial physMat;
    public void ReplaceAll()
    {
        var colliders = FindObjectsOfType<Collider>();

        foreach (var collider in colliders)
        {
            collider.sharedMaterial = physMat;
        }

        Debug.Log("All physic materials set");
    }

}

[CustomEditor(typeof(SetPhysMat))]
public class SetPhysMatEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        SetPhysMat scriptTarget = (SetPhysMat)target;

        if(GUILayout.Button("Replace All"))
            Debug.Log("bruh");
            scriptTarget.ReplaceAll();
    }
}
#endif