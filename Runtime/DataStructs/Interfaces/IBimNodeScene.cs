// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using DTTUnityCore.DataStructs;
using DTTUnityCore.Functional;
using System;

namespace DTTBim.DataStructs
{
    public interface IBimNodeScene<NodeType, BimParentNodeType> : IMetaDataNode<NodeType, IMetaData>
    {
        BimParentNodeType BimParentNode { get; }

        NodeType CombineRendererNode { get; }

        void AddCombineRender(ICombineRendererBuilder combineRendererBuilder);

        IMetaDataNode<NodeType, IMetaData> AddBimParentNode(IDataNodeBuilder bimParentNodeBuilder);
    }
}

