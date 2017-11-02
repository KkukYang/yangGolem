using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerScriptEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    Player player = (Player)target;

    //    //player.hp = EditorGUILayout.FloatField("hp", player.hp);
    //    EditorGUILayout.FloatField("hp", player.hp);
    //}

}

/*
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelScript myTarget = (LevelScript)target;

        myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
} 
 */
