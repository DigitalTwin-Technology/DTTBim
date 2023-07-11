// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimNodeObject : DataNodeBase, IBimNodeObject<DataNodeBase>
    {

        public void SetNodeId(string nodeId)
        {
            ((MetaDataBimNode)Data).Id = nodeId;
            name = nodeId;
        }

        public DataNodeBase AddBimDataNode(string nodeId, uint nodeIndex,Vector3 position, Quaternion rotation, Vector3 scale, int layer = 0)
        {
            return (DataNodeBase)AddNode(new BimNodeObjectBuilder(nodeId, nodeIndex, null, position, rotation, scale, layer), Option<DataNodeBase>.None);
        }
    }
}

