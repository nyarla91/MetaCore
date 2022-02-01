using Project;
using UnityEngine;

namespace Zenject
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _controlsTypePrefab;
        [SerializeField] private GameObject _musicPrefab;

        public override void InstallBindings()
        {
            BindControls();
            BindControlsType();
            Container.InstantiatePrefab(_musicPrefab, Vector3.zero, Quaternion.identity, null);
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
