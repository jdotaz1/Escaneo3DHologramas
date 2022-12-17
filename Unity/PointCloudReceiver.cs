using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


#if WINDOWS_UWP
using NetworkCommunication;
#else
using System.Net.Sockets;
using System.Threading;
#endif


public class PointCloudReceiver : MonoBehaviour
{
#if WINDOWS_UWP
    TransferSocket socket;
#else
    TcpClient socket;
#endif
    public int port = 48002;
    public List<Camera> camaras;
    PointCloudRenderer pointCloudRenderer;
    bool bReadyForNextFrame = true;
    bool bConnected = false;

    
    float miny=999, maxy=-999,mediay;
    float minx = 999, maxx = -999, mediax;
    float minz = 999, maxz = -999, mediaz;


    //varsettings
    bool camset = false;
    bool viewport = false;
    bool cams = false;
    bool cambgblack = false;
    bool scale = false;
    bool zoom = false;
    
    int selected=0;
    void Start()
    {
        pointCloudRenderer = GetComponent<PointCloudRenderer>();
    }

    void Update()
    {


        if (!bConnected)
            return;
        if (Input.GetKeyUp(KeyCode.Q)) camset = !camset;
       
        
        if (camset)
        {
            settingcamera();
        }
        else
        {
            if (!cambgblack) resetcameraBackgrounds();
            float[] vertices;
            byte[] colors;

            if (bReadyForNextFrame)
            {
                Debug.Log("Requesting frame");

#if WINDOWS_UWP
            socket.RequestFrame();
            socket.ReceiveFrameAsync();
#else
                RequestFrame();
#endif
                bReadyForNextFrame = false;
            }

#if WINDOWS_UWP
        if (socket.GetFrame(out vertices, out colors))
#else
            if (ReceiveFrame(out vertices, out colors))
#endif
            {
                Debug.Log("Frame received");

                fixcameras(ref vertices);



                pointCloudRenderer.Render(vertices, colors);
                bReadyForNextFrame = true;
            }
        }
    }

    void fixcameras(ref float[] vertices )
    {
        //for (int i = 0; i < vertices.Length; i += 3)
        //{
        //    maxx = (maxx < vertices[i]) ? vertices[i] : maxx;
        //    minx = (minx > vertices[i]) ? vertices[i] : minx;

        //    maxy = (maxy < vertices[i+1]) ? vertices[i + 1] : maxy;
        //    miny = (miny > vertices[i + 1]) ? vertices[i + 1] : miny;

        //    maxz = (maxz < vertices[i+2]) ? vertices[i + 2] : maxz;
        //    minz = (minz > vertices[i + 2]) ? vertices[i + 2] : minz;
        //}
        //mediax = (maxx + minx) / 2;
        //mediay = (maxy + miny) / 2;
        //mediaz = (maxz + minz) / 2;
        //foreach(Camera c in camaras)
        //{
        //    c.transform.position = new Vector3(c.transform.position.x, mediay, c.transform.position.z);
        //}

        //camaras[0].transform.position = new Vector3(mediax, mediay, mediaz+2);
        //camaras[1].transform.position = new Vector3(mediax + 2, mediay, mediaz);
        //camaras[2].transform.position = new Vector3(mediax - 2, mediay, mediaz);
        //camaras[3].transform.position = new Vector3(mediax, mediay, mediaz - 2);

    }

