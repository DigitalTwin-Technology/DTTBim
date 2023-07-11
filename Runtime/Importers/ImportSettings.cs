// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEngine;
using System;

namespace DTTBim.Importers
{
    [Serializable]
    public class ImportSettings
    {
        [SerializeField]
        [Tooltip("True to blend meshes by materials")]
        private bool _blendMeshByMaterial;

        [SerializeField]
        [Tooltip("True to add collider to the instantiated objects")]
        private bool _addCollider;

        [SerializeField]
        [Tooltip("Instantiated objects will be assigned to this layer")]
        private int _layer;

        [SerializeField]
        [Tooltip("Apply StaticBatchingUtility.Combine after the loading")]
        private bool _applyStaticBatching;

        /// <summary>
        /// True to add collider to the nstantiated objects.
        /// </summary>
        public bool AddCollider { get => _addCollider; set => _addCollider = value; }

        /// <summary>
        /// Instantiated objects will be assigned to this layer.
        /// </summary>
        public int Layer { get => _layer; set => _layer = value; }

        /// <summary>
        /// Instantiated objects will be assigned to this layer.
        /// </summary>
        public bool ApplyStaticBatching { get => _applyStaticBatching; set => _applyStaticBatching = value; }
    }
}
