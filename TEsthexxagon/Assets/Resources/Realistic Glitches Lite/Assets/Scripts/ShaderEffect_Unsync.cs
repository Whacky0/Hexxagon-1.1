using System.Collections;
using UnityEngine;


public class ShaderEffect_Unsync : MonoBehaviour {

	public static ShaderEffect_Unsync instance;
	public enum Movement {JUMPING_FullOnly, SCROLLING_FullOnly, STATIC};
	public Movement movement = Movement.STATIC;
	public float speed = 1;
	private float position = 0;
	private Material material;

	void Awake ()
	{
		material = new Material( Shader.Find("Hidden/VUnsync") );
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			DontDestroyOnLoad(this);
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		position = speed * 0.1f;

		material.SetFloat("_ValueX", position);
		Graphics.Blit (source, destination, material);
	}
}
