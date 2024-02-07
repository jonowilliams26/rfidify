<script lang="ts">
	import type { RFID } from '$lib/api/types/rfid';
	import Card from './Card.svelte';
	import Image from './Image.svelte';
	import DropdownMenu from './DropdownMenu.svelte';
	import Artists from './Artists.svelte';
	import TagId from './TagId.svelte';
	import Description from './Description.svelte';

	export let rfid: RFID;
	export let showOptionsMenu = true;
</script>

<Card>
	<Image item={rfid.spotifyItem} />
	<div class="flex w-full flex-col truncate">
		<div class="flex w-full items-start justify-between space-x-4 truncate">
			<h2 class="truncate pb-2 text-sm font-semibold">{rfid.spotifyItem.name}</h2>
			{#if showOptionsMenu}
				<DropdownMenu rfid={rfid.id} />
			{/if}
		</div>
		<div class="space-y-1 truncate text-xs text-muted-foreground">
			<Artists item={rfid.spotifyItem} />
			<Description item={rfid.spotifyItem} />
			<TagId rfid={rfid.id} />
		</div>
	</div>
</Card>
