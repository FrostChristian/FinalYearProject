using UnityEngine;


namespace FinalYear {

    [RequireComponent(typeof(Canvas))]

    public abstract class Menu<T> : Menu where T : Menu<T> { // CRTP curiosly recurring template pattern 

        private static T _instance;
        public static T Instance { get { return _instance; } }

        protected virtual void Awake() {
            if (_instance != null) { // singelton
                Destroy(gameObject);
            } else {
                _instance = (T)this;
            }
        }

        protected virtual void OnDestroy() {
            _instance = null;
        }

        public static void Open() {
            if (MenuHandler.Instance != null && Instance != null) {
                MenuHandler.Instance.OpenMenu(Instance);
            }
        }
    }

    public abstract class Menu : MonoBehaviour { // base for menus
        public virtual void OnBackPressed() {
            MenuHandler menuManager = FindObjectOfType<MenuHandler>();
            if (menuManager != null) {
                menuManager.CloseMenu();
            }
        }
    }
}
