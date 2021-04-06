using System.Collections;
using UnityEngine;


public class ShaderEffect_Tint : MonoBehaviour {

	public static ShaderEffect_Tint instance;
	public float y = 1;
	public float u = 1;
	public float v = 1;
//	public bool swapUV = false;
	private Material material;

	// Creates a private material used to the effect
	void Awake ()
	{
		material = new Material( Shader.Find("Hidden/Tint") );

		if (instance == null)
		{
			instance = this;
		}
		else
		{
			DontDestroyOnLoad(this);
		}

	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{

		material.SetFloat("_ValueX", y);
		material.SetFloat("_ValueY", u);
		material.SetFloat("_ValueZ", v);

		Graphics.Blit (source, destination, material);
	}
}
