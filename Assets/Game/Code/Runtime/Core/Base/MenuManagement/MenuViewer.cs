using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.Base.MenuManagement
{
    /// <summary>
    /// A wrapper class for the abstract class <see cref="MenuViewer{T}"/>, which does
    /// not restrict the use of certain classes inherited from Menu.
    /// </summary>
    [AddComponentMenu("ShizoGames/UI/Menu Management/Default Menu Viewer", 38)]
    public sealed class MenuViewer : MenuViewer<Menu>
    {
    }
    
    /// <summary>
    /// This is an abstract class that provides a menu system. It is designed to manage multiple menus
    /// and their interactions with each other. The class takes in a generic type parameter, which should be the base class
    /// for all menus. The class provides methods to open and close menus, to keep track of a menu history,
    /// and to query the current menu. This class inherits from MonoBehaviour class.
    /// </summary>
    /// <typeparam name="T">The base type for menus.</typeparam>
    public abstract class MenuViewer<T> : MonoBehaviour where T : Menu
    {
        [Tooltip("The Canvas used to display the menus. If empty, it will create its own Canvas")]
        [SerializeField] private Canvas _menuCanvas;
        
        [Tooltip("The sorting order of the menu canvas.")]
        [SerializeField] private int _sortingOrder;
        
        [Tooltip("The starting menu. Started on initialization.")]
        [SerializeField] private T _startingMenu;
        
        [Tooltip("The list of available menus for switching.")]
        [SerializeField, Space] private T[] _menus = Array.Empty<T>();
        
        private readonly Dictionary<Type, T> _menuInstances = new Dictionary<Type, T>();
        private readonly Stack<T> _menuHistory = new Stack<T>();
        private T _currentMenu;
        
        /// <summary>
        /// Returns true if the menu history stack is empty, false otherwise.
        /// </summary>
        public bool HistoryIsEmpty => _menuHistory.Count == 0;
        
        /// <summary>
        /// Returns true if the menu history stack is not empty, false otherwise.
        /// </summary>
        public bool HistoryIsNotEmpty => _menuHistory.Count > 0;

        private void Awake()
        {
            if (!_menuCanvas)
            {
                _menuCanvas = new GameObject("Menu Canvas").AddComponent<Canvas>();
                _menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            
            _menuCanvas.sortingOrder = _sortingOrder;
        }
        
        private void Start()
        {
            if (_startingMenu)
            {
                OpenMenu(_startingMenu);
            }
        }
        
        /// <summary>
        /// Opens the menu of the specified type.
        /// </summary>
        /// <typeparam name="TMenu">The type of the menu to open.</typeparam>
        /// <param name="remember">If true, adds to history.</param>
        /// <param name="closeLastMenu">If true, closes last menu.</param>
        /// <param name="destroy">If true, destroys the closed menus.</param>
        public void OpenMenu<TMenu>(bool remember = true, bool closeLastMenu = true, bool destroy = false) where TMenu : T
        {
            OpenMenu(typeof(TMenu), remember, closeLastMenu, destroy);
        }
        
        /// <summary>
        /// Opens the specified menu.
        /// </summary>
        /// <param name="menu">The menu to open.</param>
        /// <param name="remember">If true, adds to history.</param>
        /// <param name="closeLastMenu">If true, closes last menu.</param>
        /// <param name="destroy">If true, destroys the closed menus.</param>
        public void OpenMenu(Menu menu, bool remember = true, bool closeLastMenu = true, bool destroy = false)
        {
            OpenMenu(menu.GetType(), remember, closeLastMenu, destroy);
        }
        
        /// <summary>
        /// Opens the menu of the specified type.
        /// </summary>
        /// <param name="type">The type of the menu to open.</param>
        /// <param name="remember">If true, adds to history.</param>
        /// <param name="closeLastMenu">If true, closes last menu.</param>
        /// <param name="destroy">If true, destroys the closed menus.</param>
        public void OpenMenu(Type type, bool remember = true, bool closeLastMenu = true, bool destroy = false)
        {
            foreach (var menu in _menus)
            {
                if (menu.GetType() != type) continue;

                if (_currentMenu)
                {
                    if (remember)
                    {
                        _menuHistory.Push(_currentMenu);
                    }

                    if (closeLastMenu)
                    {
                        CloseCurrentMenu(destroy);
                    }
                }
                
                ShowOrInstantiate(menu);
            }
        }
        
        public void OpenStartingMenu()
        {
            OpenMenu(_startingMenu ? _startingMenu : _menus[0]);
        }
        
        /// <summary>
        /// Opens the last menu in the menu history and closes the current menu.
        /// </summary>
        /// <param name="closeIfCurrentMenuIsLast">If true, closes the current menu if it is the last in the history.</param>
        /// <param name="destroy">If true, destroys the closed menus.</param>
        public void OpenLastMenu(bool closeIfCurrentMenuIsLast = false, bool destroy = false)
        {
            if (HistoryIsNotEmpty)
            {
                OpenMenu(_menuHistory.Pop(), false);
            }
            else
            {
                if (closeIfCurrentMenuIsLast)
                {
                    CloseCurrentMenu(destroy);
                }
            }
        }
        
        /// <summary>
        /// Closes the menu of the specified type.
        /// </summary>
        /// <typeparam name="TMenu">The type of the menu to close.</typeparam>
        /// <param name="clearHistory">If true, the menu history will be cleared.</param>
        /// <param name="destroy">If true, destroys the closed menu.</param>
        public void CloseMenu<TMenu>(bool clearHistory = false, bool destroy = false) where TMenu : T
        {
            CloseMenu(typeof(TMenu), clearHistory, destroy);
        }
        
        /// <summary>
        /// Closes the specified menu.
        /// </summary>
        /// <param name="menu">The menu to close.</param>
        /// <param name="clearHistory">If true, the menu history will be cleared.</param>
        /// <param name="destroy">If true, destroys the closed menu.</param>
        public void CloseMenu(Menu menu, bool clearHistory = false, bool destroy = false)
        {
            CloseMenu(menu.GetType(), clearHistory, destroy);
        }
        
        /// <summary>
        /// Closes the menu of the specified type.
        /// </summary>
        /// <param name="type">The type of the menu to close.</param>
        /// <param name="clearHistory">If true, the menu history will be cleared.</param>
        /// <param name="destroy">If true, destroys the closed menu.</param>
        public void CloseMenu(Type type, bool clearHistory = false, bool destroy = false)
        {
            if (!_menuInstances.TryGetValue(type, out var menu)) return;
            
            menu.Close();
            
            if (destroy)
            {
                Destroy(menu.gameObject);
                
                _menuInstances.Remove(menu.GetType());
            }
            
            if (clearHistory)
            {
                _menuHistory.Clear();
            }
        }

        /// <summary>
        /// Closes the current menu.
        /// </summary>
        /// <param name="clearHistory">If true, the menu history will be cleared.</param>
        /// <param name="destroy">If true, destroys the closed menu.</param>
        public void CloseCurrentMenu(bool clearHistory = false, bool destroy = false)
        {
            if (!_currentMenu) return;
            
            CloseMenu(_currentMenu, clearHistory, destroy);
            
            _currentMenu = null;
        }
        
        /// <summary>
        /// Closes all menus.
        /// </summary>
        /// <param name="destroy">If true, destroys the closed menus.</param>
        public void CloseAllMenus(bool destroy = false)
        {
            var menus = _menuInstances.Keys.ToArray();
            foreach (var menu in menus)
            {
                CloseMenu(menu, destroy);
            }
            
            _menuHistory.Clear();
        }
        
        /// <summary>
        /// Returns the currently open menu, or null if there are no open menus.
        /// </summary>
        /// <returns>The currently open menu.</returns>
        public T GetCurrentMenu()
        {
            return _currentMenu ? _currentMenu : null;
        }
        
        /// <summary>
        /// Determines whether a menu of the specified type is open.
        /// </summary>
        /// <typeparam name="TMenu">The type of the menu to check.</typeparam>
        /// <returns>True if the menu is open, false otherwise.</returns>
        public bool IsMenuOpen<TMenu>() where TMenu : T
        {
            return _menuInstances.TryGetValue(typeof(TMenu), out var menu) && menu.IsOpen;
        }
        
        private void ShowOrInstantiate(Menu menu)
        {
            if (_menuInstances.TryGetValue(menu.GetType(), out var m))
            {
                _currentMenu = m;
            }
            else
            {
                var menuObj = Instantiate(menu, _menuCanvas.transform, false);
                menuObj.SetMenuViewer(this);
                
                _menuInstances.Add(menu.GetType(), (T)menuObj);
                
                _currentMenu = (T)menuObj;
            }
            
            _currentMenu.Open();
        }
    }
}