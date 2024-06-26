// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace Htmxor.DependencyInjection;

internal class DefaultAntiforgeryStateProvider : AntiforgeryStateProvider, IDisposable
{
	private const string PersistenceKey = $"__internal__{nameof(AntiforgeryRequestToken)}";
	private readonly PersistingComponentStateSubscription subscription;
	private readonly AntiforgeryRequestToken? currentToken;

	[UnconditionalSuppressMessage(
		"Trimming",
		"IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
		Justification = $"{nameof(DefaultAntiforgeryStateProvider)} uses the {nameof(PersistentComponentState)} APIs to deserialize the token, which are already annotated.")]
	public DefaultAntiforgeryStateProvider(PersistentComponentState state)
	{
		// Automatically flow the Request token to server/wasm through
		// persistent component state. This guarantees that the antiforgery
		// token is available on the interactive components, even when they
		// don't have access to the request.
		subscription = state.RegisterOnPersisting(() =>
		{
			state.PersistAsJson(PersistenceKey, GetAntiforgeryToken());
			return Task.CompletedTask;
		}, RenderMode.InteractiveAuto);

		state.TryTakeFromJson(PersistenceKey, out currentToken);
	}

	/// <inheritdoc />
	public override AntiforgeryRequestToken? GetAntiforgeryToken() => currentToken;

	/// <inheritdoc />
	public void Dispose() => subscription.Dispose();
}
