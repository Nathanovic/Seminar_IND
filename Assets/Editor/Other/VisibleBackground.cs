using UnityEngine;
using UnityEditor;

public static class VisibleBackground {

	private static ScreenInteractor background;

	//onDisable on ScreenInteractor.cs
	public static void TryChangingBackground(ScreenInteractor bg){
		background = null;

		if (Selection.activeTransform != null && Selection.activeTransform.parent == bg.transform) {
			background = bg;			
		}
		else {
			bg.DeactivateSelf ();
		}
	}

	//onDisable on InteractionButton.cs
	public static void TryChangingBackground(){
		if (background == null) { 
			return;
		}
		 
		if (Selection.activeTransform == null || Selection.activeTransform.parent == null || Selection.activeTransform.parent != background.transform) {
			background.DeactivateSelf ();
		}
	}
}
