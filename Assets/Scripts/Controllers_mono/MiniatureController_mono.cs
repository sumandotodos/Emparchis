using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniatureController_mono : MonoBehaviour {

	public RawImage frame;
	public RawImage contents;

	public Texture[] table1;
	public Texture[] table2;
	public Texture[] table3;
	public Texture[] table4;
	public Texture[] table5;
	public Texture[] table6;
	public Texture[] table7;

	public NotMyRouletteController_mono notMyRouletteController;

	public Texture[] positives;

	public void updateMiniature(int testType, int table, bool negative, int row) {

		if (testType == 0) {
			frame.enabled = negative;
			contents.enabled = true;
			contents.texture = notMyRouletteController.aux0Images [row];
		} else
			updateMiniature (testType, table, negative);

	}

	public void updateMiniature(int testType, int table, bool negative) {

		Texture[] images = table1;

		//if (testType == 0) {
		//	frame.enabled = false;
		//	contents.enabled = false;
		//	return;
		//} else {
			frame.enabled = negative;
			contents.enabled = true;
		//}



		if (negative) {
			switch (table) {
			case 0:
				images = table1;
				break;
			case 1:
				images = table2;
				break;
			case 2:
				images = table3;
				break;
			case 3:
				images = table4;
				break;
			case 4:
				images = table5;
				break;
			case 5:
				images = table6;
				break;
			case 6:
				images = table7;
				break;
			}
		} else {
			images = positives;
		}


		int r = Random.Range (0, images.Length);

		contents.texture = images [r];

	}

}
