using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualObject : MonoBehaviour{

	float width, height;


	public void setSize(float width, float height){
		this.height = height;
		this.width = width;
		gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
	}

	public void print(GameObject panel, int posX, int posY){
		gameObject.transform.SetParent(panel.transform);
		gameObject.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0); 
		gameObject.SetActive(true);
	}
}
