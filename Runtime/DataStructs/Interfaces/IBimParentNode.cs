// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public interface IBimParentNode<NodeType> : IMetaDataNode<NodeType, IMetaData>
    {
        void AddBimDataNode(string id, string parentId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer, bool useLocalPostion);

        void AddMesh(string id, Mesh mesh, Material[] sharedMaterials, bool addMeshCollider = false);

        NodeObjectType FindNodeObjectById<NodeObjectType>(string name) where NodeObjectType : NodeType;
    }
}

