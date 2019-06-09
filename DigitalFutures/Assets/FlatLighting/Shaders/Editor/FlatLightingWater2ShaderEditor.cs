using UnityEngine;
using UnityEditor;
using FlatLighting;
using System.Collections;

public class FlatLightingWater2ShaderEditor : FlatLightingShaderEditor {

	private MaterialProperty animationSpeed = null;
	private MaterialProperty animationRepeat = null;
	private MaterialProperty animationScale = null;
	private MaterialProperty normalAnimationSpeed = null;
	private MaterialProperty normalAnimationRepeat = null;
	private MaterialProperty normalAnimationScale = null;
	private MaterialProperty alpha = null;

	protected override void FindProperties(MaterialProperty[] properties) {
		base.FindProperties(properties);
		animationSpeed = FindProperty("_VertexAnimSpeed", properties);
		animationRepeat = FindProperty("_VertexAnimRepeat", properties);
		animationScale = FindProperty("_VertexAnimScale", properties);
		normalAnimationSpeed = FindProperty("_NormalAnimSpeed", properties);
		normalAnimationRepeat = FindProperty("_NormalAnimRepeat", properties);
		normalAnimationScale = FindProperty("_NormalAnimScale", properties);
		alpha = FindProperty("_Alpha", properties);
	}

	protected override void ShaderPropertiesGUI() {
		ShowWaveSettings();
		base.ShaderPropertiesGUI();
	}

	private void ShowWaveSettings() {
		using (new UITools.GUIVertical(UITools.VGroupStyle)) {
			UITools.Header(Labels.Water);
			EditorGUILayout.HelpBox("It's recomended to use only one axis at the time.", MessageType.Info, true);
			base.materialEditor.ShaderProperty(animationSpeed, animationSpeed.displayName);
			base.materialEditor.ShaderProperty(animationRepeat, animationRepeat.displayName);
			base.materialEditor.ShaderProperty(animationScale, animationScale.displayName);
			UITools.DrawSeparatorThinLine();
			base.materialEditor.ShaderProperty(normalAnimationSpeed, normalAnimationSpeed.displayName);
			base.materialEditor.ShaderProperty(normalAnimationRepeat, normalAnimationRepeat.displayName);
			base.materialEditor.ShaderProperty(normalAnimationScale, normalAnimationScale.displayName);
			UITools.DrawSeparatorThinLine();
			base.materialEditor.ShaderProperty(alpha, alpha.displayName);
		}
	}
}
