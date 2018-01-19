using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PanelAttackIfStatement : MonoBehaviour {

	private DialogManager mgDialog;

	private PanelEditAttack panel;
	private string category = "";
	private string value = "";
	private int attack = 0;

	//UI
	public Button bCategory;
	public Button bValue;
	public Button bAttack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(PanelEditAttack p, DialogManager mgDialog)
	{
		panel = p;
		this.mgDialog = mgDialog;

		SetCategory(NextAttackCategory.HEALTH.ToString());
		SetValue(NextAttackValueHealth.HIGH.ToString());
		SetAttack("This attack", panel.GetAttacks().IndexOf(panel.GetCurrentAttack()));
	}

	public void Load(DataAttackNext next)
	{
		SetCategory(next.category);
		SetValue(next.value);
		SetAttack(next.nextAttack.name.value, panel.GetAttacks().IndexOf(next.nextAttack));
	}

	public void OnClickSetCategory()
	{
		var list = M.GetListOfEnum(typeof(NextAttackCategory));
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
		mgDialog.DisplayList("", items, delegate(DialogManager.ListItem obj) {
			SetCategory(obj.name);
		});
	}

	public void OnClickSetValue()
	{
		var list = GetValuesFromCategory(M.GetEnum<NextAttackCategory>(category));
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
		mgDialog.DisplayList("", items, delegate(DialogManager.ListItem obj) {
			SetValue(obj.name);
		});
	}

	public void OnClickSetAttack()
	{
		var list = panel.GetAttackNames();
		var items = new List<DialogManager.ListItem>();
		for(int i = 0; i < list.Count; i++) items.Add(new DialogManager.ListItem(list[i], i));
		mgDialog.DisplayList("", items, delegate(DialogManager.ListItem obj) {
			SetAttack(obj.name, obj.id);
		});
	}

	public void OnClickClose()
	{
		panel.RemoveNextAttack(this);
	}

	public void OnClickMoveUp()
	{
		panel.MoveUpNextAttack(this);
	}

	public void OnClickMoveDown()
	{
		panel.MoveDownNextAttack(this);
	}

	public static List<string> GetValuesFromCategory(NextAttackCategory category)
	{
		switch(category)
		{
		case NextAttackCategory.HEALTH: return M.GetListOfEnum(typeof(NextAttackValueHealth));
		case NextAttackCategory.POSITION: return M.GetListOfEnum(typeof(NextAttackValuePosition));
		default: return new List<string>();
		}
	}

	void SetCategory(string text)
	{
		bCategory.GetComponentInChildren<Text>().text = text;
		SetValue(GetValuesFromCategory(M.GetEnum<NextAttackCategory>(text)).First());
		category = text;
	}

	void SetValue(string text)
	{
		bValue.GetComponentInChildren<Text>().text = text;
		value = text;
	}

	void SetAttack(string text, int id)
	{
		bAttack.GetComponentInChildren<Text>().text = text;
		attack = id;
	}

	public DataAttackNext GetData()
	{
		DataAttackNext data = new DataAttackNext();
		data.category = category;
		data.value = value;
		data.nextAttack = panel.GetAttacks().ElementAt(attack);
		return data;
	}
}
