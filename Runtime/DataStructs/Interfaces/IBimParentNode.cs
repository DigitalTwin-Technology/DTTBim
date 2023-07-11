// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;
using DTTUnityCore.Functional;
using System;

namespace DTTBim.DataStructs
{
    public interface IBimParentNode<NodeType, NodeObjectType> : IMetaDataNode<NodeType, IMetaData>
    {
        NodeObjectType this[string id]
        {
            get;
            set;
        }

        NodeObjectType this[uint nodeIndex]
        {
            get;
            set;
        }

        void AddMesh(string id, Mesh mesh,
            Material[] sharedMaterials,
            Option<ICombineRendererBuilder> combineRendererBuilder,
            bool addMeshCollider = false);

        void AddMesh(uint nodeIndex, Mesh mesh,
            Material[] sharedMaterials,
            Option<ICombineRendererBuilder> combineRendererBuilder,
            bool addMeshCollider = false);

        void AddBimDataNodeById(string newNodeid, string parentId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer);

        void AddBimDataNodeByNodeIndex(uint newNodeIndex, uint? parentIndex, string nodeid, string parentId, Vector3 position, Quaternion rotation, Vector3 scale, int layer);
    }
}

