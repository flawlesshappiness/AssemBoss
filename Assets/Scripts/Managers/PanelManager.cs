using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour {

	public Panel initPanel;
	public DialogManager mgDialog;
	private Stack<Panel> panels = new Stack<Panel>();
	private Panel pCur;

	//Awake
	void Awake()
	{
		mgDialog.Init();
	}

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Init()
	{
		foreach(Panel p in GetComponentsInChildren<Panel>())
		{
			p.SetActive(false);
		}
		Forward(initPanel);
	}

	public void Forward(Panel panel)
	{
		if(pCur != null) 
		{
			pCur.SetActive(false);
			panels.Push(pCur);
		}

		pCur = panel;
		pCur.SetActive(true);
	}

	/// <summary>
	/// Returns to previous panel, or previous menu.
	/// </summary>
	public void Back()
	{
		pCur.SetActive(false);
		pCur = panels.Pop();
		pCur.SetActive(true);
	}
}
