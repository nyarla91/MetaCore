using Player;
using UI;
using UnityEngine;
using View;

namespace Zenject
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerMarkerPrefab;
        [SerializeField] private CameraMouseCast _cameraMouseCast;
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private PlayerUICluster _playerUICluster;
        [SerializeField] private MessageWindow messageWindow;

        public override void InstallBindings()
        {
            Container.Bind<CameraMouseCast>().FromInstance(_cameraMouseCast).AsSingle();
            Container.Bind<GameplayInstaller>().FromInstance(this).AsSingle();
            Container.Bind<CameraView>().FromInstance(_cameraView).AsSingle();
            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            Container.Bind<PlayerUICluster>().FromInstance(_playerUICluster).AsSingle();
            Container.Bind<MessageWindow>().FromInstance(messageWindow).AsSingle();
            BindPlayerMarker();
        }

        private void BindPlayerMarker()
        {
            PlayerMarker playerMarker = Container.InstantiatePrefabForComponent<PlayerMarker>(
                _playerMarkerPrefab, Vector3.zero, Quaternion.identity, null);

            Container.Bind<PlayerMarker>().FromInstance(playerMarker).AsSingle();
        }
    }   
}
