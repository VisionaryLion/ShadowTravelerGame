using UnityEditor;
using UnityEngine;
using System.Collections;

public class Tags : AssetPostprocessor {
	
	static Tags(){
		Create ();
	}
	
	/*
	* The asset need the Layer Ground on User Layer 8 and the Layer Enemy on User Layer 9
	* This script starts automatically and set the Layers to the project settings
	*/
	static void Create(){
		
		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		
		SerializedProperty it = tagManager.GetIterator();
		bool showChildren = true;

		bool groundIsSet = false;
		bool enemyIsSet = false;
		bool playerIsSet = false;
		bool wallIsSet = false;

		while (it.NextVisible(showChildren)){
			
			bool isSet = false;
			
			if(it.name.Contains("User Layer")){
				
				if(it.stringValue.Equals("Ground")){
					groundIsSet = true;
				}
				
				if(it.stringValue.Equals("Enemy")){
					enemyIsSet = true;
				}

				if(it.stringValue.Equals("Player")){
					playerIsSet = true;
				}

				if(it.stringValue.Equals("Wall")){
					wallIsSet = true;
				}

				if(it.stringValue.Equals("")){
					
					if(!groundIsSet){
						groundIsSet = true;
						it.stringValue = "Ground";
						isSet = true;
					}
					
					if(!isSet){
						if(!enemyIsSet){
							enemyIsSet = true;
							it.stringValue = "Enemy";
							isSet = true;
						}
					}

					if(!isSet){
						if(!playerIsSet){
							playerIsSet = true;
							it.stringValue = "Player";
							isSet = true;
						}
					}

					if(!isSet){
						if(!wallIsSet){
							wallIsSet = true;
							it.stringValue = "Wall";
							isSet = true;
						}
					}
				}
			}
		}
		
		SerializedProperty layerProp = tagManager.FindProperty("layers");
		
		// Unity 5 Tag-Fix
		if (!groundIsSet || !enemyIsSet) {
			for (int i = 8; i <= 31; i++) {
				SerializedProperty sp = layerProp.GetArrayElementAtIndex(i);
				
				if(sp != null){
					if(sp.stringValue.Equals("Ground")){
						groundIsSet = true;
					}
					
					if(sp.stringValue.Equals("Enemy")){
						enemyIsSet = true;
					}

					if(sp.stringValue.Equals("Player")){
						playerIsSet = true;
					}

					if(sp.stringValue.Equals("Wall")){
						wallIsSet = true;
					}
				}
			}
			
			
			if (!groundIsSet) {
				
				for(int i = 8; i <= 31; i++){
					
					SerializedProperty sp = layerProp.GetArrayElementAtIndex(i);
					
					if(sp != null){
						if(sp.stringValue.Equals("")){
							sp.stringValue = "Ground";
							groundIsSet = true;
							
							break;
						}
					}
				}
			}
			
			if (!enemyIsSet) {
				
				for(int i = 8; i <= 31; i++){
					
					SerializedProperty sp = layerProp.GetArrayElementAtIndex(i);
					
					if(sp != null){
						if(sp.stringValue.Equals("")){
							sp.stringValue = "Enemy";
							enemyIsSet = true;
							break;
						}
					}
				}
			}

			if (!playerIsSet) {
				
				for(int i = 8; i <= 31; i++){
					
					SerializedProperty sp = layerProp.GetArrayElementAtIndex(i);
					
					if(sp != null){
						if(sp.stringValue.Equals("")){
							sp.stringValue = "Player";
							playerIsSet = true;
							break;
						}
					}
				}
			}

			if (!wallIsSet) {
				
				for(int i = 8; i <= 31; i++){
					
					SerializedProperty sp = layerProp.GetArrayElementAtIndex(i);
					
					if(sp != null){
						if(sp.stringValue.Equals("")){
							sp.stringValue = "Wall";
							wallIsSet = true;
							break;
						}
					}
				}
			}
		}


		SerializedProperty tagsProp = tagManager.FindProperty("tags");

		generateTag ("PlayerBullet", tagsProp);
		generateTag ("FieldTop", tagsProp);
		generateTag ("FieldEdge", tagsProp);
		generateTag ("Enemy", tagsProp);
		generateTag ("Shot", tagsProp);
		generateTag ("Player", tagsProp);

		tagManager.ApplyModifiedProperties();
		tagManager.Update ();
	}

	private static void generateTag(string tag, SerializedProperty tagsProp) {
		
		bool foundFieldTop = false;
		for (int i = 0; i < tagsProp.arraySize; i++)
		{
			SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
			if (t.stringValue.Equals(tag)) { foundFieldTop = true; break; }
		}

		// if not found, add it
		if (!foundFieldTop)
		{
			tagsProp.InsertArrayElementAtIndex(0);
			SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
			n.stringValue = tag;
		}
	}
}