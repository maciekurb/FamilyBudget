using Microsoft.Extensions.DependencyInjection;

namespace FamilyBudget.Infrastructure.DependencyInjection;

public static class DependencyInjectionInstaller
{
    public static IServiceCollection AddInjectables(this IServiceCollection services)
    {
        var allTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName != null && a.FullName.StartsWith("FamilyBudget") && !a.FullName.Contains("Tests"))
            .SelectMany(a => a.GetTypes())
            .ToList();

        var scopedTypes = allTypes
            .Where(t => t.GetCustomAttributes(typeof(InjectableAttribute), false).Any());

        AddTypes(services, scopedTypes, allTypes, Lifetime.Transient);

        return services;
    }

    private static void AddTypes(IServiceCollection services, IEnumerable<Type> types, List<Type> allTypes,
        Lifetime lifetime)
    {
        foreach (var type in types)
        {
            if (type.IsClass)
                AddClassType(services, lifetime, type);

            if (type.IsInterface)
                AddInterfaceType(services, allTypes, lifetime, type);
        }
    }

    private static void AddClassType(IServiceCollection services, Lifetime lifetime, Type type)
    {
        if (lifetime == Lifetime.Transient)
            services.AddTransient(type);
        else if (lifetime == Lifetime.Singleton)
            services.AddSingleton(type);
    }

    private static void AddInterfaceType(IServiceCollection services, List<Type> allTypes, Lifetime lifetime, Type type)
    {
        var implementations = allTypes
            .Where(c => c.IsClass && c.GetInterfaces().Contains(type))
            .ToList();

        if (!implementations.Any())
            throw new ArgumentException($"No implementing classes found for Injectable interface. {type.FullName}.");

        if (implementations.Count() > 1)
            throw new ArgumentException(
                $"There can be only one implementation for Injectable interface {type.FullName}. Found {implementations.Count}.");

        var implementation = implementations.Single();

        if (lifetime == Lifetime.Transient)
            services.AddTransient(type, implementation);
        else if (lifetime == Lifetime.Singleton)
            services.AddSingleton(type, implementation);
    }

    private enum Lifetime
    {
        Transient = 0,
        Singleton = 1
    }
}