// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;
using DTTUnityCore;
using System;

namespace DTTBim.DataStructs
{
    [Serializable]
    public class BimNodeParent : DataNodeBase, IBimParentNode<DataNodeBase, BimNodeObject>
    {
        [SerializeField]
        private MetaDataName _metaDataName;

        public BimNodeParent(string name)
        {
            _metaDataName = new MetaDataName(name);
        }

        public BimNodeObject this[int index] 
        { 
            get => (BimNodeObject)Childs[index]; 
            set => Childs[index] = value; 
        }

        public IMetaData MetaData { get => _metaDataName; set => _metaDataName = (MetaDataName)value; }

        public void AddBimDataNode(string id, string parentId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer = 0, bool useLocalPostion = true)
        {
            if(string.IsNullOrEmpty(parentId))
            {
                int childNodeIndex = Childs.FindIndex(0, node => ((MetaDataBimNode)node.Data).Id == id);
                if (childNodeIndex == -1) 
                {
                    AddNode(new BimNodeObjectBuilder(nodeIndex, id, parentIndex, position, rotation, scale, layer), Option<DataNodeBase>.None);
                }
            }
            else
            {
                BimNodeObject bimNodeObject = FindNodeObjectById<BimNodeObject>(parentId);

                if(bimNodeObject != null)
                {
                    bimNodeObject.AddBimDataNode(nodeIndex, id, position, rotation, scale, layer);
                }
            }
        }

        public void AddMesh(string id, Mesh sharedMesh, Material[] sharedMaterialsArrays, Option<ICombineRendererBuilder> combineRendererBuilder, bool addMeshCollider = false)
        {
            GuardsClauses.ArgumentStringNullOrEmpty(id, nameof(id));
            GuardsClauses.ArgumentNotNull(sharedMesh, nameof(sharedMesh));
            GuardsClauses.ArgumentNotNull(sharedMaterialsArrays, nameof(sharedMaterialsArrays));

            BimNodeObject bimNodeObject = FindNodeObjectById<BimNodeObject>(id);
            if(bimNodeObject)
            {
                //TODO: Resolve a empty sharedMaterialArray with a existing mesh

                MeshFilter meshFilter = bimNodeObject.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = sharedMesh;

                if (addMeshCollider)
                {
                    MeshCollider meshCollider = bimNodeObject.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = sharedMesh;
                }

                _ = combineRendererBuilder.Match(some =>
                {
                    for(int i=0; i< sharedMaterialsArrays.Length; i++)
                    {
                        some.AddCombineInstance(sharedMaterialsArrays[i], sharedMesh, bimNodeObject.transform.localToWorldMatrix);
                    }
                    return 0;
                },
                () =>
                {
                    MeshRenderer meshRenderer = bimNodeObject.AddComponent<MeshRenderer>();
                    meshRenderer.materials = sharedMaterialsArrays;
                    return 0;
                });
            }
        }

        public int FindChildIndexNodeByNodeId(string id)
        {
            return Childs.FindIndex(0, node => ((MetaDataBimNode)node.Data).Id == id);
        }

        public NodeObjectType FindNodeObjectById<NodeObjectType>(string id) where NodeObjectType : DataNodeBase
        {
            NodeObjectType bimNodeObject = (NodeObjectType)Childs.Find(node => ((MetaDataBimNode)node.Data).Id == id);

            if(bimNodeObject == null)
            {
                for(int i=0; i< Childs.Count; i++)
                {
                    bimNodeObject = (NodeObjectType)Childs[i].Childs.Find(node => ((MetaDataBimNode)node.Data).Id == id);
                    if(bimNodeObject != null)
                    {
                        return bimNodeObject;
                    }
                }
            }

            return bimNodeObject; 
        }
    }
}

