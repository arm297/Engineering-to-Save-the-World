using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Basic Class for connection creation, highlighting, and deletion.
 */

namespace Drills
{
    class Connection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        [SerializeField]
        private Color mousedOverColor;

        [SerializeField]
        private Color selectedColor;

        [SerializeField]
        private GameObject connectionEnd1;

        [SerializeField]
        private GameObject connectionEnd2;

        private Color defaultColor;

        public void OnPointerClick(PointerEventData eventData) {

        }

        public void OnPointerEnter(PointerEventData eventData) {

        }

        public void OnPointerExit(PointerEventData eventData) {

        }

        void Start() {
            
        }


        public void Enable() {

        }

        public void Disable() {

        }

    }

}
