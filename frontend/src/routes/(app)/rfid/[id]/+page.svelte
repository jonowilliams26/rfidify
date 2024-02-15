<script lang="ts">
	import type { PageData } from './$types';
	import { RFID, EmptyStateRFID } from '$lib/components/rfid';

	export let data: PageData;
</script>

{#if !data.rfid.spotifyItem}
	<EmptyStateRFID rfid={data.rfid.id} />
{:else}
	<RFID rfid={data.rfid} />
{/if}