    void resetcameraBackgrounds()
    {
        foreach(Camera c in camaras)
        {
            c.backgroundColor = Color.black;
        }
        cambgblack = true;
    }
    void settingcamera()
    {
        
        Camera c1, c2;
        if (Input.GetKeyUp(KeyCode.Tab)) cams = !cams;
        if (Input.GetKeyUp(KeyCode.Z)) zoom = !zoom;
        if (Input.GetKeyUp(KeyCode.V)) viewport = !viewport;

        if (cams)
        {
            c1 = camaras[0];
            c2 = camaras[3];
        }
        else
        {
            c1 = camaras[1];
            c2 = camaras[2];
        }

        resetcameraBackgrounds();
        c1.backgroundColor = Color.green;
        c2.backgroundColor = Color.green;
        cambgblack = false;

        if (viewport)
        {
            foreach (Camera c in camaras)
            {
                c.backgroundColor = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                scale = !scale;
               
            }

            if (scale)
            {
                
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    foreach(Camera c in camaras)
                    {
                        Rect r = c.rect;
                        r.height += 0.01f;
                        r.width = r.height / 2;
                        r.y -= (0.01f / 2);
                        r.x -= (0.01f / 4);
                        c.rect = r;
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    foreach (Camera c in camaras)
                    {
                        Rect r = c.rect;
                        r.height -= 0.01f;
                        r.width = r.height / 2;
                        r.y += (0.01f / 2);
                        r.x += (0.01f / 4);
                        c.rect = r;
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    foreach (Camera c in camaras)
                    {
                        Rect r = c.rect;
                        float move;
                        if (r.center.y == 0.5)
                        {
                            move = (0.5 - r.x < 0) ? 0.01f : -0.01f;
                            r.x +=move;

                        }
                        else
                        {
                            move = (0.5 - r.y < 0) ? 0.01f : -0.01f;
                            r.y += move;
                        }
                        
                        c.rect = r;
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    foreach (Camera c in camaras)
                    {
                        Rect r = c.rect;
                        float move;
                        if (r.center.y == 0.5)
                        {
                            move = (0.5 - r.x < 0) ? 0.01f : -0.01f;
                            r.x -= move;

                        }
                        else
                        {
                            move = (0.5 - r.y < 0) ? 0.01f : -0.01f;
                            r.y -= move;
                        }

                        c.rect = r;
                    }
                }

            }

        }
        else
        {
            if (zoom){
                
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    c1.orthographicSize -= 0.01f;
                    c2.orthographicSize -= 0.01f;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    c1.orthographicSize += 0.01f;
                    c2.orthographicSize += 0.01f;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    c1.transform.position += (cams) ? new Vector3(+.1f,0, 0) : new Vector3(0, .1f, 0);
                    c2.transform.position += (cams) ? new Vector3(+.1f, 0, 0) : new Vector3(0, .1f, 0);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    c1.transform.position += (cams) ? new Vector3(-.1f,0, 0 ):new Vector3(0, -.1f, 0);
                    c2.transform.position += (cams) ? new Vector3(-.1f,0, 0) : new Vector3(0, -.1f, 0);
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    
                    c1.transform.position += (!cams) ? new Vector3(0, 0, +.1f): new Vector3( 0,-.1f, 0);
                    c2.transform.position += (!cams) ? new Vector3(0, 0, +.1f): new Vector3( 0,-.1f, 0);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    c1.transform.position += (!cams) ? new Vector3(0, 0, -.1f) : new Vector3( 0,.1f, 0);
                    c2.transform.position += (!cams) ? new Vector3(0, 0, -.1f) : new Vector3( 0,.1f, 0);
                }
            }
            
        }
        
    }
    
    
    public void Connect(string IP)
        {
#if WINDOWS_UWP
        socket = new NetworkCommunication.TransferSocket(IP, port);
#else
            socket = new TcpClient(IP, port);
#endif
            bConnected = true;
            Debug.Log("Coonnected");
    }

    //Frame receiving for the editor
#if WINDOWS_UWP
#else
    void RequestFrame()
    {
        byte[] byteToSend = new byte[1];
        byteToSend[0] = 0;

        socket.GetStream().Write(byteToSend, 0, 1);
    }

    int ReadInt()
    {
        byte[] buffer = new byte[4];
        int nRead = 0;
        while (nRead < 4)
            nRead += socket.GetStream().Read(buffer, nRead, 4 - nRead);

        return BitConverter.ToInt32(buffer, 0);
    }

    bool ReceiveFrame(out float[] lVertices, out byte[] lColors)
    {
        int nPointsToRead = ReadInt();

        lVertices = new float[3 * nPointsToRead];
        short[] lShortVertices = new short[3 * nPointsToRead];
        lColors = new byte[3 * nPointsToRead];


        int nBytesToRead = sizeof(short) * 3 * nPointsToRead;
        int nBytesRead = 0;
        byte[] buffer = new byte[nBytesToRead];

        while (nBytesRead < nBytesToRead)
            nBytesRead += socket.GetStream().Read(buffer, nBytesRead, Math.Min(nBytesToRead - nBytesRead, 64000));

        System.Buffer.BlockCopy(buffer, 0, lShortVertices, 0, nBytesToRead);

        for (int i = 0; i < lShortVertices.Length; i++)
            lVertices[i] = lShortVertices[i] / 1000.0f;

        nBytesToRead = sizeof(byte) * 3 * nPointsToRead;
        nBytesRead = 0;
        buffer = new byte[nBytesToRead];

        while (nBytesRead < nBytesToRead)
            nBytesRead += socket.GetStream().Read(buffer, nBytesRead, Math.Min(nBytesToRead - nBytesRead, 64000));

        System.Buffer.BlockCopy(buffer, 0, lColors, 0, nBytesToRead);

        return true;
    }
#endif
}
