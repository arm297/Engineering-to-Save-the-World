using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public Transform PanelArea;

	public void OnDrop(PointerEventData eventData) {
		ButtonCreate_NewNode BCN = eventData.pointerDrag.GetComponent<ButtonCreate_NewNode> ();
		if (BCN != null) {
			BCN.parentToReturnTo = PanelArea;
		}

		ButtonEdit_NewNode BEN = eventData.pointerDrag.GetComponent<ButtonEdit_NewNode> ();
		if (BEN != null) {
			BEN.parentToReturnTo = PanelArea;
		}
	}

	public void OnPointerEnter (PointerEventData eventData) {
		ButtonCreate_NewNode BCN = eventData.pointerDrag.GetComponent<ButtonCreate_NewNode> ();
		if (BCN != null) {
			BCN.parentToReturnTo = PanelArea;
			BCN.ableToMove = false;
			BCN.nodeAreaDropName = transform.name;
		}

		ButtonEdit_NewNode BEN = eventData.pointerDrag.GetComponent<ButtonEdit_NewNode> ();
		if (BEN != null) {
			BEN.parentToReturnTo = PanelArea;
			BEN.ableToMove = false;
			BEN.nodeAreaDropName = transform.name;
		}
	}

	public void OnPointerExit (PointerEventData eventData) {
		ButtonCreate_NewNode BCN = eventData.pointerDrag.GetComponent<ButtonCreate_NewNode> ();
		if (BCN != null) {
			BCN.parentToReturnTo = null;
			BCN.ableToMove = true;
			BCN.nodeAreaDropName = "";
		}

		ButtonEdit_NewNode BEN = eventData.pointerDrag.GetComponent<ButtonEdit_NewNode> ();
		if (BEN != null) {
			BEN.parentToReturnTo = null;
			BEN.ableToMove = true;
			BEN.nodeAreaDropName = "";
		}
	}
}
