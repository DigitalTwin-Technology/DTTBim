// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimNodeObjectBuilder : IDataNodeBuilder
    {
        private MetaDataBimNode _metaDataBimNode;

        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;
        private int _layer;

        public IMetaData MetaData { get => _metaDataBimNode; set => _metaDataBimNode = (MetaDataBimNode)value; }

        public BimNodeObjectBuilder(string nodeId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer )
        {
            _metaDataBimNode = new MetaDataBimNode(nodeIndex, nodeId);

            _position = position;
            _rotation = rotation;   
            _scale = scale;
            _layer = layer;
        }

        public IMetaDataNode<NodeType, MetaDataType> Create<NodeType, MetaDataType>(IMetaDataNode<NodeType, MetaDataType> parent) where MetaDataType : IMetaData
        {
            BimNodeObject newBimNodeObject = (new GameObject(_metaDataBimNode.Id)).AddComponent<BimNodeObject>();
            newBimNodeObject.Id = _metaDataBimNode.Id;
            newBimNodeObject.Data = _metaDataBimNode;

            newBimNodeObject.Header = ((DataNodeBase)parent.Node).Header;
            newBimNodeObject.Parent = ((DataNodeBase)parent.Node);

            newBimNodeObject.transform.localScale = _scale;
            newBimNodeObject.transform.localPosition = _position;
            newBimNodeObject.transform.localRotation = _rotation;

            newBimNodeObject.gameObject.layer = _layer;

            return (IMetaDataNode<NodeType, MetaDataType>)newBimNodeObject;
        }
    }
}

