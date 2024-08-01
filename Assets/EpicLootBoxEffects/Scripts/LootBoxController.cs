using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxController : MonoBehaviour {

	public int idIcon;
	public int idEffect;
	public bool isOpened;
	public bool _isMuiti;
	public GameObject[] IconPrefabs;
	public GameObject[] EffectPrefabs;
	public GameObject[] DesFxObjs;
	public GameObject[] DesIconObjs;
	private GameObject Lootbox;
	LobbyUIManager _lobbyUIManager;

	void Start () {
		_lobbyUIManager = GameObject.FindObjectOfType<LobbyUIManager>();
		idEffect += 1;
		idIcon += 1;
		SetupVfx ();
		isOpened = false;
		//OpenBox();
	}

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Ended)
			{
				OpenBox();
			}
		}
	}

    public void OpenBox()
    {
		if (!isOpened)
		{
			StartCoroutine(PlayFx());
		}
	}

	IEnumerator PlayFx() {
		isOpened = true;
		Destroy(GameObject.Find("LootBox_Effect"));
		idEffect = Mathf.Clamp(idEffect, 1, 25);
		yield return new WaitForSeconds(0.2f);
		Destroy (Lootbox);
		Lootbox = Instantiate (IconPrefabs [2], this.transform.position, this.transform.rotation);
		Lootbox.transform.position = new Vector3(0.5f, 0, 0);
		Lootbox.transform.SetParent(transform);
		yield return new WaitForSeconds(0.1f);
		if (_isMuiti)
			_lobbyUIManager.MultiGachaUI();
		else
			_lobbyUIManager.SingleGachaUI();
		GameObject _lootbox_Effect = Instantiate (EffectPrefabs [idEffect], this.transform.position, this.transform.rotation);
		_lootbox_Effect.transform.SetParent(transform);
		CameraShake.myCameraShake.ShakeCamera (0.3f, 0.1f);
	}

	IEnumerator PlayIcon() {
		DesIconObjs = GameObject.FindGameObjectsWithTag("Icon");

		foreach(GameObject DesIconObj in DesIconObjs)
			Destroy(DesIconObj.gameObject);
		
		yield return new WaitForSeconds(0.1f);
		Lootbox = Instantiate (IconPrefabs [1], this.transform.position, this.transform.rotation);
		Lootbox.transform.position = new Vector3(0.5f, 0, 0);
	}

	public void ChangedFx (int i) {
		ResetVfx ();
		idEffect = idEffect + i;
		idEffect = Mathf.Clamp(idEffect, 1, 25);
		//StartCoroutine(PlayIcon());
	}

	public void SetupVfx () {
		Lootbox = Instantiate (IconPrefabs [1], this.transform.position, this.transform.rotation);
		Lootbox.transform.position = new Vector3(0.5f, 0, 0);
		Lootbox.transform.SetParent(transform);
		GameObject _lootBox = Instantiate(EffectPrefabs[idEffect], this.transform.position, this.transform.rotation);
		_lootBox.transform.SetParent(transform);
	}

	public void PlayAllVfx (){
		if (!isOpened) {
			StartCoroutine(PlayFx());
		}
	}

	public void ResetVfx () {
		DesFxObjs = GameObject.FindGameObjectsWithTag("Effects");
	
		foreach(GameObject DesFxObj in DesFxObjs)
				Destroy(DesFxObj.gameObject);
		isOpened = false;

		DesIconObjs = GameObject.FindGameObjectsWithTag("Icon");
	
		foreach(GameObject DesIconObj in DesIconObjs)
			Destroy(DesIconObj.gameObject);
		StartCoroutine(PlayIcon());
	}
}