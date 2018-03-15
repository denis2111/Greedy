using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Grid.gridPos pos;
	public System.Action<int> scoreChanged;
	public System.Action LogicalFrame;
	int spriteType;
	int score;
	int curentDirectionInt = 0;
	float width,height;
	float speed = 120;
	float timeToChangeSprite;
	float currentFrameTime;
	const float logicalFrameTime = 0.5f;
	bool stopped;
	Vector2 spawnPoint, nextPosition, position, lastPosition;
	direction curentDirection;
	Sprite[] sprite = new Sprite[2];
	VisualObject visualObject;
	Image image;
	
	void Start () {
		curentDirection = direction.East;
		stopped = false;
		spriteType = 0;
		timeToChangeSprite = 0;
		loadSprites();
		image = gameObject.GetComponentInChildren<Image>();

		visualObject = this.gameObject.GetComponent<VisualObject>();
		LogicalFrame += delegate {
			switch (curentDirection){
				case direction.North:
					curentDirectionInt = 0;
					break;
				case direction.East:
					curentDirectionInt = 1;
					break;
				case direction.South:
					curentDirectionInt = 2;
					break;
				case direction.West:
					curentDirectionInt = 3;
					break;
			}
			image.sprite = sprite[spriteType];
			Debug.Log(curentDirectionInt);
			image.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(curentDirectionInt == 3 ? 180 : 0, 0, -curentDirectionInt * 90);
		};
	}
	
	void Update () {
		timeToChangeSprite += Time.deltaTime;
		currentFrameTime += Time.deltaTime;

		var movement = nextPosition - lastPosition;

		position = (currentFrameTime/logicalFrameTime) * movement + lastPosition;

		if(currentFrameTime >= logicalFrameTime){
			LogicalFrame();
			currentFrameTime -= logicalFrameTime;
		}

		if (timeToChangeSprite>0.2){
			timeToChangeSprite = 0;
			spriteType = 1-spriteType;
			image.sprite = sprite[spriteType];
			Debug.Log(curentDirectionInt);
			image.gameObject.GetComponent<RectTransform>().localEulerAngles = new Vector3(curentDirectionInt == 3 ? 180 : 0, 0, -curentDirectionInt * 90);
		}

		gameObject.GetComponent<RectTransform>().localPosition = position;
	}

	public Grid.gridPos nextPos(){
		switch (curentDirection){
				case direction.North:
					return new Grid.gridPos(pos.X, pos.Y - 1);
				case direction.East:
					return new Grid.gridPos(pos.X + 1, pos.Y);
				case direction.South:
					return new Grid.gridPos(pos.X, pos.Y + 1);
				case direction.West:
					return new Grid.gridPos(pos.X - 1, pos.Y);
				default:
					throw new System.NotImplementedException();
			}
	}

	public void addScore(int score){
		this.score += score;
		scoreChanged(this.score);
	}

	public int getScore(){
		return score;
	}

	public void setSpawnPoint(Grid.gridPos pos, Vector2 realPos){
		nextPosition = realPos;
		move(pos,realPos);
	}

	public void move(Grid.gridPos pos, Vector2 realPos){
		this.pos = pos;
		lastPosition = nextPosition;
		nextPosition = realPos;
	}

	public void changeDirection(direction newDirection){
		curentDirection = newDirection;
	}

	public void changeDirection(Vector2 newDirection){
		direction lastDirection = curentDirection;
		if (Mathf.Abs(newDirection.x) > Mathf.Abs(newDirection.y)){
			if (newDirection.x > 0){
				curentDirection = direction.East;
			}
			else if (newDirection.x <= 0){
				curentDirection = direction.West;
			}
		}
		else if (Mathf.Abs(newDirection.x) <= Mathf.Abs(newDirection.y)){
			if (newDirection.y > 0){
				curentDirection = direction.North;
			}
			else if (newDirection.y <= 0){
				curentDirection = direction.South;
			}
		}
	}

	public void setSize(Vector2 size){
		this.width = size.x;
		this.height = size.y;
		GetComponent<RectTransform>().sizeDelta = size;
	}

	public void restart(){
		addScore(-score);
	}

	void loadSprites(){
		sprite[0] = Resources.Load<Sprite>("Materials/Android");
		sprite[1] = Resources.Load<Sprite>("Materials/Android2");
	}

	public enum direction{
		North,
		East,
		South,
		West
	};
}
