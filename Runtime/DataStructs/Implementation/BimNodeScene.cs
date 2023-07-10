// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;

namespace DTTBim.DataStructs
{
    public class BimNodeScene : DataNodeBase, IBimNodeScene<DataNodeBase, BimNodeParent>
    {
        private BimNodeParent _bimParentNode;
        private DataNodeBase _combineRendererParentNode;

        public BimNodeParent BimParentNode => _bimParentNode;

        public DataNodeBase CombineRendererNode => _combineRendererParentNode;

        private void Reset()
        {
            Data = new MetaDataBase();
        }

        public IMetaDataNode<DataNodeBase, IMetaData> AddBimParentNode(IDataNodeBuilder bimParentNodeBuilder)
        {
            _bimParentNode = (BimNodeParent)AddNode(bimParentNodeBuilder, Option<DataNodeBase>.None);
            return _bimParentNode;
        }

        public void AddCombineRender(ICombineRendererBuilder combineRendererBuilder)
        {
            AddNode(combineRendererBuilder, Option<DataNodeBase>.None);
        }
    }
}

