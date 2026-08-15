using System;

namespace Engine.Debugging;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DontInspectAttribute : Attribute
{
    
}