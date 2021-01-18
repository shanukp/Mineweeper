// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/02/05 19:50

#if UNITY_EDITOR
using DG.Tweening.Core;
using UnityEditor;
using UnityEngine;

namespace DG.DOTweenEditor
{
    [CustomEditor(typeof(DOTweenSettings))]
    public class DOTweenSettingsInspector : Editor
    {
        override public void OnInspectorGUI()
        {
            GUI.enabled = false;

            DrawDefaultInspector();

            GUI.enabled = true;
        }
    }
}
#endif