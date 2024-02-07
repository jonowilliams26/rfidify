<script lang="ts">
	import type { SpotifyItem } from '$lib/api/types/spotify';
	import { Person } from 'radix-icons-svelte';

	export let item: SpotifyItem;
</script>

{#if item.type === 'Track' || item.type === 'Album'}
	<div class="flex space-x-1">
		<Person />
		<span>{item.artists.map((x) => x.name)}</span>
	</div>
{/if}
