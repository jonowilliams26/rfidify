<script lang="ts">
	import type { LayoutData } from './$types';
	import { SpotifyItem } from '$lib/components/spotify';
	export let data: LayoutData;
</script>

{#if data.rfid.spotifyItem}
	<div class="pb-6">
		<SpotifyItem spotifyItem={data.rfid.spotifyItem} rfid={data.rfid.id} />
	</div>
{/if}

<slot />
