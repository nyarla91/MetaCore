using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Tutorial
{
    public class Tutorials : MonoBehaviour
    {
        public const int MechanicTutorialsTotal = 3;
        public static int MechanicTutorialProgress { get; set; }

        public static List<ConsumableType> ConsumableTutorialsSeen { get; } = new List<ConsumableType>();

        [SerializeField] private List<Page> _pages;

        private readonly Dictionary<string, Page> _pagesDictionary = new Dictionary<string, Page>();
        

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            foreach (var page in _pages)
            {
                _pagesDictionary.Add(page.Name, page);
            }
        }

        public Page GetPage(string name)
        {
            return _pagesDictionary.ContainsKey(name) ? _pagesDictionary[name] : null;
        }
    }
    
    [Serializable]
    public class Page
    {
        [SerializeField] private string _name;
        [SerializeField] private string _label;
        [SerializeField] private Sprite _illustration;
        [SerializeField] [TextArea(5, 999)] private string _tutorialText;

        public string Name => _name;
        public Sprite Illustration => _illustration;
        public string TutorialText => _tutorialText;
        public string Label => _label;
    }
}