<script lang="ts">
	import type { SpotifyItem } from '$lib/api/types/spotify';
	export let item: SpotifyItem;
</script>

{#if item.type === 'Playlist'}
	<span class="truncate">{item.description}</span>
{/if}
