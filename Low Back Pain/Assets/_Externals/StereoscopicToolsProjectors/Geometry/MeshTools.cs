using UnityEngine;

namespace UnityTools.Geometry
{
	public static class Mesh
	{
		public static UnityEngine.Mesh CreatePlane(float width = 1.0f, float height = 1.0f)
		{
			UnityEngine.Mesh plane = new UnityEngine.Mesh();
			plane.vertices = new Vector3[4];
			plane.uv = new Vector2[4];
			plane.triangles = new int[6];
			UpdatePlane(plane, width, height);
			return plane;
		}

		public static void UpdatePlane(UnityEngine.Mesh plane, float width, float height)
		{
			float halfWidth = 0.5f * width;
			float halfHeight = 0.5f * height;

			Vector3[] vertices = plane.vertices;
			vertices[0].Set(-halfWidth, -halfHeight, 0.0f);
			vertices[1].Set(halfWidth, -halfHeight, 0.0f);
			vertices[2].Set(halfWidth, halfHeight, 0.0f);
			vertices[3].Set(-halfWidth, halfHeight, 0.0f);
			plane.vertices = vertices;

			Vector2[] uv = plane.uv;
			uv[0].Set(0.0f, 0.0f);
			uv[1].Set(0.0f, 1.0f);
			uv[2].Set(1.0f, 1.0f);
			uv[3].Set(1.0f, 0.0f);
			plane.uv = uv;

			int[] triangles = plane.triangles;
			triangles[0] = 0;
			triangles[1] = 1;
			triangles[2] = 2;
			triangles[3] = 0;
			triangles[4] = 2;
			triangles[5] = 3;
			plane.triangles = triangles;

			plane.RecalculateNormals();
		}
	}
}