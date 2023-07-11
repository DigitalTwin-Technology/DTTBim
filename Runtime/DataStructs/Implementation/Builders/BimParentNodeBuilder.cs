// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimParentNodeBuilder : IDataNodeBuilder
    {
        private MetaDataName _metaDataName;

        public IMetaData MetaData { get => _metaDataName; set => _metaDataName = (MetaDataName)value; }

        public BimParentNodeBuilder(string name)
        {
            _metaDataName = new MetaDataName(name);
        }

        public IMetaDataNode<NodeType, MetaDataType> Create<NodeType, MetaDataType>(IMetaDataNode<NodeType, MetaDataType> parent) where MetaDataType : IMetaData
        {
            BimNodeParent newBimNodeParent = new GameObject(_metaDataName.Name).AddComponent<BimNodeParent>();
            newBimNodeParent.Header = ((DataNodeBase)parent.Node).Header;
            newBimNodeParent.Parent = ((DataNodeBase)parent.Node);
            return (IMetaDataNode<NodeType, MetaDataType>)newBimNodeParent;
        }
    }
}

