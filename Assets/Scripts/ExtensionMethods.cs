using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

	public static void AddUnique<T>(this List<T> list, T item){
		if (!list.Contains (item)) {
			list.Add (item);
		}
	}
}
