using DG.Tweening;
using UnityEngine;

namespace Sources.Runtime
{
    public class CameraZoomer : MonoBehaviour
    {
        [SerializeField]
        private float _zoomDuration = 1;
        private CharacterPresentersBank _bank;

        private Camera _camera;
        private Vector3 _cameraOriginPos;

        private void Awake()
        {
            _bank = FindObjectOfType<CharacterPresentersBank>();
            _camera = Camera.main;
            _cameraOriginPos = _camera.transform.position;
            
            foreach (var characterPresenter in _bank.AllCharacters)
            {
                characterPresenter.Model.Attacking +=
                    _ => CameraZoom(characterPresenter.Model);
            }
        }

        private void CameraZoom(Character attacker)
        {
            var attackerPresenter = _bank.GetPresenterByModel(attacker);

            _camera.transform.DOKill();
            var tween = _camera.transform.DOMoveY(transform.position.y, _zoomDuration);
            tween.onUpdate += () =>
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 3.5f, tween.fullPosition);

            attackerPresenter.Model.Attacked += () =>
            {
                _camera.transform.DOKill();
                var tween2 = _camera.transform.DOMove(_cameraOriginPos, _zoomDuration / 2);
                tween2.onUpdate += () =>
                    _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 5, tween2.fullPosition);
            };
        }
    }
}