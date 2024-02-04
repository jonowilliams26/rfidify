<script lang="ts">
	import * as Card from '$lib/components/ui/card';
	import { Button } from '$lib/components/ui/button';
	import { Input } from '$lib/components/ui/input';
	import { Label } from '$lib/components/ui/label';
	import { setSpotifyCredentials } from '$lib/http/api';
    import { toastError } from '$lib/toasts';

	let clientId = '';
	let clientSecret = '';
	let loading = false;
	$: disabled = loading || !clientId || !clientSecret;

	async function authorize() {
        loading = true;

        const response = await setSpotifyCredentials({
            clientId,
            clientSecret,
            redirectUri: `${window.location.origin}/setup/callback`
        });

        if (!response.ok) {
            toastError();
            loading = false;
            return;
        }

        window.location.href = response.data.authorizationUri;
	}
</script>

<Card.Root class="m-auto max-w-lg">
	<Card.Header class="text-center">
		<Card.Title class="text-2xl">
			<img class="mx-auto h-16" src="spotify-logo-with-text.png" alt="RFIDify" />
		</Card.Title>
		<Card.Description>
			<p>Please enter your Spotify ClientID and Secret.</p>
			<p>
				To get your credentials visit the <Button href="/dashboard" variant="link" class="p-0"
					>Spotify Web API docs</Button
				>
			</p>
		</Card.Description>
	</Card.Header>
	<Card.CardContent class="flex flex-col space-y-6">
		<div>
			<Label for="clientId">Client ID</Label>
			<Input type="text" id="clientId" placeholder="Client ID" bind:value={clientId} />
		</div>

		<div>
			<Label for="clientSecret">Client Secret</Label>
			<Input
				type="password"
				id="clientSecret"
				placeholder="Client secret"
				bind:value={clientSecret}
			/>
		</div>

		<Button class="w-full" on:click={authorize} {disabled}>Authorize</Button>
	</Card.CardContent>
</Card.Root>
