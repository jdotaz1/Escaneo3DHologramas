using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System;


public class readsavedframes : MonoBehaviour
{

   // Start is called before the first frame update
    private StreamReader sr1,sr2;
    void Start()
    {

        sr1 = new StreamReader("D:\\IHCnuevo program\\LiveScan3D-master\\bin\\f00001.ply.txt");
        sr2 = new StreamReader("D:\\IHCnuevo program\\LiveScan3D-master\\bin\\f00001_triangulos.txt");

        string[] data= sr1.ReadLine().Split(",");
        List<Vector3> puntos= new List<Vector3>();
        for(int i=0;i< data.Length-1; i+=3)
        {
            puntos.Add(new Vector3(float.Parse(data[i]), float.Parse(data[i + 1]), float.Parse(data[i + 2])));
        }
        data =sr2.ReadLine().Split(",");
        List<int> triangulos = new List<int>();
        for (int i = 0; i < data.Length - 1; i++)
        {
            triangulos.Add(int.Parse(data[i]));
        }
        sr1.Close();
        sr2.Close();
        int mod = triangulos.Count % 3;
        while (mod != 0)
        {
            triangulos.RemoveAt(triangulos.Count - 1);
            mod--;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = puntos.ToArray() ;
        mesh.triangles=triangulos.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
