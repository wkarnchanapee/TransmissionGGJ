﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    // arbitary change
    Camera playerCam;
	bool isLoaded;
    [SerializeField] AudioSource sndShoot;
    [SerializeField] AudioSource sndAbsorb;

    [SerializeField] GameObject particleFx;

    [SerializeField] Animator gunAnimCtrl;

	void Awake() {
		playerCam = GetComponent<Camera>();

        particleFx.SetActive(false);

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GunHandler();
        //DrawLine(transform.position, transform.position + transform.forward * 10, Color.blue);
        
    }

    private void LateUpdate()
    {
        //reset gun animation state

        gunAnimCtrl.SetInteger("state", 0);
    }

    GameObject RaycastObject()
	{
		RaycastHit hit;
		float dist;

		Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(transform.position, forward, Color.green);

		if(Physics.Raycast(transform.position, forward, out hit))
		{
			if (Input.GetMouseButtonDown(0))
			{
				print(hit.transform.gameObject.name);
				return hit.transform.gameObject;
			}
		}

		return null;
	}


	void Absorb()
	{
		isLoaded = true;

        sndAbsorb.Play();

        particleFx.SetActive(true);

        gunAnimCtrl.SetInteger("state", 1);
    }

	void Shoot()
	{

		isLoaded = false;

        sndShoot.Play();

        particleFx.SetActive(false);

        gunAnimCtrl.SetInteger("state", 1);
	}

	void GunHandler()
	{
		GameObject raycastedObj = RaycastObject();
		if (raycastedObj == null)
			return;

		EnergyObject raycastedEnergyObj = raycastedObj.GetComponent<EnergyObject>();
		if (raycastedEnergyObj == null)
			return;

		if (!isLoaded && raycastedEnergyObj.isVisible)
		{
			Absorb();
			raycastedEnergyObj.SwitchState();
		}

		else if (isLoaded && !raycastedEnergyObj.isVisible)
		{
			Shoot();
			raycastedEnergyObj.SwitchState();
		}
	}

    // arbitary change
	/*void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.SetParent(transform);
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.alignment = LineAlignment.Local;
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 0.5f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}*/
}
