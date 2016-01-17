﻿using UnityEngine;
using System.Collections;

public class ArcItem : Item {

	private int _id;
    private string _label;
	private ItemType _itemType;
    private EventScript _action;

	private ShortcutSettings _sSettings;
	private ItemSettings _iSettings;
	private GameObject _parentObj;

	private Color _backgroundColor;
	private Color _focusingColor;
	private Color _selectingColor;

	private GameObject rendererObj;
	private GameObject backgroundObj;
	private GameObject focusingObj;
	private GameObject selectingObj;
	private GameObject labelObj;
	private GameObject centerObj;


	private UIArcItem _uiArcItemBg;
	private UIArcItem _uiArcItemFs; 
	private UIArcItem _uiArcItemSt;

	private UILabel _uiLabel;


	private float _innerRadius;
	private float _outerRadius;
	private float _thickness;


	private float _selectProg = 0.0f;
	private float _selectSpeed = 0.01f;
	private bool _isSelected = false;

	private ShortcutItemLayer _nextLayer = null;

	private ShortcutItemLayer _curLayer;
	public ShortcutItemLayer Layer {
		get {
			return _curLayer;
		}
		set {
			_curLayer = value;
		}
	}

	private bool _isCancelItem = false;
	public bool IsCancelItem {
		get {
			return _isCancelItem;
		}
		set {
			_isCancelItem = value;
		}
	}

	/* Set User input datas */
    public override void SetUserInputs(string label, ItemType itemType, EventScript action) {
        _label = label;
        _itemType = itemType;
        _action = action;
    }


	public override void Build(ShortcutSettings sSettings, GameObject parentObj) {
		// variables setting
		_id = ShortcutUtil.ItemAutoId;
		
		_sSettings = sSettings;
		_iSettings = _sSettings.ItemSettings;
		_parentObj = parentObj;

		_innerRadius = _iSettings.InnerRadius;
		_outerRadius = _iSettings.InnerRadius + _iSettings.Thickness;
		_thickness = _iSettings.Thickness;

		_backgroundColor = _iSettings.BackgroundColor;
		_focusingColor = _iSettings.FocusingColor;
		_selectingColor = _iSettings.SelectingColor;

		// rendering
		Rendering ();

		// interaction setting
		InteractionManager.SetItemPos (_id, Camera.main.WorldToViewportPoint(centerObj.transform.position));

	}
	
	// Update is called once per frame
	public override void Update () {
		if (_curLayer != null) {
			if (InteractionManager.HasItemId (_id)) {
				if (_curLayer.UILayer.IsCurrentLayer) {
					// register this item pos to interaction manager
					InteractionManager.SetItemPos (_id, Camera.main.WorldToViewportPoint (centerObj.transform.position));
					
					
					/***** focus, select ui update *****/
					float progress = InteractionManager.GetItemHighlightProgress (_id);
					
					float focusStart = 0.8f; // trigger focus percent = 80%
					if (progress > focusStart) { // is focusing
						float focusProg = Mathf.Lerp (0, 1, progress - focusStart);
						
						// focus, select event process
						if (focusProg == 1) { // all focus, is selecting
							if (_selectProg < 1.0f) {
								_selectProg += _selectSpeed;
							}
							
							// item click evnet
							if (_selectProg >= 1.0f) {
								_selectProg = 1.0f;
								if (!_isSelected) { // select action is triggered just once
									_isSelected = true;
									SelectAction();
								}
								
							}
							// select ui update
							_uiArcItemBg.UpdateMesh (0.0f, 0.0f, _backgroundColor);
							_uiArcItemFs.UpdateMesh (_innerRadius + (_thickness * _selectProg), _innerRadius + (_thickness * focusProg), _focusingColor);
							_uiArcItemSt.UpdateMesh (_innerRadius, _innerRadius + (_thickness * _selectProg), _selectingColor);
							
						} else {
							_selectProg = 0.0f;
							_isSelected = false;
							// focus ui update
							_uiArcItemBg.UpdateMesh (_innerRadius + (_thickness * focusProg), _outerRadius, _backgroundColor);
							_uiArcItemFs.UpdateMesh (_innerRadius, _innerRadius + (_thickness * focusProg), _focusingColor);
							_uiArcItemSt.UpdateMesh (0.0f, 0.0f, _selectingColor);
						}
						
					} else { // is non focusing
						// general ui update
						_uiArcItemBg.UpdateMesh (_innerRadius, _outerRadius, _backgroundColor);
						_uiArcItemFs.UpdateMesh (0.0f, 0.0f, _focusingColor);
						_uiArcItemSt.UpdateMesh (0.0f, 0.0f, _selectingColor);
					}
					
					/****************************************/
					
				}
				else {
					// register this item pos to interaction manager
					InteractionManager.SetItemPos (_id, Vector3.one*999);
					
				}
				
			}
		}
	}


