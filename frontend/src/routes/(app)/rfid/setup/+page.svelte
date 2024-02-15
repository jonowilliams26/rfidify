<script lang="ts">
	import { onDestroy, onMount } from 'svelte';
	import type { PageData } from './$types';
	import { Margin } from 'radix-icons-svelte';
	import * as signalR from '@microsoft/signalr';
	import { PUBLIC_API_URL } from '$env/static/public';
	import { goto } from '$app/navigation';

	type RFIDScannedMessage = {
		id: string;
	};

	export let data: PageData;

	onMount(() => {
		console.log('RFID Hub: Connecting...');
		const connection = new signalR.HubConnectionBuilder()
			.withUrl(`${PUBLIC_API_URL}/rfidhub`, {
				withCredentials: false
			})
			.withAutomaticReconnect()
			.build();

		connection.on('RFIDScanned', (message: RFIDScannedMessage) => {
			console.log('RFID Hub: On RFIDScanned. RFID:', message.id);
            goto(`/rfid/${message.id}`);
		});

		// Need to use callbacks since cant have a cleanup function using async
		// see: https://svelte.dev/docs/svelte#onmount
		connection
			.start()
			.then(() => {
				console.log('RFID Hub: Connection started');
			})
			.catch((err) => {
				console.error('RFID Hub: Connection failed', err);
			});

		return () => {
			console.log('RFID Hub: Cleaning up...');
			connection
				?.stop()
				.then(() => {
					console.log('RFID Hub: Connection stopped');
				})
				.catch((err) => {
					console.error('RFID Hub: Connection failed', err);
				});
		};
	});
</script>

<div class="mx-auto flex max-w-sm flex-col items-center pt-12">
	<Margin class="text-muted-foreground -mb-12 ml-8 h-10 w-10 animate-ping" />
	<img src="/rfid-scan.svg" alt="rfid-scan" class="z-10" />
	<p class="text-muted-foreground animate-pulse pt-4 text-sm font-semibold">
		Please scan an RFID...
	</p>
</div>