
// http://www.nicoptere.net/AS3/homographie/blog/Homography.as
using UnityEngine;
using System.Collections;

public class Homography
{
	static public Color32[] GetTransformedColors (Texture2D bmpd, 
											  int destWidth, 
											  int destHeight, 
											  Vector2 p0, 
											  Vector2 p1, 
											  Vector2 p2, 
											  Vector2 p3)
	{
			
		if (p0 == null || p1 == null || p2 == null || p3 == null)
			return null;
			
		// resolving system
		float[] system = getSystem (new Vector2[]{ p0, p1, p2, p3 });
		
		// assigns each pixel to its new location
		int i;
		int j;
			
		float x;
		float y;
			
		float u;
		float v;
		Vector2 p;
		
		Color32[] pixcels = bmpd.GetPixels32 (0);
		Color32[] newPixcels = new Color32[destWidth * destHeight];
			
		for (i = 0; i < destWidth; i++) {
			x = i;
			for (j = 0; j < destHeight; j++) {
				y = j;
				p = invert (x / destWidth, y / destHeight, system);
					
				//target.SetPixel( i, j, bmpd.GetPixel((int)p.x, (int)p.y) );
				int setPixcelIndex = j * destWidth + i;
				int getPixcelIndex = (int)(p.y) * bmpd.width + (int)(p.x);
				newPixcels [setPixcelIndex] = pixcels [getPixcelIndex];
			}
		}
			
		return newPixcels;
			
	}

	static public byte[] GetTransformedBytes (byte[] pixcels, 
	                                           int srcWidth, int srcHeight,
	                                              int destWidth, int destHeight, 
	                                              Vector2 p0, 
	                                              Vector2 p1, 
	                                              Vector2 p2, 
	                                              Vector2 p3)
	{
		
		if (p0 == null || p1 == null || p2 == null || p3 == null)
			return null;
		
		// resolving system
		float[] system = getSystem (new Vector2[]{ p0, p1, p2, p3 });
		
		// assigns each pixel to its new location
		int i;
		int j;
		
		float x;
		float y;
		
		float u;
		float v;
		Vector2 p;

		byte[] newPixcels = new byte[destWidth * destHeight * 4];
		
		for (i = 0; i < destWidth; i++) {
			x = i;
			for (j = 0; j < destHeight; j++) {
				y = j;
				p = invert (x / destWidth, y / destHeight, system);

				int setPixcelIndex = j * destWidth + i;
				int getPixcelIndex = (int)(p.y) * srcWidth + (int)(p.x);
				newPixcels [setPixcelIndex*4+0] = pixcels [getPixcelIndex*4+0];
				newPixcels [setPixcelIndex*4+1] = pixcels [getPixcelIndex*4+1];
				newPixcels [setPixcelIndex*4+2] = pixcels [getPixcelIndex*4+2];
				newPixcels [setPixcelIndex*4+3] = pixcels [getPixcelIndex*4+3];
			}
		}
		
		return newPixcels;
	}

	
	static public Texture2D setTransform (Texture2D bmpd, 
											  int destWidth, 
											  int destHeight, 
											  Vector2 p0, 
											  Vector2 p1, 
											  Vector2 p2, 
											  Vector2 p3)
	{
			
		if (p0 == null || p1 == null || p2 == null || p3 == null)
			return null;
			
		// resolving system
		float[] system = getSystem (new Vector2[]{ p0, p1, p2, p3 });
			
		//creating the destination bitmapData
		Texture2D target = new Texture2D (destWidth, destHeight, TextureFormat.RGBA32, false);
		//target.lock();
		//bmpd.lock();
			
		// assigns each pixel to its new location
		int i;
		int j;
			
		float x;
		float y;
			
		float u;
		float v;
		Vector2 p;
		
		Color32[] pixcels = bmpd.GetPixels32 (0);
		Color32[] newPixcels = new Color32[target.width * target.height];
			
		for (i = 0; i < destWidth; i++) {
			x = i;
			for (j = 0; j < destHeight; j++) {
				y = j;
				p = invert (x / destWidth, y / destHeight, system);
					
				//target.SetPixel( i, j, bmpd.GetPixel((int)p.x, (int)p.y) );
				int setPixcelIndex = j * target.width + i;
				int getPixcelIndex = (int)(p.y) * bmpd.width + (int)(p.x);
				newPixcels [setPixcelIndex] = pixcels [getPixcelIndex];
			}
		}
			
		target.SetPixels32 (newPixcels, 0);
		target.Apply (false);
			
		return target;
			
	}
	
	/*
		// 
		static public Matrix4x4 getMatrix (	 Vector2 p0 , 
											  Vector2 p1 , 
											  Vector2 p2 , 
											  Vector2 p3  ){
			Matrix4x4 m = new Matrix4x4();
			if ( p0 == null || p1 == null || p2 == null || p3 == null ) return m;
			float[] system = getSystem( new Vector2[]{ p0, p1, p2, p3 } );
			m[0,0] = system[0]/ 128.0f;  m[0,1] = 0;  m[0,2] = system[2]/ 128.0f;  m[0,3] = 0;
		    m[1,0] = 0;  m[1,1] = system[1]/ 128.0f;  m[1,2] = system[3]/ 128.0f;  m[1,3] = 0;
		    m[2,0] = 0;  m[2,1] = 0;  m[2,2] = system[4]/ 128.0f;  m[2,3] = system[5]/ 128.0f;
		    m[3,0] = 0;  m[3,1] = 0;  m[3,2] = system[6]/ 128.0f;  m[3,3] = system[7]/ 128.0f;
			
			return m;	
		}
		*/
	
	static private float[] getSystem (Vector2[] P)
	{
		float[] system = new float[ 8 ];
		float sx = (P [0].x - P [1].x) + (P [2].x - P [3].x);
		float sy = (P [0].y - P [1].y) + (P [2].y - P [3].y);
			
		float dx1 = P [1].x - P [2].x;
		float dx2 = P [3].x - P [2].x;
		float dy1 = P [1].y - P [2].y;
		float dy2 = P [3].y - P [2].y;
		 
		float z = (dx1 * dy2) - (dy1 * dx2);
		float g = ((sx * dy2) - (sy * dx2)) / z;
		float h = ((sy * dx1) - (sx * dy1)) / z;
		 
		system [0] = P [1].x - P [0].x + g * P [1].x;
		system [1] = P [3].x - P [0].x + h * P [3].x;
		system [2] = P [0].x;
		system [3] = P [1].y - P [0].y + g * P [1].y;
		system [4] = P [3].y - P [0].y + h * P [3].y;
		system [5] = P [0].y;
		system [6] = g;
		system [7] = h;
		 
		return system;
	}
		 
	static private Vector2 invert (float u, float v, float[] system)
	{
		return new Vector2 ((system [0] * u + system [1] * v + system [2]) / (system [6] * u + system [7] * v + 1),
							 (system [3] * u + system [4] * v + system [5]) / (system [6] * u + system [7] * v + 1));
	}
	
	
}