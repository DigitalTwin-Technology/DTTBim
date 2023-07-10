// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;
using DTTUnityCore;
using DTTUnityCore.Functional;

namespace DTTBim.DataStructs
{
    public class BimNodeSceneBuilder : IDataNodeBuilder
    {
        private int _initialCapacity;
        private MetaDataName _metaDataName;

        IDataNodeBuilder _bimParentNodeBuilder;

        public BimNodeSceneBuilder(string name, IDataNodeBuilder bimParentNodeBuilder, int initalCapacity = 0)
        {
            GuardsClauses.ArgumentStringNullOrEmpty(name, "name");
            GuardsClauses.ArgumentNotNull(bimParentNodeBuilder, "dataNodeBuilder");

            _metaDataName = new MetaDataName(name);
            _initialCapacity = initalCapacity;
            _bimParentNodeBuilder = bimParentNodeBuilder;
        }

        public IMetaData MetaData { get => _metaDataName; set => _metaDataName = (MetaDataName)value; }

        public IMetaDataNode<NodeType, MetaDataType> Create<NodeType, MetaDataType>(IMetaDataNode<NodeType, MetaDataType> parent) where MetaDataType : IMetaData
        {
            BimNodeScene bimNodeScene = new GameObject(_metaDataName.Name).AddComponent<BimNodeScene>();

            if(_initialCapacity != 0)
            {
                bimNodeScene.Childs.Capacity = _initialCapacity;
                bimNodeScene.Childs.TrimExcess();
            }

            bimNodeScene.AddBimParentNode(_bimParentNodeBuilder);

            return (IMetaDataNode<NodeType, MetaDataType>)bimNodeScene;
        }
    }
}

