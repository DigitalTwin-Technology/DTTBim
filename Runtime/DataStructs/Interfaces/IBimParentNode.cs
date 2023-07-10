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
        NodeObjectType this[int index]
        {
            get;
            set;
        }

        void AddMesh(string id, Mesh mesh,
            Material[] sharedMaterials,
            Option<ICombineRendererBuilder> combineRendererBuilder,
            bool addMeshCollider = false);

        void AddBimDataNode(string id, string parentId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer, bool useLocalPostion);

        NodeObjectType_ FindNodeObjectById<NodeObjectType_>(string name) where NodeObjectType_ : NodeType;
    }
}

