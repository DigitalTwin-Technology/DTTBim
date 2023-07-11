// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public interface IBimNodeObject<NodeType> : IMetaDataNode<NodeType, IMetaData>
    {
        void SetNodeId(string nodeId);

        NodeType AddBimDataNode(string nodeId, uint nodeIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer);
    }
}

