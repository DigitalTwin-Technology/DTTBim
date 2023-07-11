// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using GLTFast;
using DTTUnityCore;
using DTTUnityCore.Functional;
using DTTUnityCore.DataStructs;
using DTTBim.DataStructs;

namespace DTTBim.Importers
{
    public class DTTglTFastCustomInstantiator : IInstantiator
    {
        Dictionary<uint, string> _nodeIndexIdDictionary;
        private uint _maxID;

        private IGltfReadable _gltf;
        private DataNodeBase _mainParent;
        private Option<ICombineRendererBuilder> _combineRenderBuilder;
        private ImportSettings _instantiatorSettings;

        private BimNodeScene _sceneParent;


        public DTTglTFastCustomInstantiator(IGltfReadable gltf,
            DataNodeBase sceneListParent,
            ImportSettings instantiatorSettings,
            Option<ICombineRendererBuilder> cobineRendererBuilder,
            int sceneIndex = 0)
        {
            GuardsClauses.ArgumentNotNull(gltf, nameof(gltf));
            GuardsClauses.ArgumentNotNull(sceneListParent, nameof(sceneListParent));
            GuardsClauses.ArgumentNotNull(instantiatorSettings, nameof(instantiatorSettings));

            _gltf = gltf;
            _mainParent = sceneListParent;

            _combineRenderBuilder = cobineRendererBuilder;
            _instantiatorSettings = instantiatorSettings;

            string sceneName = string.IsNullOrEmpty(gltf.GetSourceScene(sceneIndex).name) ? $"Scene {sceneIndex}" : gltf.GetSourceScene(sceneIndex).name;

            _sceneParent = (BimNodeScene)_mainParent.AddNode(new BimNodeSceneBuilder(sceneName, new BimParentNodeBuilder("Nodes")),
                            Option<DataNodeBase>.None);

            _nodeIndexIdDictionary = new Dictionary<uint, string>();
            _maxID = (uint)_gltf.GetSourceScene(sceneIndex).nodes.Length;
        }

        #region IInstantiator Implementation

        public void BeginScene(string name, uint[] rootNodeIndices)
        {

        }

        /// <inheritdoc />
        public void CreateNode(uint nodeIndex, uint? parentIndex, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            string nodeId = _gltf.GetSourceRoot().nodes[nodeIndex].name;

            _sceneParent.BimParentNode.AddBimDataNodeByNodeIndex(nodeIndex, parentIndex,
                nodeId,
                parentIndex.HasValue ? _gltf.GetSourceRoot().nodes[parentIndex.Value].name : string.Empty,
                position,
                rotation,
                scale,
                _instantiatorSettings.Layer);

            _nodeIndexIdDictionary[nodeIndex] = nodeId;
        }

        public void EndScene(uint[] rootNodeIndices)
        {
            _ = _combineRenderBuilder.Match(some =>
            {
                _sceneParent.AddCombineRender(some);
                return 0;
            },
            () =>
            {
                if (_instantiatorSettings.ApplyStaticBatching)
                {
                    StaticBatchingUtility.Combine(_sceneParent.gameObject);
                }
                return 0;
            });
        }

        public void SetNodeName(uint nodeIndex, string name)
        {
            _sceneParent.BimParentNode[nodeIndex].SetNodeId(name ?? $"Node-{nodeIndex}");
        }

        public void AddPrimitive(uint nodeIndex, string meshName, Mesh mesh, int[] materialIndices, uint[] joints = null, uint? rootJoint = null, float[] morphTargetWeights = null, int primitiveNumeration = 0)
        {
            if (mesh == null) { return; }

            string id = primitiveNumeration == 0 ? _gltf.GetSourceRoot().nodes[nodeIndex].name : meshName;

            Material[] materials = new Material[materialIndices.Length];
            for (var index = 0; index < materials.Length; index++)
            {
                Material material = _gltf.GetMaterial(materialIndices[index]) ?? _gltf.GetDefaultMaterial();
                materials[index] = material;
            }

            if (primitiveNumeration == 0)
            {
                _sceneParent.BimParentNode.AddMesh(nodeIndex,
                    mesh,
                    materials,
                    _combineRenderBuilder,
                    _instantiatorSettings.AddCollider);
            }
            else
            {
                _sceneParent.BimParentNode.AddBimDataNodeById(meshName,
                _gltf.GetSourceRoot().nodes[nodeIndex].name,
                _maxID,
                nodeIndex,
                Vector3.zero,
                Quaternion.identity,
                Vector3.one,
                _instantiatorSettings.Layer);

                _sceneParent.BimParentNode.AddMesh(meshName,
                     mesh,
                     materials,
                     _combineRenderBuilder,
                     _instantiatorSettings.AddCollider);
                _maxID++;
            }
        }

        public void AddPrimitiveInstanced(uint nodeIndex, string meshName, Mesh mesh, int[] materialIndices, uint instanceCount, NativeArray<Vector3>? positions, NativeArray<Quaternion>? rotations, NativeArray<Vector3>? scales, int primitiveNumeration = 0)
        {
            //TODO
        }

        public void AddAnimation(AnimationClip[] animationClips)
        {
            //TODO
        }

        public void AddCamera(uint nodeIndex, uint cameraIndex)
        {
            //TODO
        }

        public void AddLightPunctual(uint nodeIndex, uint lightIndex)
        {
            //TODO
        }

        #endregion
    }
}


