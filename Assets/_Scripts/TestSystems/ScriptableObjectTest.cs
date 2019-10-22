using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public float Value;
}

public class DumbEnemy : MonoBehaviour
{
    public FloatVariable MaxHP;
    public FloatVariable MoveSpeed;
}

[Serializable]
public class FloatReference
{
    public bool UseConstant = true;
    public float ConstantValue;
    public FloatVariable Variable;

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}

public class DumbEnemy2 : MonoBehaviour
{
    public FloatReference MaxHP;
    public FloatReference MoveSpeed;
}

