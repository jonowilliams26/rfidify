<script lang="ts">
	import type { LayoutData } from './$types';
	import { RFID, EmptyStateRFID } from '$lib/components/rfid';

	export let data: LayoutData;
	$: ({ rfid } = data);
</script>

{#if !data.rfid.spotifyItem}
	<EmptyStateRFID rfid={data.rfid.id} />
{:else}
	<RFID rfid={data.rfid} />
{/if}

<slot />
