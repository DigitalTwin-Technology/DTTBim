// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;
using DTTUnityCore;
using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

namespace DTTBim.DataStructs
{
    [Serializable]
    public class BimNodeParent : DataNodeBase, IBimParentNode<DataNodeBase, BimNodeObject>
    {
        Dictionary<string, BimNodeObject> _idNodesObjectsDictionary;
        Dictionary<uint, BimNodeObject> _nodeIndexObjectsDictionary;


        [SerializeField]
        private MetaDataName _metaDataName;

        public IMetaData MetaData { get => _metaDataName; set => _metaDataName = (MetaDataName)value; }

        public BimNodeObject this[string id]
        { 
            get
            {
                if (_idNodesObjectsDictionary == null) 
                {
                    _idNodesObjectsDictionary = new Dictionary<string, BimNodeObject>();
                }
                return _idNodesObjectsDictionary[id];
            }
            set 
            {
                if (_idNodesObjectsDictionary == null)
                {
                    _idNodesObjectsDictionary = new Dictionary<string, BimNodeObject>();
                }
                _idNodesObjectsDictionary[id] = value;
            }
        }

        public BimNodeObject this[uint nodeIndex]
        {
            get
            {
                if (_nodeIndexObjectsDictionary == null)
                {
                    _nodeIndexObjectsDictionary = new Dictionary<uint, BimNodeObject>();
                    return null;
                }
                else if(_nodeIndexObjectsDictionary.ContainsKey(nodeIndex))
                {
                    return _nodeIndexObjectsDictionary[nodeIndex];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_nodeIndexObjectsDictionary == null)
                {
                    _nodeIndexObjectsDictionary = new Dictionary<uint, BimNodeObject>();
                }
                _nodeIndexObjectsDictionary[nodeIndex] = value;
            }
        }

        public void AddBimDataNodeById(string nodeId, string parentId, uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale, int layer = 0)
        {
            if(_idNodesObjectsDictionary == null) { _idNodesObjectsDictionary = new Dictionary<string, BimNodeObject>(); }
            if(_nodeIndexObjectsDictionary == null) { _nodeIndexObjectsDictionary = new Dictionary<uint, BimNodeObject>(); }

            if (string.IsNullOrEmpty(parentId))
            {
                _idNodesObjectsDictionary[nodeId] = (BimNodeObject)AddNode(new BimNodeObjectBuilder(nodeId, nodeIndex, parentIndex, position, rotation, scale, layer), Option<DataNodeBase>.None);
            }
            else
            {
                _idNodesObjectsDictionary[nodeId] = (BimNodeObject)_idNodesObjectsDictionary[parentId].AddBimDataNode(nodeId, nodeIndex, position, rotation, scale, layer);
                _nodeIndexObjectsDictionary[nodeIndex] = _idNodesObjectsDictionary[nodeId];
            }
        }

        public void AddBimDataNodeByNodeIndex(uint newNodeIndex, uint? parentIndex, string nodeId, string parentId, Vector3 position, Quaternion rotation, Vector3 scale, int layer)
        {
            if (_idNodesObjectsDictionary == null) { _idNodesObjectsDictionary = new Dictionary<string, BimNodeObject>(); }
            if (_nodeIndexObjectsDictionary == null) { _nodeIndexObjectsDictionary = new Dictionary<uint, BimNodeObject>(); }

            if (parentIndex.HasValue)
            {
                _nodeIndexObjectsDictionary[newNodeIndex] = (BimNodeObject)_nodeIndexObjectsDictionary[parentIndex.Value].AddBimDataNode(nodeId, newNodeIndex, position, rotation, scale, layer);
                _idNodesObjectsDictionary[nodeId] = _nodeIndexObjectsDictionary[newNodeIndex];
            }
            else
            {
                _nodeIndexObjectsDictionary[newNodeIndex] = (BimNodeObject)AddNode(new BimNodeObjectBuilder(nodeId, newNodeIndex, parentIndex, position, rotation, scale, layer), Option<DataNodeBase>.None);
                _idNodesObjectsDictionary[nodeId] = _nodeIndexObjectsDictionary[newNodeIndex];
            }
        }

