// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimNodeSceneBuilder : IDataNodeBuilder
    {
        private int _initialCapacity;
        private MetaDataName _metaDataName;

        public BimNodeSceneBuilder(string name, int initalCapacity = 0)
        {
            _metaDataName = new MetaDataName(name);
            _initialCapacity = initalCapacity;
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

            return (IMetaDataNode<NodeType, MetaDataType>)bimNodeScene;
        }
    }
}

