﻿using UnityEngine;
using System.Collections;

public class ShortcutController : MonoBehaviour {

	public ShortcutSettings _ShortcutSettings;
	public ItemSettings _ItemSettings;
	public ItemHierarchy _ItemHierarchy;
	public Transform _Camera;

	private bool isFirst = true;


	void Awake () {
		if (!CheckInspector ()) {
			print ("Check inspector factors");
			return;
		}
		_ShortcutSettings.ItemSettings = _ItemSettings;

		PutInsideOfMainCamera ();
	}


	/* Check all Inspector factors are valid. */
	private bool CheckInspector() {
		if (_ShortcutSettings == null ||
		    _ItemSettings == null ||
		    _ItemHierarchy == null ||
		    _Camera == null) {
			return false; 
		}
		return true;
	}


	/* Put this shortcut inside of main camera. */
	private void PutInsideOfMainCamera() {
		gameObject.transform.SetParent (_Camera.transform, false);

		Vector3 pos = new Vector3 (_ShortcutSettings.XPosition, _ShortcutSettings.YPosition, ComputeZPos (_ShortcutSettings.XPosition, _ShortcutSettings.YPosition));
		gameObject.transform.position = Camera.main.ViewportToWorldPoint (pos);

		// xz 평면 회전 보정
		//Vector3 relative = transform.InverseTransformPoint (_Camera.transform.position);
		//float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
		//if (_ShortcutSettings.Type == ShortcutType.Arc) {
		//	gameObject.transform.localRotation = Quaternion.Euler (new Vector3 (180 - angle, 0, 0));
		//} 
		//else if (_ShortcutSettings.Type == ShortcutType.Stick) {
			//gameObject.transform.localRotation = Quaternion.Euler (new Vector3 (360-angle, 180, 180));
		//}

		// yz 평면 회전 보정
		//angle = Mathf.Atan2 (relative.y, relative.z) * Mathf.Rad2Deg;
		//if (_ShortcutSettings.Type == ShortcutType.Stick) {
		//	gameObject.transform.localRotation = Quaternion.Euler (new Vector3 (0, 180-angle, 0));
		//} 

	}

	private float ComputeZPos(float x, float y) {
		float d = 1.0f;

		return Mathf.Sqrt ((d * d) - ((x-0.5f) * (x-0.5f)) - ((y-0.5f) * (y-0.5f)));
	}


	/* Appear this shortcut. */
	public void Appear() {
		if (isFirst) {
			// item hierarchy build
			_ItemHierarchy.Build (_ShortcutSettings, gameObject);
			isFirst = false;
		} 
		else {
			_ItemHierarchy.Appear ();
		}
	}


	/* DisAppear this shortcut. */
	public void Disappear() {
		_ItemHierarchy.Disappear ();
	}
}
