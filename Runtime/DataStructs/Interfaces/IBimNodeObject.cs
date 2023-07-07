// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public interface IBimNodeObject<NodeType> : IMetaDataNode<NodeType, IMetaData>
    {
        void AddBimDataNode(uint nodeIndex, string id, Vector3 position, Quaternion rotation, Vector3 scale, int layer);
    }
}

