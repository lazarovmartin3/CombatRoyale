using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureDatabase : MonoBehaviour
{
    [Serializable]
    public enum StructureType
    {
        None,
        Castle,
        Wall,
        GateWall,
        Stables,
        Barracks,
        Farm
    }

    [Serializable]
    public struct StructuresData
    {
        public StructureType struct_type;
        public GameObject[] structures_construct_prefabs;
    }

    public List<StructuresData> stucture_database;
    public List<StructuresData> stucture_on_field;

    public GameObject GetStucture(List<StructuresData> structureList,StructureType struct_type, int const_phase)
    {
        foreach (var Entry in structureList)
        {
            if (Entry.struct_type == struct_type)
            {
                return Entry.structures_construct_prefabs[const_phase];
            }
        }
        return null;
    }
}
