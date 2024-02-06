<script lang="ts">
	import { onMount, afterUpdate } from 'svelte';
	import { goto, beforeNavigate } from '$app/navigation';
	import * as signalR from '@microsoft/signalr';
	import { Margin } from 'radix-icons-svelte';
	type RFIDScannedMessage = {
		id: string;
	};
	let connection: signalR.HubConnection | undefined = undefined;

	onMount(async () => {
		connection = new signalR.HubConnectionBuilder()
			.withUrl('http://localhost:5293/rfidhub', {
				withCredentials: false
			})
			.withAutomaticReconnect()
			.build();

		connection.on('RFIDScanned', (rfid: RFIDScannedMessage) => {
			try {
				console.log('Hub Connection On RFIDScanned', rfid);
				goto(`/rfid/${rfid.id}`);
			} catch (e) {
				console.log('Error in Hub Connection On RFIDScanned');
			}
		});

		try {
			await connection.start();
		} catch (error) {
			console.log('Error in Connecting to RFID Hub', error);
		}
	});

	beforeNavigate(async (nav) => {
		if (nav.to?.url.pathname === '/rfid/scan') {
			return;
		}

		try {
			await connection?.stop();
		} catch (e) {
			console.log('Error in Disconnecting from RFID Hub', e);
		}
	});
</script>

<div class="mx-auto flex flex-grow flex-col items-center">
	<Margin class="ml-8 h-16 w-16 animate-ping text-muted-foreground opacity-40" />
	<img src="/scan-image.svg" class="z-10 -m-16 w-96" alt="scan-rfid" />
	<p class="mt-20 text-sm font-semibold text-muted-foreground">Please scan an RFID tag...</p>
</div>