	private void SelectAction() {
		switch (_itemType) {
		case (ItemType.Parent) :
			if (_nextLayer == null) {
				_nextLayer = Getter.GetChildLayerFromGameObject (gameObject);
				_nextLayer.Level = _curLayer.Level+1;
				_nextLayer.PrevLayer = _curLayer;
				
				_nextLayer.Build (_sSettings, gameObject);
			}
			else {
				_nextLayer.UILayer.AppearLayer(_nextLayer.Level - _curLayer.Level);
			}
			_curLayer.UILayer.DisappearLayer(_nextLayer.Level - _curLayer.Level);
			
			//print ("curLevel : " + _curLayer.Level + "  nextLevel : " + _nextLayer.Level);
			break;
		case (ItemType.NormalButton) :
			if (_isCancelItem) { // action for cancel item
				ShortcutItemLayer prevLayer = _curLayer.PrevLayer;
				prevLayer.UILayer.AppearLayer(prevLayer.Level - _curLayer.Level);
				_curLayer.UILayer.DisappearLayer(prevLayer.Level - _curLayer.Level);
				
				//print ("curLevel : " + _curLayer.Level + "  nextLevel : " + prevLayer.Level);
				break;
			}
			if (_action != null) {
				_action.ClickAction ();
			}
			else {
				print ("action script is empty");
			}
			break;
		default :
			break;
		}
		
	}



	public override void Rendering() {
		/***** rendering *****/
		// make renderer gameobject
		rendererObj = new GameObject ("Renderer");
		rendererObj.transform.SetParent (_parentObj.transform, false);
		
		// center pivot position setting
		centerObj = new GameObject("Center");
		centerObj.transform.SetParent (rendererObj.transform, false);
		centerObj.transform.localPosition = new Vector3 (0, 0, _iSettings.InnerRadius + (_iSettings.Thickness / 2));
		
		// build arc item background ui
		backgroundObj = new GameObject ("Background");
		backgroundObj.transform.SetParent (rendererObj.transform, false);
		focusingObj = new GameObject ("Focusing");
		focusingObj.transform.SetParent (rendererObj.transform, false);
		selectingObj = new GameObject ("Selecting");
		selectingObj.transform.SetParent (rendererObj.transform, false);
	
		_uiArcItemBg = backgroundObj.AddComponent<UIArcItem> ();
		_uiArcItemBg.Build (_sSettings);
		_uiArcItemBg.UpdateMesh(_innerRadius, _outerRadius, _backgroundColor);
		
		_uiArcItemFs = focusingObj.AddComponent<UIArcItem> ();
		_uiArcItemFs.Build (_sSettings);
		
		_uiArcItemSt = selectingObj.AddComponent<UIArcItem> ();
		_uiArcItemSt.Build (_sSettings);


		// build item label ui
		labelObj = new GameObject ("Label");
		labelObj.transform.SetParent(rendererObj.transform, false);
		labelObj.transform.localPosition = new Vector3(0, 0, _iSettings.InnerRadius);
		labelObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
		labelObj.transform.localScale = new Vector3(1, 1, 1);
		
		_uiLabel = labelObj.AddComponent<UILabel>();
		_uiLabel.SetAttributes (_label, _iSettings.TextFont, _iSettings.TextSize, _iSettings.TextColor);
		
		// each type's ui
		switch (_itemType) {
		case (ItemType.Parent) :
			GameObject labelHasChildObj = new GameObject ("HasChildArrow");
			labelHasChildObj.transform.SetParent (rendererObj.transform, false);
			labelHasChildObj.transform.localPosition = new Vector3(0, 0, _iSettings.InnerRadius+_iSettings.Thickness);
			labelHasChildObj.transform.localRotation = Quaternion.FromToRotation(Vector3.back, Vector3.right);
			labelHasChildObj.transform.localScale = new Vector3(1, 1, 1);
			
			UILabelHasChild uiLabelHasChild = labelHasChildObj.AddComponent<UILabelHasChild>();
			uiLabelHasChild.SetAttributes (">", _iSettings.TextFont, _iSettings.TextSize, _iSettings.TextColor);

			break;
		case (ItemType.NormalButton) :
			
			break;
		default :
			break;
		}
		
	}
}
