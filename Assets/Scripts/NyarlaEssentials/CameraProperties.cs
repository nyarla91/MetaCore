using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials
{
    [RequireComponent(typeof(Camera))]
    public class CameraProperties : Transformer
    {
        private static CameraProperties _instance;
        public static CameraProperties Instance => _instance;
        
        private static Camera _main;
        public static Camera Main => _main ??= Camera.main;

        public static Vector3 MousePosition2D => Main.ScreenToWorldPoint(Input.mousePosition);

        public static float YRotation => _instance.transform.rotation.eulerAngles.y;
        

        public static Vector3 PercentToScreenPoint(Vector2 percent)
        {
            percent += Vector2.one;
            percent /= 2;
            return new Vector2(Screen.width, Screen.height) * percent;
        }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _main = null;
        }
    }
}
