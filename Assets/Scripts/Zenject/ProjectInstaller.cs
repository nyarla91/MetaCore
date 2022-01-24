using System;
using System.Collections;
using Project;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _controlsTypePrefab;
        [SerializeField] private GameObject _tutorialsPrefab;

        public override void InstallBindings()
        {
            BindControls();
            BindControlsType();
            BindTutorials();
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

        private void BindTutorials()
        {
            Tutorials tutorials = Container.InstantiatePrefabForComponent<Tutorials>(_tutorialsPrefab,
                Vector3.zero, Quaternion.identity, null).GetComponent<Tutorials>();
            
            Container.Bind<Tutorials>().FromInstance(tutorials).AsSingle();
        }
    }
}
