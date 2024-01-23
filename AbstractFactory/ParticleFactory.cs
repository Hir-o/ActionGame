using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class ParticleFactory
{
    private static Dictionary<string, Type> particlesByName;
    private static bool IsInitialized => particlesByName != null;

    private static void InitializeFactory()
    {
        if (IsInitialized)
            return;

        var particleTypes = Assembly.GetAssembly(typeof(ParticleAbstractFactory)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ParticleAbstractFactory)));

        particlesByName = new Dictionary<string, Type>();

        foreach (var type in particleTypes)
        {
            var tempEffect = Activator.CreateInstance(type) as ParticleAbstractFactory;
            particlesByName.Add(tempEffect.Name, type);
        }
    }

    public static ParticleAbstractFactory GetParticle(string particleType)
    {
        InitializeFactory();

        if (particlesByName.ContainsKey(particleType))
        {
            Type type = particlesByName[particleType];
            var particle = Activator.CreateInstance(type) as ParticleAbstractFactory;
            return particle;
        }

        return null;
    }

    internal static IEnumerable<string> GetParticleNames()
    {
        InitializeFactory();
        return particlesByName.Keys;
    }
}
