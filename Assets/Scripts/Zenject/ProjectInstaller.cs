using System;
using System.Collections;
using Project;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _controlsTypePrefab;

        public override void InstallBindings()
        {
            BindControls();
            BindControlsType();
        }
        
        private void BindControls()
        {
            Controls controls = new Controls();
            Container.Bind<Controls>().FromInstance(controls).AsSingle();
        }

        private void BindControlsType()
        {
            ControlsType controlsType = Container.InstantiatePrefabForComponent<ControlsType>(_controlsTypePrefab,
                Vector3.zero, Quaternion.identity, null).GetComponent<ControlsType>();
            
            Container.Bind<ControlsType>().FromInstance(controlsType).AsSingle();
        }
    }
}
