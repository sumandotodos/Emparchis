using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Helper class which chooses a text from a table
 * following a set of rules */

public enum GameType { normal, kids };

public class Situation {

	public string question;
	public int table, qc, row;
	public string answer1;
	public int a1c;
	public string answer2;
	public int a2c;
	public Mood mood;

}

public class SituationChooser : MonoBehaviour {

	public GameType gameType;

	public FGTable[] FGTables;
	//public SBTable[] tables;
	TableUsage[] tableUsage;
	//public IntegerList[] tableIsNegativeSituation;
	//public TablaAuxiliar correspondences;
	public FGTable FGCorrespondences;
	public bool isNegative;

	public void initialize() {

		tableUsage = new TableUsage[FGTables.Length];
		for (int i = 0; i < FGTables.Length; ++i) {
			tableUsage [i] = new TableUsage ();
			tableUsage [i].tableIndex = i;
			tableUsage [i].parent = this;
			//tableUsage [i].correspondences = correspondences;
			tableUsage [i].FGCorrespondences = FGCorrespondences;
			tableUsage [i].initialize (FGTables [i].nRows());
		}

	}

	public string retrieveTextFromTables(int table, int row, int column) {

		return (string)FGTables [table].getElement (column, row);
		//tables [table].column [column].rosetta = rosetta;
		//return tables [table].column [column].getString (row);

	}

	public bool isNegativeSituation(int table, int row) {

		bool res = false;

		if (table > 0) 
		{
			res = ((int)FGTables [table].getElement (9, row)) == 0;
		}
		else res = ((int)FGTables [table].getElement (3, row)) == 0;
		//res = (tableIsNegativeSituation [table].data [row] == 0);
		isNegative = res;
		return res;
	}

	public void markRowInTable(int table, int row) {
		tableUsage [table].markRow(row);
	}

	// SE HA SUMADO 1 A TODAS LAS COLUMNAS, PORQUE AHORA LA PRIMERA COLUMNA ES EL NUMERADOR DE LAS SITUACIONES
	public Situation chooseSituation(Family role, int test, int subtest) {

		Situation result = new Situation ();
		int row = 0;
		int column = 0; // la "situación"
		int table = 0;
		int column1 = 0; // la "respuesta" del pensadillo ahora esto depende sólo del tipo de test, y no de la tabla seleccionada
		int column2 = -1; // lista de emociones esto tampoco depende de la tabla seleccionada. O vale -1 o vale 2

		do {

			if(test == 0) { table = 0; column = 1; column1 = 2; column2 = -1; }
			//if((test == 0) && (gameType == GameType.kids)) { table = 0; column = 0; column1 = 0; column2 = -1; }

			if(test == 2) {
				if(gameType == GameType.normal) {
					int r = Random.Range(0, 4);
					if(r == 0) table = 1;
					if(r == 1) table = 3;
					if(r == 2) table = 5;
					if(r == 3) table = 6;
				}
				if(gameType == GameType.kids) {
					int r = Random.Range(0, 4);
					if(r == 0) table = 1;
					if(r == 1) table = 3;
					if(r == 2) table = 6;
				}
				column = 1;
				column1 = 4;
				column2 = 3;
			}

			if((test == 4) || (test == 1) || (test == 3)) {
				if(gameType == GameType.normal) {
					int r = Random.Range(0, 3);
					if(r == 0) table = 3;
					if(r == 1) table = 5;
					if(r == 2) table = 6;
				}
				if(gameType == GameType.kids) {
					int r = Random.Range(0, 2);
					if(r == 0) table = 3;
					if(r == 1) table = 6;
				}
				column = 2;
				if(test == 4) {
					column1 = 4; // expresar
				}
				if(test == 1) {
					column1 = 6; // circunstancias
				}
				if(test == 3) {
					column1 = 7; //creatividad
				}
				column2 = 3;
			}

			row = FGTables[table].getNextRowIndex();

		} while(!isNegativeSituation (table, row));



		result.question = (string)FGTables[table].getElement(column, row);
		if (result.question.Equals ("")) { // como puede que se haya escapado algo en la asignación de las columnas...
			column = 1-column;
			result.question = (string)FGTables[table].getElement(column, row);
		}

		result.answer1 = (string)FGTables[table].getElement(column1, row);

		if (test != 0) {
			// if the situation is positive, do not spawn emotion bubbles
			if (!isNegativeSituation (table, row))
				column2 = -1;
		}
//		if (test == 3 && !isNegativeSituation (table, row)) {
//			if(table > 1) column = 2;
//		}
		if (column2 != -1) {
			result.answer2 = (string)FGTables [table].getElement (column2, row);

		} else
		result.answer2 = "";

		result.question = result.question.Replace ("\\n", "\n\n");
		
		result.table = table;
		result.row = row;
		result.qc = column; // la "situación"
		result.a1c = column1; // la "contestación" para los paneles auxiliares
		result.a2c = column2; // la lista de emociones

		return result;
		//return "some shit";
	}
}
