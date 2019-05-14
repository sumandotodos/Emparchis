using UnityEngine;
using System.Collections;

public class SBTable : MonoBehaviour {

	public Rosetta rosetta;
	public IntegerList nega_n;
	public StringBank[] column;
	//public string[] columnName;

	public int nColumns() {
		return column.Length;
	}

	public int nRows() {
		// we trust that all columns have the same length
		return column [0].nItems();
	}

	public string getContents(int c, int r) {

		column [c].rosetta = rosetta;
		return column [c].getString (r);

	}

	public string export() {
		string res = "";
		int columns = column.Length;
		if (nega_n != null)
			columns++;
		res += ((columns) + "\n");

		for (int k = 0; k < column [0].nItems (); ++k) {
			for (int i = 0; i < column.Length; ++i) {
				res += (column [i].phrase [k] + "\n");
			}
			if (nega_n != null) {
				res += ((nega_n.data [k]) + "\n");
			}
		}

		res = res.TrimEnd ('\n');
		return res;
	}

}
