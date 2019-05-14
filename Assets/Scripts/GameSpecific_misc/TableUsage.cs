using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableUsage {

	int[] rows;
	int round;
	int roundRemain;
	public int tableIndex;

	public SituationChooser parent;

	//public TablaAuxiliar correspondences;
	public FGTable FGCorrespondences;

	public void initialize(int nRows) {
		rows = new int[nRows];
		for (int i = 0; i < nRows; ++i) {
			rows [i] = 0;
		}
		round = 0;
		roundRemain = nRows;
	}

	public void markRow(int row) {
		rows [row]++;
	}

	public int selectRow() {

		// find a free index
		int index = Random.Range (0, rows.Length);
		while (rows [index] != round) {
			index = Random.Range (0, rows.Length);
		}

//		// check for correspondence
//		int otherTable, otherRow;
//		if ((int)FGCorrespondences.getElement (0, 0) == tableIndex) {
//			for (int i = 0; i < FGCorrespondences.nRows (); ++i) {
//				if (((int)FGCorrespondences.getElement (1, i)) == index) {
//					otherTable = (int)FGCorrespondences.getElement (2, i);
//					otherRow = (int)FGCorrespondences.getElement (3, i);
//					parent.markRowInTable (otherTable, otherRow);
//					break;
//				}
//			}
//		}
//
//		else if ((int)FGCorrespondences.getElement (2, 0) == tableIndex) {
//			for (int i = 0; i < FGCorrespondences.nRows (); ++i) {
//				if (((int)FGCorrespondences.getElement (3, i)) == index) {
//					otherTable = (int)FGCorrespondences.getElement (0, i);
//					otherRow = (int)FGCorrespondences.getElement (1, i);
//					parent.markRowInTable (otherTable, otherRow);
//					break;
//				}
//			}
//		}


		rows [index]++; // mark as used

		--roundRemain;

		if (roundRemain == 0) {
			roundRemain = rows.Length;
			round++;
		}

		return index;

	}

}
