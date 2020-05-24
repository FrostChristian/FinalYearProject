using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace FinalYear {

    public class MenuHandler : MonoBehaviour {
        [Header("Menu Prefabs")]
        [SerializeField] private MainMenu mainMenuPrefab = default;
        [SerializeField] private FlagPainterMenu flagPainterMenuPrefab = default;
        [SerializeField] private BoxMatchMenu boxMatchMenuPrefab = default;

        [Space]
        [SerializeField] private Transform _menuParent = default;

        private Stack<Menu> _menuStack = new Stack<Menu>();

        private static MenuHandler _instance;
        public static MenuHandler Instance { get => _instance; }

        private void Awake() {
            if (_instance != null) {//singleton
                Destroy(gameObject);
            } else {
                _instance = this; // global
                InitializeMenus();
                DontDestroyOnLoad(gameObject); //keep active between scenes obv
            }
        }

        private void OnDestroy() {
            if (_instance == this) {
                _instance = null;
            }
        }

        private void InitializeMenus() {
            if (_menuParent == null) {
                GameObject menuParentObject = new GameObject("Menus");
                _menuParent = menuParentObject.transform;
            }
            DontDestroyOnLoad(_menuParent.gameObject);
            // Menu[] menuPrefabs = { mainMenuPrefab, loadMenuPrefab, optionsMenuPrefab, creditsScreenPrefab, inGameMenuPrefab,  pauseMenuPrefab, winMenuPrefab };

            BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fields = this.GetType().GetFields(myFlags);

            foreach (FieldInfo field in fields) {
                Menu prefab = field.GetValue(this) as Menu;
                if (prefab != null) {
                    Menu menuInstance = Instantiate(prefab, _menuParent);
                    if (prefab != mainMenuPrefab) {
                        menuInstance.gameObject.SetActive(false);
                    } else {
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        public void OpenMenu(Menu menuInstance) {
            if (menuInstance == null) {
                return;
            }

            if (_menuStack.Count > 0) {
                foreach (Menu menu in _menuStack) {
                    menu.gameObject.SetActive(false);
                }
            }
            menuInstance.gameObject.SetActive(true);
            _menuStack.Push(menuInstance);
        }

        public void CloseMenu() {
            if (_menuStack.Count == 0) {
                return;
            }

            Menu topMenu = _menuStack.Pop();
            topMenu.gameObject.SetActive(false);

            if (_menuStack.Count > 0) {
                Menu nextMenu = _menuStack.Peek();
                nextMenu.gameObject.SetActive(true);
            }
        }

        public void PlayClickSound() {
            StartCoroutine(PlayPointerClick());
        }

        private IEnumerator PlayPointerClick() {
            //audioSource.priority = 10;
            //audioSource.PlayOneShot(mouseClick);
            yield return new WaitForSeconds(0.4f);
        }
    }
}