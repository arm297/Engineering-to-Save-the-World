using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Basic Class for connection creation, highlighting, and deletion.
 */

namespace Drill
{
    class Connection
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

        void Start() {

        }


        public void Enable() {

        }

        public void Disable() {

        }

    }

}
