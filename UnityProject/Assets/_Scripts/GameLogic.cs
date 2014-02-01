using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {

	private GameLogic m_Instance;
	// Use this for initialization

	public class OutColider{};
	public class TableColider{};
	public class OutNetColider{};

	public GameLogic Instance { get { return m_Instance; } }

	private int tableBounces = 0;
	private int serveBounces = 0;
	private int currentSet = 0;

	private bool isServe = true;

	private Player player1 = new Player("Player1");
	private Player player2 = new Player("Player2");
	private int currentPlayer = 0;
	private bool isPlayer1 = true;

	public Vector3 player1start;
	public Vector3 player2start;

	void Awake() {
		m_Instance = this;
	}

	void OnDestroy() {
		m_Instance = null;
	}

	void Start () {
	
	}
	/// <summary>
	/// Returns number of a points in current set of a player
	/// </summary>
	/// <returns>The player poins.</returns>
	/// <param name="player">The number of a player 1 or 2</param>
	public int GetPlayerPoins(int player) {
		if (player == 1)
			return player1.GetPoints(currentSet);
		else
			return player2.GetPoints(currentSet);
	}
	public int GetPlayerSets(int player){
		if (player == 1)
			return player1.Sets;
		else
			return player2.Sets;
	}

	/// <summary>
	/// Checks if the there was any condition to add a point for a certain player after hit 	
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="colider">Colider - object the Ball had a collision with.</param>
	public int CheckHit (MonoBehaviour colider) {
		//jeśli out obojętne jaki nie serwujący dostaje punkt.
		if ((colider is OutColider)|| (colider is OutNetColider)) {
										if (currentPlayer == 0) {
												player2.AddPoint(currentSet);
										} else {
				player1.AddPoint(currentSet);
										}
		}  else if (colider is TableColider) 
		{
			tableBounces++;
			if(tableBounces > 1) {

			}
		}
		return 3;
	}


	/// <summary>
	/// Piłka uerze na aut/// </summary>
	public void PlayerHitOut() {
		//po prostu aut, lub bład serwisowy
		if ((tableBounces == 0) || (isServe && (tableBounces == 1) )) {
			if (isPlayer1) {
								player2.AddPoint (currentSet);
						} else {
								player1.AddPoint (currentSet);
						}
				} else {
						//odbiło się od stołu i potem aut - player nie odbił
						
			if (isPlayer1) {
								player1.AddPoint (currentSet);
						} else {
								player2.AddPoint (currentSet);
						}
				}
		//teraz ktoś będzie servować;
		isPlayer1 = !isPlayer1;
		isServe = true;
	}

	/// <summary>
	/// Players hit the table part of a player
	/// </summary>
	/// <returns><c>true</c>, if hit table was playered, <c>false</c> otherwise.</returns>
	/// <param name="player">Numer gracza, od którego połowy stołu odbiłą się piłeczka.</param>
	public bool PlayerHitTable(int player) {
		//jesli serwuje
		bool result = false;
		if (isServe) {
			if(tableBounces == 0) {
				//jeśli pierwsze odbicie to musi być po mojej stronie - wtedy inkrementuje odbicia i dalej
				if(((isPlayer1)&& (player==1)) || ((!isPlayer1)&&(player==2))) {
					tableBounces++;
					return false; //nie przerywamy - serv jak do tej pory poprawny
				} else {
					//jeśli pierwsze odbicie było po złej stronie przy servie
					//punkt dla przeciwnika
					if (isPlayer1) {
						player2.AddPoint (currentSet);
					} else {
						player1.AddPoint (currentSet);
					}
					result = true;
				}
			} else if(tableBounces == 1) {
				if(((isPlayer1)&& (player==2)) || ((!isPlayer1)&&(player==1))) {
					tableBounces++;
					result = false; //nie przerywamy - poprawny serv wyszedł
				}
			} else {
				if (isPlayer1) {
					player2.AddPoint (currentSet);
				} else {
					player1.AddPoint (currentSet);
				}
				result = true; //przerywamy - kolejne odbicie piłeski
			}
				} 
		//jeśli nie przy serwie
		else {
			if(tableBounces == 0) { 
				//jeśli uderzam w połowę przeciwnika to nic się nie dzieje.
				if(((isPlayer1)&& (player==2)) || ((!isPlayer1)&&(player==1))) {
					tableBounces++;
					result = false; //nie przerywamy - poprawny serv wyszedł
				} else {
					//jeśli uderzam w swoją połowę to punt dsla przeciwnika
					if (isPlayer1) {
						player2.AddPoint (currentSet);
					} else {
						player1.AddPoint (currentSet);
					}
					result = true;
				}
			} else {
				//jeśli kolejne odibie to punkt dla tego co odbił


			}
				
		}
		return result;
	}

	public bool ChaeckPoints() {
		bool result = false;
		if ((player1.GetPoints(currentSet) > 11) && (player2.GetPoints(currentSet) < 10)) {
						player1.AddSet();
						currentSet++;
			result = true;
				}
		return result;
	}
	// Update is called once per frame
	void Update () {
	
	}

	public class Player {
		private string name;
		private int sets = 0;
		private int[] points = new []{0,0,0};


		public Player(string playerName) {
			name = playerName;
			sets = 0;

		}
		public string Name { get { return name; } }
		public int GetPoints(int set = 0) { return points[set]; }
		public int Sets { get { return sets; } }

		public void AddPoint(int set) {
			points[set]++;
		}
		public void AddSet(){
			sets++;
		}


	}
	public enum GameState {
		Serv,
		Play,


	}

}
