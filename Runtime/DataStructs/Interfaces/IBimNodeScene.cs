// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public interface IBimNodeScene<NodeType> : IMetaDataNode<NodeType, IMetaData>
    {
        IMetaDataNode<NodeType, IMetaData> AddBimParentNode(IDataNodeBuilder bimParentNodeBuilder);
    }
}