        public void AddMesh(string id, Mesh sharedMesh, Material[] sharedMaterialsArrays, Option<ICombineRendererBuilder> combineRendererBuilder, bool addMeshCollider = false)
        {
            GuardsClauses.ArgumentStringNullOrEmpty(id, nameof(id));
            GuardsClauses.ArgumentNotNull(sharedMesh, nameof(sharedMesh));
            GuardsClauses.ArgumentNotNull(sharedMaterialsArrays, nameof(sharedMaterialsArrays));

            //TODO: Resolve a empty sharedMaterialArray with a existing mesh

            MeshFilter meshFilter = _idNodesObjectsDictionary[id].GetComponent<MeshFilter>();
            if(meshFilter == null)
            {
                meshFilter = _idNodesObjectsDictionary[id].AddComponent<MeshFilter>();
            }
            meshFilter.sharedMesh = sharedMesh;

            if (addMeshCollider)
            {
                MeshCollider meshCollider = _idNodesObjectsDictionary[id].GetComponent<MeshCollider>();
                if(meshCollider == null)
                {
                    meshCollider = _idNodesObjectsDictionary[id].AddComponent<MeshCollider>();
                }
                meshCollider.sharedMesh = sharedMesh;
            }

            _ = combineRendererBuilder.Match(some =>
            {
                for(int i=0; i< sharedMaterialsArrays.Length; i++)
                {
                    some.AddCombineInstance(sharedMaterialsArrays[i], sharedMesh, _idNodesObjectsDictionary[id].transform.localToWorldMatrix);
                }
                return 0;
            },
            () =>
            {
                MeshRenderer meshRenderer = _idNodesObjectsDictionary[id].GetComponent<MeshRenderer>();

                if( meshRenderer == null)
                {
                    meshRenderer = _idNodesObjectsDictionary[id].AddComponent<MeshRenderer>();
                }
                meshRenderer.materials = sharedMaterialsArrays;
                return 0;
            });
        }

        public void AddMesh(uint nodeIndex, Mesh sharedMesh, Material[] sharedMaterialsArrays, Option<ICombineRendererBuilder> combineRendererBuilder, bool addMeshCollider = false)
        {
            GuardsClauses.ArgumentNotNull(sharedMesh, nameof(sharedMesh));
            GuardsClauses.ArgumentNotNull(sharedMaterialsArrays, nameof(sharedMaterialsArrays));

            MeshFilter meshFilter = _nodeIndexObjectsDictionary[nodeIndex].GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = _nodeIndexObjectsDictionary[nodeIndex].AddComponent<MeshFilter>();
            }
            meshFilter.sharedMesh = sharedMesh;

            if (addMeshCollider)
            {
                MeshCollider meshCollider = _nodeIndexObjectsDictionary[nodeIndex].GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    meshCollider = _nodeIndexObjectsDictionary[nodeIndex].AddComponent<MeshCollider>();
                }
                meshCollider.sharedMesh = sharedMesh;
            }

            _ = combineRendererBuilder.Match(some =>
            {
                for (int i = 0; i < sharedMaterialsArrays.Length; i++)
                {
                    some.AddCombineInstance(sharedMaterialsArrays[i], sharedMesh, _nodeIndexObjectsDictionary[nodeIndex].transform.localToWorldMatrix);
                }
                return 0;
            },
            () =>
            {
                MeshRenderer meshRenderer = _nodeIndexObjectsDictionary[nodeIndex].GetComponent<MeshRenderer>();

                if (meshRenderer == null)
                {
                    meshRenderer = _nodeIndexObjectsDictionary[nodeIndex].AddComponent<MeshRenderer>();
                }
                meshRenderer.materials = sharedMaterialsArrays;
                return 0;
            });
        }
    }
}

