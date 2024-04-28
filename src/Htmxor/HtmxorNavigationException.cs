﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Htmxor;

/// <summary>
/// Exception thrown to indicate that a navigation operation was requested during static service side rendering.
/// </summary>
public sealed class HtmxorNavigationException : NavigationException
{
    /// <summary>
    /// Gets the URI that was originally passed to the <see cref="NavigationManager"/>.
    /// </summary>
    public string RequestedLocation { get; }

    /// <summary>
    /// Gets the <see cref="NavigationOptions"/> that was originally passed to the <see cref="NavigationManager"/>.
    /// </summary>
    public NavigationOptions Options { get; }

    public HtmxorNavigationException(
        [StringSyntax("Uri")] string requestedUri,
        [StringSyntax("Uri")] string absoluteUri,
        in NavigationOptions options) : base(absoluteUri)
    {
        RequestedLocation = requestedUri;
        Options = options;
    }
}
