using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace NyarlaEssentials.Localization
{
    public class LocalizedTextMesh : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private TextMeshProUGUI _textMeshGUI;
        [SerializeField, TextArea(3, 10)] private string[] _text;

        private void Start()
        {
            string localizedText = Localization.Translate(_text);
            if (_textMesh != null)
                _textMesh.text = localizedText;
            if (_textMeshGUI != null)
                _textMeshGUI.text = localizedText;

        }
    }

}