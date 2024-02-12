<script lang="ts">
	import * as Card from '$lib/components/ui/card';
	import Label from '$lib/components/ui/label/label.svelte';
	import Input from '$lib/components/ui/input/input.svelte';
	import Button from '$lib/components/ui/button/button.svelte';
	import { buttonVariants } from '$lib/components/ui/button';
	import { cn } from '$lib/utils';
	import { setSpotifyCredentials } from '$lib/api/endpoints';
	import { Reload } from 'radix-icons-svelte';
	import { toast } from "svelte-sonner";

	let clientId = '';
	let clientSecret = '';
	let loading = false;
	$: disabled = !clientId || !clientSecret || loading;
	$: buttonText = loading ? 'Authorizing...' : 'Authorize';

	async function authorize() {
		loading = true;
		
		const response = await setSpotifyCredentials(fetch, {
			clientId,
			clientSecret,
			redirectUri: `${window.location.origin}/spotify/setup/callback`
		});
		
		if (!response.ok){
			toast.error("Sorry, something went wrong. Please try again.");
			loading = false;
			return;
		}

		window.location.href = response.data.authorizationUri;
	}
</script>

<div class="mx-auto flex h-screen items-center justify-center">
	<Card.Root>
		<Card.Header class="text-muted-foreground items-center space-y-0 text-sm">
			<img src="/spotify-logo-with-name.png" alt="Spotify" class="w-64" />
			<p class="pt-2">Please enter your Spotify Client ID and Secret.</p>
			<p>
				You can get your credentials at <a
					href="https://developer.spotify.com/documentation/web-api"
					class={cn(buttonVariants({ variant: 'link' }), 'px-0 py-0')}>Spotify Web API docs.</a
				>
			</p>
		</Card.Header>
		<Card.Content>
			<Label for="clientId">Client ID</Label>
			<Input
				id="clientId"
				name="clientId"
				type="text"
				bind:value={clientId}
				placeholder="Client id"
			/>
			<div class="py-1.5"></div>
			<Label for="clientSecret">Client Secret</Label>
			<Input
				id="clientSecret"
				name="clientSecret"
				type="password"
				bind:value={clientSecret}
				placeholder="Client secret"
			/>
			<div class="py-1.5"></div>
			<Button type="submit" class="w-full" {disabled} on:click={authorize}>
				{#if loading}
					<Reload class="mr-2 h-4 w-4 animate-spin" />
				{/if}
				{buttonText}
			</Button>
		</Card.Content>
	</Card.Root>
</div>
