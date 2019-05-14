using UnityEngine;
using System.Collections;

public class FamilyMember {

	public Family member;
	public Mood mood;
	public int family;

}

public class TestAux {

	// a couple of black-box functions

	// cat is in range 1..7 (no zeroes!)
	public static bool isCombinationValid(Family member, bool changingRole, int cat) {


		// bottom part of the diagram
		if (changingRole == true) {

			if ((member == Family.father) || (member == Family.mother)) {

				if ((cat == 1) || (cat == 3) || (cat == 5) || (cat == 6))
					return false;
				else
					return true;

			} 

			else {

				if ((cat == 2) || (cat == 4) || (cat == 7))
					return false;
				else
					return true;

			}

		} 


		// top part
		else {

			if ((member == Family.father) || (member == Family.mother)) {

				if ((cat == 2) || (cat == 3) || (cat == 5) || (cat == 6))
					return false;
				else
					return true;

			} 

			else {

				if ((cat == 1) || (cat == 4) || (cat == 7))
					return false;
				else
					return true;

			}

		}


	}

	// this only codifies the role-changing situation (bottom part)
	public static FamilyMember getRoleChangeMember(Family member, int table) {

		FamilyMember result = new FamilyMember ();
		result.mood = Mood.sad;

		if (member == Family.son) {

			if (table == 0) {
				result.member = Family.parents;
				result.family = 1;
				return result;
				}
			if (table == 2) {
				result.member = Family.daughter;
				result.family = 1;
				return result;
				}
			if (table == 4)
				{
					result.member = Family.daughter;
					result.family = 2;
					return result;
				}
			if (table == 5) {
					result.member = Family.son;
					result.family = 2;
					return result;
					
			}

			// 'impossible' case, just to prevent the compiler from throwing an error
			return result;

		} 

		else if (member == Family.daughter) {

			if (table == 0) {
					result.member = Family.parents;
					result.family = 1;
					return result;
				}
			if (table == 2) {
					result.member = Family.son;
					result.family = 1;
					return result;
				}
			if (table == 4) {
					result.member = Family.son;
					result.family = 2;
					return result;
				}
			if (table == 5) {
					result.member = Family.daughter;
					result.family = 2;
					return result;
				}
			// 'impossible' case, just to prevent the compiler from throwing an error
			return result;

		} 

		else if (member == Family.father) {

			if (table == 1) {
				result.member = Family.siblings;
				result.family = 1;
				return result;
			}
			if (table == 3) {
				result.member = Family.mother;
				result.family = 1;
				return result;
			}
			if (table == 6) {
				result.member = Family.father;
				result.family = 2;
				return result;
			}
			// 'impossible' case, just to prevent the compiler from throwing an error
			return result;

		} 

		else if (member == Family.mother) {

			if (table == 1)	{
				result.member = Family.siblings;
				result.family = 1;
				return result;
			}
			if (table == 3) {
				result.member = Family.father;
				result.family = 1;
				return result;
			}
			if (table == 6) {
				result.member = Family.mother;
				result.family = 2;
				return result;
			}

			// 'impossible' case, just to prevent the compiler from throwing an error
			return result;

		}

		// 'impossible' case, just to prevent the compiler from throwing an error
		return result;

	}

}
