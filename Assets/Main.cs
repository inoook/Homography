using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
	
	public Texture2D target;
	public Texture2D source;
	
	// target size for the homography
	public int dstWidth = 100;
	public int dstHeight = 100;
	
	public Transform P1;
	public Transform P2;
	public Transform P3;
	public Transform P4;

	public Renderer targetRender;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// new Vector2 はsource Textureでのピクセル位置
		
		
		float srcW = source.width;
		float srcH = source.height;

		
		// src texture uv position
		Vector2[] uvs = new Vector2[4];
		uvs[0] = new Vector2(P1.localPosition.x/2, P1.localPosition.y/1) /10;
		uvs[1] = new Vector2(P2.localPosition.x/2, P2.localPosition.y/1) /10;
		uvs[2] = new Vector2(P3.localPosition.x/2, P3.localPosition.y/1) /10;
		uvs[3] = new Vector2(P4.localPosition.x/2, P4.localPosition.y/1) /10;
		
		Color32[] colors = Homography.GetTransformedColors( source, dstWidth, dstHeight, 
													new Vector2(uvs[0].x * srcW, uvs[0].y * srcH),
													new Vector2(uvs[1].x * srcW, uvs[1].y * srcH),
													new Vector2(uvs[2].x * srcW, uvs[2].y * srcH),
													new Vector2(uvs[3].x * srcW, uvs[3].y * srcH));
		if(target == null){
			target = new Texture2D(dstWidth, dstHeight);
		}
		target.SetPixels32(colors, 0);
		target.Apply();

		targetRender.material.mainTexture = target;
	}

}
