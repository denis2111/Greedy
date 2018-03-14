using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public GameObject startMenu;
	public GameObject gamePanel;
	public GameObject gridPanel;
	public GameObject restartButton;
	public GameObject score;
	GameObject brick;
	GameObject cherry;
	GameObject player;
	PlayerController playerController;
	Grid grid;
	Text scoreText;
	Vector2 startPos;
	Vector2 direction;
	bool gameStarted;
	const int touchSensitivity = 40;

	void Update(){
		if (gameStarted){
			if (Input.touchCount > 0){
				Touch touch = Input.GetTouch(0);
				switch (touch.phase){
					case TouchPhase.Began:
						Debug.Log("Began");
						direction = Vector2.zero;
						startPos = touch.position;
						break;

					case TouchPhase.Moved:
						direction = touch.position - startPos;
						break;

					case TouchPhase.Ended:
						break;
				}
			}
			if (direction.magnitude>touchSensitivity)
				playerController.changeDirection(direction);	
		}
	}

	public void startButtonClick(){
		startMenu.SetActive(false);
		gamePanel.SetActive(true);

		initialize();
		
		gameStarted = true;

		addScoreChanged();
		addPlayerMovement();
	}

	void addPlayerMovement(){
		playerController.LogicalFrame += delegate {
			var nextPosition = playerController.nextPos();
			var objectFromNextPosition = grid.getObject(nextPosition);
			if (objectFromNextPosition != null){
				switch (grid.getObject(nextPosition).tag){
					case "Cherry":
						playerController.addScore(10);
						grid.destroy(nextPosition);
						grid.move(playerController.pos, nextPosition);
						playerController.move(nextPosition, grid.gridPosToRealPos(nextPosition));
						grid.add(GameObject.Instantiate(cherry.GetComponent<VisualObject>()), grid.getRandomEmptyCell());
						break;

					case "Brick":
						//make one move from curent position to the same position to stabilise the player
						playerController.move(playerController.pos,grid.gridPosToRealPos(playerController.pos));
						break;
				}
			}
			else{
				grid.move(playerController.pos, nextPosition);
				playerController.move(nextPosition, grid.gridPosToRealPos(nextPosition));
			}
		};
	}

	void addScoreChanged(){
		playerController.scoreChanged += (score) =>{
			scoreText.text = string.Format("Score: {0}", score);
		};
	}

	void initialize(){
		brick = Resources.Load<GameObject>("Prefabs/Brick");
		cherry = Resources.Load<GameObject>("Prefabs/Cherry");
		
		createGrid();
		
		scoreText = score.GetComponent<Text>();
		
		player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
		playerController = player.GetComponent<PlayerController>();
		playerController.setSize(grid.getCellSize());
		playerController.setSpawnPoint(grid.spawnPoint,grid.gridPosToRealPos(grid.spawnPoint));
		VisualObject playerVS = player.GetComponent<VisualObject>();
		grid.add(playerVS, playerController.pos);
	}

	void createGrid(){
		grid = new Grid(gridPanel, 660, 660);
		grid.makeBorder(brick.GetComponent<VisualObject>());
		grid.add(GameObject.Instantiate(cherry.GetComponent<VisualObject>()), grid.getRandomEmptyCell());
	}

	public void restartButtonClick(){
		//To do
	}
}
