using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Panel : MonoBehaviour {

	//Publics
	public CameraTarget initCamTarget;
	public CameraManager cam;

	public UnityEvent onActivated;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
		if(active) OnActivated();
	}

	public void OnActivated()
	{
		cam.SetTarget(initCamTarget);
		onActivated.Invoke();
	}

	#region BUTTON
	public void EnableButton(Button button)
	{
		button.enabled = true;
	}

	public void DisableButton(Button button)
	{
		button.enabled = false;
	}
	#endregion
}
