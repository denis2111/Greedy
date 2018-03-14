using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Grid{

	GameObject gamePanel;
	int width = 660;
	int height = 660;
	int linesNumber = 10;
	int columnsNumber = 10;
	int cellSizeX, cellSizeY;
	VisualObject[,] grid;

	public Grid (GameObject gamePanel){
		this.gamePanel = gamePanel;
	}

	public Grid (GameObject gamePanel, int width, int height){
		this.gamePanel = gamePanel;
		this.width = width;
		this.height = height;
		calibrate();
	}

	public Grid (GameObject gamePanel, int width, int height, int linesNumber, int columnsNumber){
		this.gamePanel = gamePanel;
		this.width = width;
		this.height = height;
		this.linesNumber = linesNumber;
		this.columnsNumber = columnsNumber;

		calibrate();
		print();
	}

	public VisualObject getObject(gridPos pos){
		return grid[pos.X,pos.Y];
	}

	public void makeBorder(VisualObject obj){
		for (int col = 0; col < columnsNumber; ++col) 
			grid[col,0] = GameObject.Instantiate(obj);
		for (int lin = 1; lin < linesNumber; ++lin) 
			grid[columnsNumber-1,lin] = GameObject.Instantiate(obj);
		for (int col = 0; col < columnsNumber-1; ++col)
			grid[col,linesNumber-1] = GameObject.Instantiate(obj);
		for (int lin = 1; lin < linesNumber-1; ++lin)
			grid[0,lin] = GameObject.Instantiate(obj);
		print();
	}

	public Vector2 getCellSize(){
		return new Vector2(cellSizeX,cellSizeY);
	}

	public void destroy(gridPos pos){
		GameObject.Destroy(grid[pos.X, pos.Y].gameObject);
	}

	public void add(VisualObject obj, gridPos pos){
		obj.setSize(cellSizeX,cellSizeY);
		grid[pos.X,pos.Y] = obj;
		print();
	}

	public void move(gridPos from, gridPos to){
		grid[to.X, to.Y] = grid[from.X, from.Y];
		grid[from.X, from.Y] = null;
	}

	public gridPos getRandomEmptyCell(){
		gridPos position = new gridPos(Random.Range(1,columnsNumber-1),Random.Range(1,linesNumber-1));
		while (grid[position.X,position.Y] != null){
			position = new gridPos(Random.Range(1,columnsNumber-1),Random.Range(1,linesNumber-1));
		}
		return position;
	}

	public void setWidth(int width){
		this.width = width;
		calibrate();
	}

	public void setHeight(int height){
		this.height = height;
		calibrate();
	}

	public void setLinesNumber(int linesNumber){
		this.linesNumber = linesNumber;
		calibrate();
	}

	public void setColumnsNumber(int columnsNumber){
		this.columnsNumber = columnsNumber;
		calibrate();
	}
	
	public Vector2 gridPosToRealPos(gridPos pos){
		return new Vector2(pos.X*cellSizeX,-pos.Y*cellSizeY);
	}

	void print(){
		for (int lin = 0; lin < linesNumber; ++lin){
			for (int col = 0; col < columnsNumber; ++col){
				if (grid[col,lin]!=null){
					grid[col,lin].setSize(cellSizeX,cellSizeY);
					grid[col,lin].print(gamePanel, col * cellSizeX, -lin * cellSizeY);
					grid[col,lin].gameObject.SetActive(true);
				}
			}
		}
	}

	void calibrate(){
		cellSizeY = height / linesNumber;
		cellSizeX = width / columnsNumber;
		//To check - Posibil memorylick. Ce era inainte in grid nu se sterge.
		grid = new VisualObject[linesNumber, columnsNumber];
		spawnPoint = new gridPos((columnsNumber/2-1), (linesNumber/2-1));
	}

	public gridPos spawnPoint {get; private set;}

	public struct gridPos{
		public int X,Y;

		public gridPos(int _X,int _Y){
			X = _X;
			Y = _Y;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", X, Y);
		}

		public static bool operator==(gridPos A, gridPos B)
		{
			return A.X == B.X && A.Y == B.Y;
		}

		public static bool operator!=(gridPos A, gridPos B)
		{
			return !(A == B);
		}
	}

}
