using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Cerberos.TfaImporter;

public static class Extensions
{
    private static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

    public static Window? FindWindowByViewModel(this INotifyPropertyChanged viewModel) =>
        Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext));
    
    /// <summary>
    /// This is a extension class of enum
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static string GetEnumDisplayName(this Enum enumType)
    {
        return enumType?.GetType().GetMember(enumType.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            .Name;
    }

    /// <summary>
    /// Parse string value to specified <see cref="TEnum"/>
    /// </summary>
    /// <param name="souce">source value</param>
    /// <param name="defaultValue">default enum result if source value is not found.</param>
    /// <typeparam name="TEnum">specified enum</typeparam>
    /// <returns></returns>
    public static TEnum ParseDisplayNameToEnum<TEnum>(this string souce, TEnum defaultValue) where TEnum : struct, Enum
    {
        var enumValues = Enum.GetValues<TEnum>();
        
        if(!enumValues.Any(obj =>
               obj.GetEnumDisplayName().Equals(souce, StringComparison.CurrentCultureIgnoreCase)))
            return defaultValue;

        var result = enumValues.First(obj =>
            obj.GetEnumDisplayName().Equals(souce, StringComparison.CurrentCultureIgnoreCase));

        return result;
    }
}