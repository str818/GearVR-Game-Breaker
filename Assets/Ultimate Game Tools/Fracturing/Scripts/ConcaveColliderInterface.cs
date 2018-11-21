using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace UltimateFracturing
{
    public static class ConcaveColliderInterface
    {
        [StructLayout(LayoutKind.Sequential)]
        struct SConvexDecompositionInfoInOut
        {
       	    public uint     uMaxHullVertices;
	        public uint     uMaxHulls;
	        public float    fPrecision;
            public float    fBackFaceDistanceFactor;
            public uint     uLegacyDepth;
            public uint     uNormalizeInputMesh;
            public uint     uUseFastVersion;

            public uint     uTriangleCount;
            public uint     uVertexCount;

	        // Out parameters

	        public int      nHullsOut;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SConvexDecompositionHullInfo
        {
	        public int      nVertexCount;
	        public int      nTriangleCount;
        };

        [DllImport("ConvexDecompositionDll")]
        private static extern void DllInit(bool bUseMultithreading);

        [DllImport("ConvexDecompositionDll")]
        private static extern void DllClose();

        [DllImport("ConvexDecompositionDll")]
        private static extern void SetLogFunctionPointer(IntPtr pfnUnity3DLog);

        [DllImport("ConvexDecompositionDll")]
        private static extern void SetProgressFunctionPointer(IntPtr pfnUnity3DProgress);

        [DllImport("ConvexDecompositionDll")]
        private static extern void CancelConvexDecomposition();

        [DllImport("ConvexDecompositionDll")]
        private static extern bool DoConvexDecomposition(ref SConvexDecompositionInfoInOut infoInOut, Vector3[] pfVertices, int[] puIndices);

        [DllImport("ConvexDecompositionDll")]
        private static extern bool GetHullInfo(uint uHullIndex, ref SConvexDecompositionHullInfo infoOut);

        [DllImport("ConvexDecompositionDll")]
        private static extern bool FillHullMeshData(uint uHullIndex, ref float pfVolumeOut, int[] pnIndicesOut, Vector3[] pfVerticesOut);

        public static int ComputeHull(GameObject gameObject, int nMaxHullVertices, bool bVerbose)
        {
            int nTotalTriangles = 0;

            MeshFilter theMesh = (MeshFilter)gameObject.GetComponent<MeshFilter>();

            DllInit(true);

            SConvexDecompositionInfoInOut info = new SConvexDecompositionInfoInOut();

            if(theMesh)
            {
                if(theMesh.sharedMesh)
                {
                    info.uMaxHullVertices        = (uint)(Mathf.Max(3, nMaxHullVertices));
	                info.uMaxHulls               = 1;
	                info.fPrecision              = 0.8f;
                    info.fBackFaceDistanceFactor = 0.2f;
                    info.uLegacyDepth            = 0;
                    info.uNormalizeInputMesh     = 0;
                    info.uUseFastVersion         = 1;

	                info.uTriangleCount          = (uint)theMesh.sharedMesh.triangles.Length / 3;
                    info.uVertexCount            = (uint)theMesh.sharedMesh.vertexCount;

                    Vector3[] av3Vertices = theMesh.sharedMesh.vertices;

                    if(DoConvexDecomposition(ref info, av3Vertices, theMesh.sharedMesh.triangles))
                    {
                        for(int nHull = 0; nHull < info.nHullsOut; nHull++)
                        {
                            SConvexDecompositionHullInfo hullInfo = new SConvexDecompositionHullInfo();
                            GetHullInfo((uint)nHull, ref hullInfo);

                            if(hullInfo.nTriangleCount > 0)
                            {
							    Vector3[] hullVertices = new Vector3[hullInfo.nVertexCount];
                                int[]     hullIndices  = new int[hullInfo.nTriangleCount * 3];
							
							    float fHullVolume = -1.0f;

                                FillHullMeshData((uint)nHull, ref fHullVolume, hullIndices, hullVertices);

                                Mesh hullMesh = new Mesh();
                                hullMesh.vertices  = hullVertices;
                                hullMesh.triangles = hullIndices;
                                hullMesh.uv        = new Vector2[hullVertices.Length];
                                hullMesh.RecalculateNormals();

                                GameObject goNewHull = new GameObject("Hull " + (nHull + 1));
                                goNewHull.transform.position   = gameObject.transform.position;
                                goNewHull.transform.rotation   = gameObject.transform.rotation;
                                goNewHull.transform.localScale = gameObject.transform.localScale;
                                goNewHull.transform.parent     = gameObject.transform;
                                MeshCollider meshCollider = goNewHull.AddComponent<MeshCollider>() as MeshCollider;

                                meshCollider.sharedMesh = null;
							    meshCollider.sharedMesh = hullMesh;
							    meshCollider.convex     = true;

                                nTotalTriangles += hullInfo.nTriangleCount;
                            }
                            else
                            {
                                if(bVerbose)
                                {
//                                    Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned 0 triangles. Approximating with another collider.");
                                }
                            }
                        }

                        if(info.nHullsOut == 0 && bVerbose)
                        {
//                            Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned 0 hulls. Approximating with another collider.");
                        }
                    }
                    else
                    {
                        if(bVerbose)
                        {
//                            Debug.LogWarning("Error generating collider for " + gameObject.name + ": ComputeHull() returned false. Approximating with another collider.");
                        }
                    }
                }
            }

            DllClose();
            return nTotalTriangles;
	    }
    }
}