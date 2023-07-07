// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimNodeObject : DataNodeBase, IBimNodeObject<DataNodeBase>
    {
        [SerializeField] private MetaDataBimNode _metaDataName;

        public void AddBimDataNode(uint nodeIndex, string id, Vector3 position, Quaternion rotation, Vector3 scale, int layer = 0)
        {
            AddNode(new BimNodeObjectBuilder(nodeIndex, id, null, position, rotation, scale, layer), Option<DataNodeBase>.None);
        }
    }
}

