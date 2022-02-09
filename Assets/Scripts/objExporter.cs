using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
//using UnityEditor;
using System;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class ObjExporterScript : MonoBehaviour
{
	private static int StartIndex = 0;

	public static void Start()
	{
		StartIndex = 0;
	}
	public static void End()
	{
		StartIndex = 0;
	}


	public static string MeshToString(MeshFilter mf, Transform t)
	{
		Vector3 s = t.localScale;
		Vector3 p = t.localPosition;
		Quaternion r = t.localRotation;


		int numVertices = 0;
		Mesh m = mf.sharedMesh;
		if (!m)
		{
			return "####Error####";
		}
		Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

		StringBuilder sb = new StringBuilder();

		foreach (Vector3 vv in m.vertices)
		{
			Vector3 v = t.TransformPoint(vv);
			numVertices++;
			sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
		}
		sb.Append("\n");
		foreach (Vector3 nn in m.normals)
		{
			Vector3 v = r * nn;
			sb.Append(string.Format("vn {0} {1} {2}\n", -v.x, -v.y, v.z));
		}
		sb.Append("\n");
		foreach (Vector3 v in m.uv)
		{
			sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
		}
		for (int material = 0; material < m.subMeshCount; material++)
		{
			sb.Append("\n");
			sb.Append("usemtl ").Append(mats[material].name).Append("\n");
			sb.Append("usemap ").Append(mats[material].name).Append("\n");

			int[] triangles = m.GetTriangles(material);
			for (int i = 0; i < triangles.Length; i += 3)
			{
				sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
					triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
			}
		}

		StartIndex += numVertices;
		return sb.ToString();
	}
}

public class ObjExporter : MonoBehaviour
{
	//[MenuItem("File/Export/Wavefront OBJ")]
	//static void DoExportWSubmeshes()
	//{
	//	DoExport(true);
	//}

	//[MenuItem("File/Export/Wavefront OBJ (No Submeshes)")]
	//static void DoExportWOSubmeshes()
	//{
	//	DoExport(false);
	//}


	public static void DoExport(bool makeSubmeshes, GameObject GrandPa, string file_Path,string id)//it was not public 
	{
		if (GrandPa.transform.childCount == 0)
		{
			Debug.Log("Didn't Export Any Meshes; Nothing was selected!");
			return;
		}
		//new addition TO SAVE FILE NAME
		System.Random rand = new System.Random();//GENERATES RANDOM NUMBER to get ranom Name for the object

		//string meshName = GrandPa.name + rand.Next(); //string meshName = Selection.gameObjects[0].name ;
		//new addition TO SPECIFY DIRECTORY
		string meshName = id;
		//string fileName = EditorUtility.SaveFilePanel("Export .obj file", @"D:\Unity\Simp_Win_Cube\OBJ's_wavefronts", meshName, "obj");//Code changed here INPUT PATH HERE
		//string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", meshName, "obj");
		string fileName = file_Path + @"\" + meshName + ".obj";

		ObjExporterScript.Start();

		StringBuilder meshString = new StringBuilder();

		meshString.Append("#" + meshName + ".obj"
							+ "\n#" + System.DateTime.Now.ToLongDateString()
							+ "\n#" + System.DateTime.Now.ToLongTimeString()
							+ "\n#-------"
							+ "\n\n");

		Transform t = GrandPa.transform;

		/*
		//NEW addition Probuilder MEsh

		var filter = t.GetComponent<MeshFilter>();

		// Add a new uninitialized pb_Object
		var mesh = GrandPa.AddComponent<ProBuilderMesh>();

		// Create a new MeshImporter
		var importer = new MeshImporter(mesh);

		// Import from a GameObject. In this case we're loading and assigning to the same GameObject, but you may
		// load and apply to different Objects as well.
		importer.Import(filter.sharedMesh);

		// Since we're loading and setting from the same object, it is necessary to create a new mesh to avoid
		// overwriting the mesh that is being read from.
		filter.sharedMesh = new Mesh();
		*/
		Vector3 originalPosition = t.position;
		t.position = Vector3.zero;

		if (!makeSubmeshes)
		{
			meshString.Append("g ").Append(t.name).Append("\n");
		}
		meshString.Append(processTransform(t, makeSubmeshes));

		WriteToFile(meshString.ToString(), fileName);

		t.position = originalPosition;

		ObjExporterScript.End();
		Debug.Log("Exported Mesh: " + fileName);
	}

	static string processTransform(Transform t, bool makeSubmeshes)
	{
		StringBuilder meshString = new StringBuilder();

		meshString.Append("#" + t.name
						+ "\n#-------"
						+ "\n");

		if (makeSubmeshes)
		{
			meshString.Append("g ").Append(t.name).Append("\n");
		}

		MeshFilter mf = t.GetComponent<MeshFilter>();


		if (mf)
		{
			meshString.Append(ObjExporterScript.MeshToString(mf, t));
		}

		for (int i = 0; i < t.childCount; i++)
		{
			meshString.Append(processTransform(t.GetChild(i), makeSubmeshes));
		}

		return meshString.ToString();
	}

	static void WriteToFile(string s, string filename)
	{
		using (StreamWriter sw = new StreamWriter(filename))
		{
			sw.Write(s);
			
		}
	}

	internal static void MeshToFile(MeshFilter meshFilter, string path)
	{
		throw new NotImplementedException();
	}
}


/*
	public static string MeshToString(MeshFilter mf)
	{
		Mesh m = mf.sharedMesh;
		Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

		StringBuilder sb = new StringBuilder();

		sb.Append("g ").Append(mf.name).Append("\n");


		foreach (Vector3 v in m.vertices)
		{
			sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
		}
		sb.Append("\n");


		foreach (Vector3 v in m.normals)
		{
			sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
		}
		sb.Append("\n"); 


		foreach (Vector3 v in m.uv)
		{
			sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
		}
		for (int material = 0; material < m.subMeshCount; material++)
		{
			sb.Append("\n");



			//        sb.Append("usemtl ").Append(mats[material].name).Append("\n");
			//        sb.Append("usemap ").Append(mats[material].name).Append("\n");

			int[] triangles = m.GetTriangles(material);
			for (int i = 0; i < triangles.Length; i += 3)
			{
				sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
									   triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
			}
		}
		return sb.ToString();
	}

	public static void MeshToFile(MeshFilter mf, string filename)
	{
		using (StreamWriter sw = new StreamWriter(filename))
		{
			sw.Write(MeshToString(mf));
		}
		}
	*/


