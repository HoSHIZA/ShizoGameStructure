using UnityEngine;

namespace Game.Core.Base.MenuManagement
{
    /// <summary>
    /// An abstract base class for all menu objects in the game. This class inherits from MonoBehaviour class.
    /// </summary>
    public abstract class Menu : MonoBehaviour
    {
        /// <summary>
        /// The MenuViewer object that controls the display of this menu object.
        /// </summary>
        protected MenuViewer<Menu> MenuViewer { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the menu is currently open.
        /// </summary>
        public bool IsOpen => gameObject.activeSelf;
        
        /// <summary>
        /// Opens the menu.
        /// </summary>
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Closes the menu.
        /// </summary>
        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Sets the MenuViewer object for this menu.
        /// </summary>
        /// <typeparam name="T">The type of the menu.</typeparam>
        /// <param name="menuViewer">The MenuViewer object that controls the display of this menu.</param>
        internal void SetMenuViewer<T>(MenuViewer<T> menuViewer) where T : Menu
        {
            if (MenuViewer) return;

            MenuViewer = menuViewer as MenuViewer<Menu>;
        }
    }
}