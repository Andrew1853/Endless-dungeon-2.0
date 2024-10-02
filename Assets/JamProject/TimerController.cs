using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] TimerView _view;
        [SerializeField] GameManager _gameManager;

        float _currentTime = 0;
        bool _isActive = false;
        public bool side = true;
        private void Start()
        {
        }
        public void Activate(bool value)
        {
            _isActive = value == side;
        }
        private void Update()
        {
            if (_isActive)
            {
                _currentTime += Time.deltaTime;
                _view.UpdateTimerText(_currentTime);
            }
        }
        public void ResetTimer()
        {
            _currentTime = 0;
            _view.UpdateTimerText(_currentTime);
        }
    }
}